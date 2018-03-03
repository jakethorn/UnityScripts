using UnityEngine;
using UnityEngine.UI;

namespace Jake.Studies.Four
{
	using Timeline;
	using VR;

	public class StudyFour : MonoBehaviour
	{
		public const int OPEN_ELEVATOR_ONE = 1;
		public const int OPEN_ELEVATOR_TWO = 2;
		public const int OPEN_EITHER_ELEVATOR = 3;
		public const int START = 99;
		public const int CONTINUE = 98;
		
		public TimelineManager elevatorTimeline;
		public Raycaster leftHand;
		public Raycaster rightHand;

		public float end;

		public Image fade;
		public AudioSource fire;

		[Space]

		public bool useRightHand;
		
		public bool ItIsTheEnd
		{
			get
			{
				return Mathf.RoundToInt(end) == 1;
			}
		}

		private bool madeChoice;

		void Start()
		{
			if (useRightHand)
			{
				StudyLogger.LogLine("Using right controller.");
			}
			else
			{
				StudyLogger.LogLine("Using left controller.");
			}
		}

		void Update()
		{
			if (useRightHand)
			{
				if (VRInput.GetButtonDown(VRButton.Rift_A) || VRInput.GetButtonDown(VRButton.Vive_RightTrackpad))
				{
					TryOpenElevator(rightHand);
				}
			}
			else
			{
				if (VRInput.GetButtonDown(VRButton.Rift_X) || VRInput.GetButtonDown(VRButton.Vive_LeftTrackpad))
				{
					TryOpenElevator(leftHand);
				}
			}

		}
		
		public void TryOpenElevator(Raycaster hand)
		{
			var hit = hand.Hit;
			if (hit)
			{
				TryOpenElevator(hit.name);
			}
		}

		public void TryOpenElevator(string collider)
		{
			if (ItIsTheEnd && madeChoice == false)
			{
				madeChoice = true;

				if (collider == "Collider One")
				{
					StudyLogger.LogLine("Saved left at: " + Time.time.ToString());
				}
				else if (collider == "Collider Two")
				{
					StudyLogger.LogLine("Saved right at: " + Time.time.ToString());
				}

				FadeOut.StartFade();

				Lerper.LerpOverTime((v) =>
				{
					AudioListener.volume = v;
				}, 
				1, 0, 1);
			}

			if (collider == "Collider One")
			{
				if (elevatorTimeline.CurrentPauseId == OPEN_EITHER_ELEVATOR)
				{
					elevatorTimeline.NextTimelineId = 0;
					elevatorTimeline.Unpause(OPEN_EITHER_ELEVATOR);
				}
				else
				{
					elevatorTimeline.Unpause(OPEN_ELEVATOR_ONE);
				}
				
				StudyLogger.LogLine("Tried to open left elevator at: " + Time.time.ToString());
			}
			else if (collider == "Collider Two")
			{
				if (elevatorTimeline.CurrentPauseId == OPEN_EITHER_ELEVATOR)
				{
					elevatorTimeline.NextTimelineId = 1;
					elevatorTimeline.Unpause(OPEN_EITHER_ELEVATOR);
				}
				else
				{
					elevatorTimeline.Unpause(OPEN_ELEVATOR_TWO);
				}
				
				StudyLogger.LogLine("Tried to open right elevator at: " + Time.time.ToString());
			}
		}

		public void StartMainScene()
		{
			elevatorTimeline.Unpause(START);
		}

		public void ContinueMainScene()
		{
			elevatorTimeline.Unpause(CONTINUE);
		}
	}
}
