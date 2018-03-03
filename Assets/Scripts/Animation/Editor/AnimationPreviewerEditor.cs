namespace CommonCode.Animation
{
	using UnityEditor;
	using UnityEngine;
	//using System;

	[CustomEditor(typeof(AnimationPreviewer))]
	public class AnimationPreviewerEditor : Editor
	{
		AnimationController animationController;

		//private TransformLite[] originalTransforms;

		void OnEnable()
		{
			animationController = serializedObject.FindProperty("animationController").objectReferenceValue as AnimationController;
			////animationController.OnAnimationFinished += AnimationController_OnAnimationFinished;

			//if (animationController != null && animationController.players != null)
			//{
			//	originalTransforms = Array.ConvertAll(animationController.players, (p) => { return p.transform.ToLite(Space.Self); });
			//}
		}

		//private void AnimationController_OnAnimationFinished(AnimationController arg1, Animation arg2)
		//{
		//	for (int i = 0; i < originalTransforms.Length; ++i)
		//	{
		//		animationController.players[i].transform.FromLite(originalTransforms[i]);
		//	}
		//}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (animationController == null || animationController.groups == null)
				return;

			foreach (var group in animationController.groups)
			{
				if (GUILayout.Button(group.name))
				{
					animationController.PlayGroup(group.name);
				}
			}
		}
	}
}
