using UnityEngine;
using System;
using System.Collections;
#if USING_LIPSYNC
using RogoDigital.Lipsync;
#endif
namespace CommonCode.Animation
{
	public class AnimationPlayer : MonoBehaviour
	{
		//public new	Animation	animation		= null;
		public		Transform	relativeTo		= null;
		public		Vector3		positionOffset	= Vector3.zero;
		public		Quaternion	rotationOffset	= Quaternion.identity;
		public		Space		offsetSpace;
		//public		bool		animatePosition = true;
		//public		bool		animateRotation = true;
		//public		bool		playOnStart		= false;
		//public		bool		loop			= false;
		public		float		transitionTime	= 0;

		//public		bool		debug = false;
#if USING_LIPSYNC
		public LipSync lipSyncedAudioSource;
#endif
		public bool IsPlaying
		{
			get
			{
				return isPlaying;
			}
		}
		
		public Animation PlayingAnimation
		{
			get
			{
				return playingAnimation;
			}
		}

		public event Action<AnimationPlayer, Animation> OnAnimationStarted;
		public event Action<AnimationPlayer, Animation> OnAnimationFinished;

		private bool					isPlaying;
		private Coroutine				playCoroutine;
		private Animation				playingAnimation;
		private Processor<Transform>	postProcessor;

		private Vector3 PositionOffset
		{
			get
			{
				switch (offsetSpace)
				{
					case Space.World:
						return positionOffset;
					case Space.Self:
						return transform.rotation * positionOffset;
					default:
						throw new Exception("Wrong space");
				}
			}
		}

		private Quaternion RotationOffset
		{
			get
			{
				return rotationOffset;

				//switch (offsetSpace)
				//{
				//	case Space.World:
				//		return rotationOffset;
				//	case Space.Self:
				//		return transform.rotation * rotationOffset;
				//	default:
				//		throw new Exception("Wrong space");
				//}
			}
		}

		//public void Play()
		//{
		//	Play(animation);
		//}
		
		public void Play(Animation animation)
		{
			Play(animation, false);
		}
		
		public void Play(Animation animation, bool mirror)
		{
			if (isPlaying)
			{
				StopCoroutine(playCoroutine);
			}

			if (animation != null)
			{
				playCoroutine = StartCoroutine(Play_Coroutine(animation, mirror));
			}
		}
		
		void Start()
		{
			postProcessor = GetComponent<Processor<Transform>>();

			//if (playOnStart)
			//	Play();
		}

		private IEnumerator Play_Coroutine(Animation animation, bool mirror)
		{
			// dont invoke if looping
			if (animation != PlayingAnimation && OnAnimationStarted != null)
			{
				//playingAnimation = animation;
				OnAnimationStarted(this, animation);
			}
			else
			{
				//playingAnimation = animation;
			}
			
			playingAnimation = animation;
			isPlaying = true;
			
			// transition into animation
			if (transitionTime > 0)
			{
				for (var t = transitionTime; t > 0; t -= Time.fixedDeltaTime)
				{
					var cachedPosition = transform.position;// - positionOffset;
					var cachedRotation = transform.rotation;// * Quaternion.Inverse(rotationOffset);

					if (mirror)
						ApplyFrame(animation.frames[animation.frames.Length - 1], animation.positionOffset, animation.rotationOffset);
					else
						ApplyFrame(animation.frames[0], animation.positionOffset, animation.rotationOffset);

					transform.position = Vector3	.Lerp(cachedPosition, transform.position, Time.fixedDeltaTime / transitionTime);
					transform.rotation = Quaternion	.Lerp(cachedRotation, transform.rotation, Time.fixedDeltaTime / transitionTime);

					yield return new WaitForFixedUpdate();
				}
			}

			// play audio if any
			foreach (var l in animation.lines)
			{
				StartCoroutine(Audio_Coroutine(l));
			}
			
			// animation
			for (var f = 0f; f < animation.frames.Length; f += animation.playbackSpeed)
			{
				ApplyFrame2(animation, mirror ? animation.frames.Length - f : f);
				yield return new WaitForFixedUpdate();
			}
			
			// end
			isPlaying = false;


			// loop?
			if (animation.loop)
			{
				if (animation.mirror)
					Play(animation, !mirror);
				else
					Play(animation);
			}
			else
			{
				if (OnAnimationFinished != null)
					OnAnimationFinished(this, animation);
			}
		}
		
		private IEnumerator Audio_Coroutine(Line line)
		{
			yield return new WaitForSeconds(line.time);

			var src = GetComponent<AudioSource>();
			if (src != null && line.type == LineType.AudioOnly && line.clip != null)
			{
				src.PlayOneShot(line.clip);
			}
#if USING_LIPSYNC
			else if (lipSyncedAudioSource != null && line.lipSyncedClip != null)
			{
				lipSyncedAudioSource.Play(line.lipSyncedClip);
			}
#endif
			else
			{
				Debug.LogWarning("Something didn't happen");
			}
		}

		private void ApplyFrame(Frame frame, Vector3 animationPositionOffset, Quaternion animationRotationOffset)
		{
			/*if (animatePosition) */transform.position = (frame.position - animationPositionOffset)						- PositionOffset;
			/*if (animateRotation) */transform.rotation = (frame.rotation * Quaternion.Inverse(animationRotationOffset))	* Quaternion.Inverse(RotationOffset);

			if (relativeTo != null)
			{
				transform.position += relativeTo.position;
				transform.RotateAround(relativeTo.position, relativeTo.right,	relativeTo.eulerAngles.x);
				transform.RotateAround(relativeTo.position, relativeTo.up,		relativeTo.eulerAngles.y);
				transform.RotateAround(relativeTo.position, relativeTo.forward, relativeTo.eulerAngles.z);
			}

			if (postProcessor != null)
			{
				var t = transform;
				postProcessor.Process(ref t);
			}
		}

		private void ApplyFrame2(Animation a, float t)
		{
			var i = Mathf.Clamp(Mathf.FloorToInt(t)	, 0, a.frames.Length - 1);
			var j = Mathf.Clamp(i + 1				, 0, a.frames.Length - 1);
			var r = t - i;

			//print(i.ToString() + " " + j.ToString());

			var f = new Frame(
				Vector3		.Lerp(a.frames[i].position, a.frames[j].position, r),
				Quaternion	.Lerp(a.frames[i].rotation, a.frames[j].rotation, r)
			);

			ApplyFrame(f, a.positionOffset, a.rotationOffset);
		}
	}
}
