using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class AnimationCleanWindow : EditorWindow
{
	private CommonCode.Animation.Animation animation;
	private float maxDistance;
	private List<int> badFrames;

	[MenuItem("Jake/Clean Animation", priority = 400)]
	static void Init()
	{
		GetWindow<AnimationCleanWindow>("Clean").Show();
	}

	void OnGUI()
	{
		animation = EditorGUILayout.ObjectField("Animation", animation, typeof(CommonCode.Animation.Animation), true) as CommonCode.Animation.Animation;
		maxDistance = EditorGUILayout.FloatField("Max distance", maxDistance);

		if (animation != null && maxDistance > 0)
		{
			badFrames = new List<int>();
			for (int i = 0; i < animation.frames.Length - 1; ++i)
			{
				var frame = animation.frames[i];
				var nextFrame = animation.frames[i + 1];

				if (Vector3.Distance(frame.position, nextFrame.position) > maxDistance)
				{
					badFrames.Add(i + 1);
				}
			}
		}
		else
		{
			badFrames = null;
		}

		if (badFrames != null)
		{
			if (badFrames.Count > 0)
			{
				Selection.activeObject = animation;

				for (int i = 0; i < badFrames.Count; ++i)
				{
					var distance = Vector3.Distance(animation.frames[badFrames[i]].position, animation.frames[badFrames[i] - 1].position);

					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Format("{0} ({1:0.000})", badFrames[i], distance));
					if (i + 1 != badFrames.Count)
					{
						if (GUILayout.Button("Fix Flat"))
						{
							FixBadFrames(animation, badFrames[i], badFrames[i + 1]);
							//AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animation));
							//AssetDatabase.Refresh();

							EditorUtility.SetDirty(animation);
						}
						else if (GUILayout.Button("Fix Lerped"))
						{
							LerpBadFrames(animation, badFrames[i], badFrames[i + 1]);
							//AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animation));
							//AssetDatabase.Refresh();
							
							EditorUtility.SetDirty(animation);
						}
					}
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("All good!");
			}
		}
	}
	
	private void FixBadFrames(CommonCode.Animation.Animation animation, int startIndex, int endIndex)
	{
		var frames = animation.frames;
		for (int i = startIndex; i < endIndex; ++i)
		{
			frames[i] = frames[startIndex - 1];
		}
	}

	private void LerpBadFrames(CommonCode.Animation.Animation animation, int startIndex, int endIndex)
	{
		var frames = animation.frames;
		var length = (endIndex - startIndex) + 1;
		var startFrame = frames[startIndex - 1];
		var endFrame = frames[endIndex];

		for (int i = startIndex; i < endIndex; ++i)
		{
			var ratio = (float)((i - startIndex) + 1) / length;
			frames[i].position = Vector3	.Lerp(startFrame.position, endFrame.position, ratio);
			frames[i].rotation = Quaternion	.Lerp(startFrame.rotation, endFrame.rotation, ratio);
		}
	}
}
