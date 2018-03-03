using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace CommonCode.Animation
{
#if UNITY_EDITOR
	using UnityEditor;
#endif

	public class AnimationAssigner : MonoBehaviour
	{
#if UNITY_EDITOR
		public DefaultAsset folder;
#else
		public UnityEngine.Object folder;
#endif
		public AnimationController controller;

		public void Assign()
		{
			if (folder == null)
			{
				controller.groups = null;
			}
			else
			{
				var leftHand = TryGetAnimations("left", "Left", "left hand", "Left hand", "Left Hand");
				var rightHand = TryGetAnimations("right", "Right", "right hand", "Right hand", "Right Hand");
				var head = TryGetAnimations("head", "Head");
				
				controller.groups = new AnimationGroup[leftHand.Length];
				for (int i = 0; i < leftHand.Length; ++i)
				{
					controller.groups[i] = new AnimationGroup
					(
						leftHand[i].name,   // name
						leftHand[i],        // animations
						rightHand[i],
						head[i]
					);
				}

				controller.groups = SortGroups(controller.groups);
			}
		}

		private Animation[] TryGetAnimations(params string[] limbNames)
		{
			foreach (var limbName in limbNames)
			{
				try
				{
					return GetAnimations(limbName);
				}
				catch (Exception)
				{

				}
			}

			return null;
		}

		private Animation[] GetAnimations(string limb)
		{
			var path = AssetDatabase.GetAssetPath(folder) + "/" + limb;
			var fullPath = Application.dataPath + path.Remove(0, "Assets".Length);
			var files = Directory.GetFiles(fullPath);
			var animations = new Animation[files.Length / 2];

			for (int i = 0, j = 0; i < files.Length; i += 2, ++j)
			{
				files[i] = files[i].Replace('\\', '/');
				files[i] = "Assets" + files[i].Remove(0, Application.dataPath.Length);
				animations[j] = AssetDatabase.LoadAssetAtPath<Animation>(files[i]);
			}

			return animations;
		}

		private AnimationGroup[] SortGroups(AnimationGroup[] groups)
		{
			var shortestNameIndex = 0;
			for (var i = 0; i < groups.Length; ++i)
			{
				if (groups[i].name.Length < groups[shortestNameIndex].name.Length)
				{
					shortestNameIndex = i;
				}
			}

			var baseLength = groups[shortestNameIndex].name.Length;
			for (int i = 0; i < groups.Length; ++i)
			{
				if (groups[i].name.Length == baseLength)
				{
					groups[i].name = groups[i].name.Insert(groups[i].name.Length - 1, "0");
				}
			}

			return groups.OrderBy(a => a.name).ToArray();
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(AnimationAssigner))]
	public class AnimationAssignerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Assign"))
			{
				(target as AnimationAssigner).Assign();
			}
			
			var folder = (target as AnimationAssigner).folder;
			if (folder != null)
			{
				var path = AssetDatabase.GetAssetPath(folder);
				GUILayout.Label(path);
			}
			else
			{
				GUILayout.Label("No folder chosen");
			}

			EditorUtility.SetDirty(target);
		}
	}
}
#endif
