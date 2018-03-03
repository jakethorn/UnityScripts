using UnityEngine;
using UnityEngine.Playables;

namespace Jake.Studies.Four
{
	using Timeline;

	public class ConditionData : MonoBehaviour
	{
		public PlayableAsset	noFireNoGroup,
								noFireGroup,
								fireNoGroup,
								fireGroup,
								noDilemmaNoGroup;

		public CommonCode.Animation.Animation	alexSaveGirl, 
												sophieSaveGirl, 
												steveSaveGirl, 
												alexSaveGroup, 
												sophieSaveGroup, 
												steveSaveGroup;

		public TimelineManager timelineManager;

		public CommonCode.Animation.AnimationController alex, 
														sophie, 
														steve;

		public Transform leftElevatorTargets, rightElevatorTargets;

		public NavMeshCharacterAnimator girl, adult1, adult2, adult3, adult4, adult5;

		public StudyFour study;

		[Space]

		public ConditionFile conditionFile;

		void Awake()
		{
			// change id 
			FindObjectOfType<StudyLogger>().id = conditionFile.participantId;

			var participantId = conditionFile.participantId.ToString();
			var dilemmaType = default(DilemmaType);// conditionFile.dilemmaType;
			var influenceType = default(InfluenceType);// conditionFile.influenceType;
			var swapElevators = default(bool);// conditionFile.swapElevators;
			var swapHands = default(bool);// conditionFile.swapHands;

			// do from participantId
			if (participantId[0] == '1' || participantId[0] == '2' || participantId[0] == '3')
			{
				dilemmaType = DilemmaType.Moral;
			}
			else if (participantId[0] == '4' || participantId[0] == '5' || participantId[0] == '6')
			{
				dilemmaType = DilemmaType.Standard;
			}

			if (participantId[0] == '1' || participantId[0] == '4')
			{
				influenceType = InfluenceType.Girl;
			}
			else if (participantId[0] == '2' || participantId[0] == '5')
			{
				influenceType = InfluenceType.Adults;
			}
			else if (participantId[0] == '3' || participantId[0] == '6')
			{
				influenceType = InfluenceType.None;
			}

			if (participantId[1] == '1')
			{
				swapElevators = false;
			}
			else if (participantId[1] == '2')
			{
				swapElevators = true;
			}

			if (participantId[2] == '1')
			{
				swapHands = false;
			}
			else if (participantId[2] == '2')
			{
				swapHands = true;
			}

			// do
			if (dilemmaType == DilemmaType.Moral)
			{
				if (influenceType == InfluenceType.None)
				{
					timelineManager.SetTimeline(3, fireNoGroup);
				}
				else
				{
					timelineManager.SetTimeline(3, fireGroup);
				}
			}
			else if (dilemmaType == DilemmaType.Standard)
			{
				if (influenceType == InfluenceType.None)
				{
					timelineManager.SetTimeline(3, noFireNoGroup);
				}
				else
				{
					timelineManager.SetTimeline(3, noFireGroup);
				}
			}
			else
			{
				timelineManager.SetTimeline(3, noDilemmaNoGroup);
			}

			switch (influenceType)
			{
				case InfluenceType.Adults:
					alex.groups[14].animations[2] = alexSaveGroup;
					sophie.groups[14].animations[2] = sophieSaveGroup;
					steve.groups[14].animations[2] = steveSaveGroup;
					break;
				case InfluenceType.Girl:
					alex.groups[14].animations[2] = alexSaveGirl;
					sophie.groups[14].animations[2] = sophieSaveGirl;
					steve.groups[14].animations[2] = steveSaveGirl;
					break;
			}

			if (swapElevators)
			{
				adult1.target = leftElevatorTargets.Find("Top Left");
				adult2.target = leftElevatorTargets.Find("Top Right");
				adult3.target = leftElevatorTargets.Find("Middle");
				adult4.target = leftElevatorTargets.Find("Bottom Left");
				adult5.target = leftElevatorTargets.Find("Bottom Right");

				girl.target = rightElevatorTargets.Find("Middle");
			}
			else
			{
				adult1.target = rightElevatorTargets.Find("Top Left");
				adult2.target = rightElevatorTargets.Find("Top Right");
				adult3.target = rightElevatorTargets.Find("Middle");
				adult4.target = rightElevatorTargets.Find("Bottom Left");
				adult5.target = rightElevatorTargets.Find("Bottom Right");

				girl.target = leftElevatorTargets.Find("Middle");
			}

			study.useRightHand = !swapHands;
		}
	}
}
