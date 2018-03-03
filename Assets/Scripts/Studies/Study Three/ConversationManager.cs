using UnityEngine;
using UnityEngine.XR;
using CommonCode.Animation;

namespace Jake.Studies.Four
{
	public class ConversationManager : MonoBehaviour
	{
		public AnimationController[] group;
		public KeyCode nextKey, prevKeyAlex, prevKeySoph, prevKeySteve;
		public bool autoStart;

		[SerializeField] private float startEmergencyConversation;

		private string subtitle;
		private Texture2D labelBackgroundTexture;
		private bool emergencyStarted;

		public bool StartEmergencyConversation
		{
			get
			{
				return Mathf.RoundToInt(startEmergencyConversation) == 1;
			}
		}

		void OnEnable()
		{
			labelBackgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			labelBackgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 1));
			labelBackgroundTexture.Apply();

			foreach (var p in group)
			{
				p.OnAnimationStarted += UpdateLine;
				p.OnAnimationFinished += PlayNextAnimation;

				if (autoStart)
					p.PlayGroup(0);
			}
		}

		private void UpdateLine(AnimationController ctrl, CommonCode.Animation.Animation anim)
		{
			anim = ctrl.groups[ctrl.GetPlayingGroupIndex() + 1].animations[2];  // head
			foreach (var line in anim.lines)
			{
				Runner.RunOnce(() => { subtitle = GetSubtitle(ctrl, line); }, line.time);
			}
		}

		private string GetSubtitle(AnimationController ctrl, Line line)
		{
			var person = ctrl.transform.name;
			var end = person.IndexOf(' ');
			if (end > 0)
			{
				person = person.Remove(end);
			}
#if USING_LIPSYNC
			return person + ": " + (line.type == LineType.AudioOnly ? line.clip.name : line.lipSyncedClip.name);
#else
			return person + ": " + (line.type == LineType.AudioOnly ? line.clip.name : null);
#endif
		}

		void Update()
		{
			if (Input.GetKeyDown(nextKey))
			{
				foreach (var p in group)
				{
					PlayNextAnimation(p, true);
				}
			}

			if (Input.GetKeyDown(prevKeyAlex))
			{
				PlayPreviousLine(group[0]);
			}
			if (Input.GetKeyDown(prevKeySoph))
			{
				PlayPreviousLine(group[1]);
			}
			if (Input.GetKeyDown(prevKeySteve))
			{
				PlayPreviousLine(group[2]);
			}

			if (StartEmergencyConversation && emergencyStarted == false)
			{
				emergencyStarted = true;
				StartEmergency();
			}
		}

		void OnGUI()
		{
			GUI.backgroundColor = new Color(0, 0, 0, 1);
			GUI.contentColor = new Color(1, 1, 1, 1);

			if (GUILayout.Button("Next line (Space bar)", GUILayout.Height(32)))
			{
				foreach (var p in group)
				{
					PlayNextAnimation(p, true);
				}
			}

			GUILayout.Space(16);
			if (GUILayout.Button("Alex repeat line (Alpha 1)", GUILayout.Height(32)))
			{
				PlayPreviousLine(group[0]);
			}

			GUILayout.Space(16);
			if (GUILayout.Button("Sophie repeat line (Alpha 2)", GUILayout.Height(32)))
			{
				PlayPreviousLine(group[1]);
			}

			GUILayout.Space(16);
			if (GUILayout.Button("Steve repeat line (Alpha 3)", GUILayout.Height(32)))
			{
				PlayPreviousLine(group[2]);
			}

			GUILayout.Space(16);
			if (GUILayout.Button("Recenter Oculus (Return key)", GUILayout.Height(32)))
			{
				InputTracking.Recenter();
			}

			var style = new GUIStyle(GUI.skin.label);
			style.padding = new RectOffset(16, 8, 8, 8);
			style.fontStyle = FontStyle.Bold;
			style.normal.background = labelBackgroundTexture;
			GUILayout.Space(32);
			GUILayout.Label(subtitle, style);
		}

		void PlayNextAnimation(AnimationController passenger, CommonCode.Animation.Animation anim)
		{
			PlayNextAnimation(passenger, false);
		}

		void PlayNextAnimation(AnimationController passenger, bool skipLoopedAnimations)
		{
			try
			{
				var groupIndex = passenger.GetPlayingGroupIndex();
				var nextAnimation = passenger.groups[groupIndex + 1].animations[0];

				if (skipLoopedAnimations && nextAnimation.loop)
				{
					passenger.PlayGroup(groupIndex + 2);
				}
				else
				{
					passenger.PlayGroup(groupIndex + 1);
				}
			}
			catch
			{

			}
		}

		void PlayPreviousLine(AnimationController passenger)
		{
			// dont play previous animation, just play previous audio file
			var groupIndex = passenger.GetPlayingGroupIndex();
			var prevAnimation = passenger.groups[groupIndex - 1].animations[2]; // 2 == HEAD
			var prevLine = prevAnimation.lines[prevAnimation.lines.Length - 1];
			if (prevLine.type == LineType.LipSynced)
			{
#if USING_LIPSYNC
				var audioSrc = passenger.players[2].lipSyncedAudioSource;   // 2 == HEAD
				audioSrc.Play(prevLine.lipSyncedClip);
#endif
			}
			else
			{
				var audioSrc = passenger.players[2].GetComponent<AudioSource>();    // 2 == HEAD
				audioSrc.PlayOneShot(prevLine.clip);
			}
		}

		public void StartConversation()
		{
			foreach (var p in group)
			{
				p.PlayGroup(0);
			}
		}

		public void StartEmergency()
		{
			foreach (var p in group)
			{
				p.PlayGroup(p.groups.Length - 2);
			}
		}
	}
}
