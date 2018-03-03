using UnityEditor;
using UnityEngine;
using CommonCode.Animation;
using CommonCode.Editor;
using System.Collections.Generic;

using Animation = CommonCode.Animation.Animation;

public class AnimationSeperateWindow : EditorWindow
{
	private Animation animation;
	private Animation[] animations;
	
	[MenuItem("Jake/Seperate Animation", priority = 400)]
	static void Init()
	{
		GetWindow<AnimationSeperateWindow>("Seperate").Show();
	}

	private Animation CreateAnimation(Animation completeAnimation, Frame[] frames)
	{
		var animation = CreateInstance<Animation>();
		animation.frames			= frames;
		animation.positionOffset	= completeAnimation.positionOffset;
		animation.rotationOffset	= completeAnimation.rotationOffset;
		animation.playbackSpeed		= completeAnimation.playbackSpeed;
		return animation;
	}

	private bool IsSeperatorFrame(Frame frame)
	{
		return	frame.position == Vector3.zero &&
				frame.rotation == Quaternion.identity;
	}

	private Animation[] SeperateAnimation(Animation animation)
	{
		var animations = new List<Animation>();
		
		var frames = new List<Frame>();
		foreach (var frame in animation.frames)
		{
			if (IsSeperatorFrame(frame))
			{
				if (frames.Count > 0)
				{
					animations.Add(CreateAnimation(animation, frames.ToArray()));
					frames = new List<Frame>();
				}
			}
			else
			{
				frames.Add(frame);
			}
		}
		
		// add animation after last seperator frame
		animations.Add(CreateAnimation(animation, frames.ToArray()));

		return animations.ToArray();
	}
	
	private void SaveAnimation(Animation animation, int i)
	{
		EditorUtil.SaveAsset(animation, "Animation " + i.ToString());
		AssetDatabase.SaveAssets();
	}
	
	void OnGUI()
	{
		animation = EditorGUILayout.ObjectField("Animation", animation, typeof(Animation), true) as Animation;

		animations = animation == null ? null : SeperateAnimation(animation);
		
		if (animations != null)
		{
			int i = 0;
			foreach (var a in animations)
			{
				var label = string.Format("Save animation {0}. {1:0.000} seconds long.", i, a.frames.Length / 60f);
				if (GUILayout.Button(label))
				{
					SaveAnimation(a, i);
				}
				
				++i;
			}

			if (GUILayout.Button("Save All"))
			{
				int j = 0;
				foreach(var a in animations)
				{
					SaveAnimation(a, j++);
				}
			}
		}
	}
}
