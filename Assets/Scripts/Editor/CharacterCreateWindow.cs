using CommonCode.Animation;
#if USING_LIPSYNC
using RogoDigital.Lipsync;
#endif
#if USING_IK
using RootMotion.FinalIK;
#endif
using System;
using UnityEditor;
using UnityEngine;

namespace Jake
{
	using GameObjectExtensions;
	using VR;

	public class CharacterCreateWindow : EditorWindow
	{
		public enum Controller { None, Animation, OptiTrack, VR };

		GameObject character;
		Controller controller;
		DefaultAsset animationFolder;
		bool includeAudio;

		[MenuItem("Jake/Create Character", priority = 100)]
		public static void ShowWindow()
		{
			GetWindow<CharacterCreateWindow>("Create");
		}

		void OnGUI()
		{
			//EditorGUILayout.LabelField("Model");
			character = EditorGUILayout.ObjectField("Model", character, typeof(GameObject), false) as GameObject;
			
			controller = (Controller)EditorGUILayout.EnumPopup("Controller", controller);
			if (controller == Controller.Animation)
			{
				EditorGUILayout.Space();
				animationFolder = EditorGUILayout.ObjectField("Animation Folder", animationFolder, typeof(DefaultAsset), false) as DefaultAsset;
				includeAudio = EditorGUILayout.Toggle("Include Audio", includeAudio);

			}

			EditorGUILayout.Space();
			if (GUILayout.Button("Create"))
			{
				Create();
			}
		}

