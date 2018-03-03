using UnityEngine;
using System;

namespace CommonCode.Animation
{
	public class AnimationController : MonoBehaviour
	{
		public AnimationPlayer[]	players	= null;
		public AnimationGroup[]		groups	= null;

		public event Action<AnimationController, Animation> OnAnimationStarted;
		public event Action<AnimationController, Animation> OnAnimationFinished;
		
		void Awake()
		{
			if (players != null && players.Length > 0)
			{
				players[0].OnAnimationStarted	+= (ap, a) => { if (OnAnimationStarted	!= null) { OnAnimationStarted	(this, a); } };
				players[0].OnAnimationFinished	+= (ap, a) => { if (OnAnimationFinished	!= null) { OnAnimationFinished	(this, a); } };
			}
		}

		public bool IsPlaying
		{
			get
			{
				return Array.Exists(players, p => p.IsPlaying);
			}
		}

		public void PlayGroup(int index)
		{
			if (index >= groups.Length)
			{
				Debug.LogWarningFormat("Animation group index {0} exceeds array length of {1}", index, groups.Length);
				return;
			}

			PlayAnimations(groups[index]);
		}

		public void PlayGroup(string name)
		{
			for (int i = 0; i < groups.Length; ++i)
			{
				if (groups[i].name == name)
				{
					PlayGroup(i);
					return;
				}
			}

			Debug.LogWarning("No animation group with name: " + name);
		}

		public int GetPlayingGroupIndex()
		{
			var a = players[0].PlayingAnimation;
			if (a == null)
			{
				return -1;
			}

			var i = 0;
			foreach (var g in groups)
			{
				if (g.animations[0].name == a.name)
				{
					return i;
				}
				else
				{
					++i;
				}
			}

			throw new Exception("How did I get here?");
		}

		private void PlayAnimations(AnimationGroup group)
		{
			var n = players.Length;
			for (int i = 0; i < n; ++i)
			{
				players[i].Play(group.animations[i]);
			}
		}
	}

	[Serializable]
	public class AnimationGroup
	{
		public string name;
		public Animation[] animations;

		public AnimationGroup()
		{

		}

		public AnimationGroup(string name, params Animation[] animations)
		{
			this.name		= name;
			this.animations = animations;
		}
	}
}