		private void Create()
		{
#if USING_IK
			// model 1
			var model = Instantiate(character);
			model.name = "Model";
			model.RemoveComponent<Animator>();
			var ik = model.AddComponent<FullBodyBipedIK>();

			// elbows
			var leftElbow = new GameObject("Left Elbow");
			leftElbow.SetPosition(new Vector3(-.25f, 1, -.1f));

			var rightElbow = new GameObject("Right Elbow");
			rightElbow.SetPosition(new Vector3(.25f, 1, -.1f));

			var elbows = new GameObject("Elbows");

			leftElbow.SetParent(elbows);
			rightElbow.SetParent(elbows);

			// hands 1
			var leftHand = new GameObject("Left Hand");
			var rightHand = new GameObject("Right Hand");
			var hands = new GameObject("Hands");
			if (controller == Controller.Animation)
			{
				leftHand.AddComponent<AnimationPlayer>();
				rightHand.AddComponent<AnimationPlayer>();
			}

			leftHand.SetParent(hands);
			rightHand.SetParent(hands);

			// head
			var headEffector = new GameObject("Head Effector");
			var modelHead = InitialiseHeadEffector(headEffector.AddComponent<FBBIKHeadEffector>(), ik, model);
			headEffector.SetPosition(modelHead.GetPosition());
			headEffector.SetRotation(modelHead.GetRotation());

			var head = new GameObject("Head");
			head.SetPosition(((modelHead.Child("mixamorig:LeftEye").GetPosition() + modelHead.Child("mixamorig:RightEye").GetPosition()) / 2) + new Vector3(0, 0, .025f));
			if (controller == Controller.OptiTrack || controller == Controller.VR)
			{
				var camera = head.AddComponent<Camera>();
				camera.nearClipPlane = .03f;
				head.AddComponent<FlareLayer>();
				head.AddComponent<AudioListener>();
			}
			else if (controller == Controller.Animation)
			{
				head.AddComponent<AnimationPlayer>();
				if (includeAudio)
				{
#if USING_LIPSYNC
					/*var lipSync = */
					head.AddComponent<LipSync>();
					/*var audioSource = */head.AddComponent<AudioSource>();

					//BlendSystemEditor.FindBlendSystems(lipSync);
					//BlendSystemEditor.ChangeBlendSystem(lipSync, 2);
					//(lipSync.blendSystem as BlendshapeBlendSystem).characterMesh = model.Child("Body").GetComponent<SkinnedMeshRenderer>();
					//lipSync.audioSource = audioSource;

					// could also assign presets.
#endif
				}
			}

			headEffector.SetParent(head);

			// hands 2
			hands.SetPosition(head.GetPosition());
			
			// offset
			var offset = new GameObject("Body Offset");
			model.SetParent(offset);
			head.SetParent(offset);
			hands.SetParent(offset);
			elbows.SetParent(offset);

			// container
			var container = new GameObject(character.name);
			switch (controller)
			{
				case Controller.VR:
					var vrInput = container.AddComponent<VRInput>();
					vrInput.leftHand = leftHand.transform;
					vrInput.rightHand = rightHand.transform;
					break;
				case Controller.Animation:
					var controller = container.AddComponent<AnimationController>();
					controller.players = new AnimationPlayer[]
					{
						leftHand.GetComponent<AnimationPlayer>(),
						rightHand.GetComponent<AnimationPlayer>(),
						head.GetComponent<AnimationPlayer>()
					};
					var previewer = container.AddComponent<AnimationPreviewer>();
					previewer.animationController = controller;
					var assigner = container.AddComponent<AnimationAssigner>();
					assigner.folder = animationFolder;
					assigner.controller = controller;
					assigner.Assign();
					break;
			}
			offset.SetParent(container);

			// model 2
			InitialiseIK(ik, container, leftHand, leftElbow, rightHand, rightElbow);
#else
			throw new Exception("No IK package found.");
#endif
		}
#if USING_IK
		private GameObject InitialiseHeadEffector(FBBIKHeadEffector headEffector, FullBodyBipedIK ik, GameObject model)
		{
			var spine0 = model.Child("mixamorig:Hips", "mixamorig:Spine");
			var spine1 = spine0.Child("mixamorig:Spine1");
			var spine2 = spine1.Child("mixamorig:Spine2");
			var neck = spine2.Child("mixamorig:Neck");
			var head = neck.Child("mixamorig:Head");

			headEffector.ik = ik;
			headEffector.bodyWeight = .85f;
			headEffector.thighWeight = .85f;
			headEffector.handsPullBody = false;
			headEffector.rotationWeight = 1;
			headEffector.bendBones = new FBBIKHeadEffector.BendBone[]
			{
				new FBBIKHeadEffector.BendBone(spine0.transform, .5f),
				new FBBIKHeadEffector.BendBone(spine1.transform, .7f),
				new FBBIKHeadEffector.BendBone(spine2.transform, .9f),
				new FBBIKHeadEffector.BendBone(neck.transform, 1)
			};
			headEffector.fixHead = true;
			headEffector.stretchBones = new Transform[]
			{
				spine0.transform,
				spine1.transform,
				spine2.transform,
				neck.transform,
				head.transform
			};

			return head;
		}

		private void InitialiseIK(FullBodyBipedIK ik, GameObject container, GameObject leftHand, GameObject leftElbow, GameObject rightHand, GameObject rightElbow)
		{
			ik.SetReferences(ik.references, ik.solver.rootNode);

			var solver = ik.solver;

			// body
			var bodyEffector = solver.bodyEffector;
			bodyEffector.target = container.transform;
			bodyEffector.positionWeight = 1;
			solver.spineStiffness = 0;
			solver.pullBodyVertical = 0;

			// left hand
			var leftHandEffector = solver.leftHandEffector;
			var leftHandChain = solver.leftArmChain;
			leftHandEffector.target = leftHand.transform;
			leftHandEffector.positionWeight = 1;
			leftHandEffector.rotationWeight = 1;
			leftHandChain.pull = 0;
			leftHandChain.bendConstraint.bendGoal = leftElbow.transform;
			leftHandChain.bendConstraint.weight = 1;

			// right hand
			var rightHandEffector = solver.rightHandEffector;
			var rightHandChain = solver.rightArmChain;
			rightHandEffector.target = rightHand.transform;
			rightHandEffector.positionWeight = 1;
			rightHandEffector.rotationWeight = 1;
			rightHandChain.pull = 0;
			rightHandChain.bendConstraint.bendGoal = rightElbow.transform;
			rightHandChain.bendConstraint.weight = 1;
		}
#endif
	}
}
