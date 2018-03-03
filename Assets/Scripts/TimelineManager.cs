using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Jake.Timeline
{
	public class TimelineManager : MonoBehaviour
	{
		/*
		 *	Fields
		 */

		[SerializeField] private PlayableDirector director;
		[SerializeField] private PlayableAsset[] timelines;

		[Space]

		[SerializeField] private float newPauseId;
		[SerializeField] private float nextTimeline0;
		[SerializeField] private float nextTimeline1;
		[SerializeField] private float nextTimeline2;
		[SerializeField] private float nextTimeline3;
		[SerializeField] private float nextTimeline4;

		[SerializeField] private float nextTimelineId = -1;

		private TimelineManager[] timelineManagers;
		private float currentPauseId;

		/*
		 *	Properties
		 */
		 
		public int NewPauseId
		{
			get
			{
				return Mathf.RoundToInt(newPauseId);
			}

			private set
			{
				newPauseId = value;
			}
		}

		public int CurrentPauseId
		{
			get
			{
				return Mathf.RoundToInt(currentPauseId);
			}

			private set
			{
				currentPauseId = value;
			}
		}

		public PlayableAsset[] NextTimelines
		{
			get
			{
				return new PlayableAsset[]
				{
					timelines.Length > Mathf.RoundToInt(nextTimeline0) ? timelines[Mathf.RoundToInt(nextTimeline0)] : null,
					timelines.Length > Mathf.RoundToInt(nextTimeline1) ? timelines[Mathf.RoundToInt(nextTimeline1)] : null,
					timelines.Length > Mathf.RoundToInt(nextTimeline2) ? timelines[Mathf.RoundToInt(nextTimeline2)] : null,
					timelines.Length > Mathf.RoundToInt(nextTimeline3) ? timelines[Mathf.RoundToInt(nextTimeline3)] : null,
					timelines.Length > Mathf.RoundToInt(nextTimeline4) ? timelines[Mathf.RoundToInt(nextTimeline4)] : null
				};
			}
		}

		public int NextTimelineId
		{
			get
			{
				return Mathf.RoundToInt(nextTimelineId);
			}

			set
			{
				if (value > 4 || value < -1)
				{
					throw new Exception("Invalid TimelineManager.NextTimelineId. Value provided: " + value.ToString());
				}

				nextTimelineId = value;
			}
		}

		/*
		 *	Methods
		 */
		 
		void Awake()
		{
			NextTimelineId = -1;
		}

		void Start()
		{
			timelineManagers = FindObjectsOfType<TimelineManager>();
		}

		void Update()
		{
			// pause
			if (NewPauseId != 0 && CurrentPauseId == 0)
			{
				director.Pause();
				StudyLogger.LogLine("timeline paused at: " + Time.time.ToString());
			}

			CurrentPauseId = NewPauseId;
			
			// next timeline
			var nextTimeline = NextTimelineId > -1 ? NextTimelines[NextTimelineId] : default(PlayableAsset);
			if (nextTimeline)
			{
				NextTimelineId = -1;
				director.Play(nextTimeline);
				StudyLogger.LogLine("timeline played at: " + Time.time.ToString());
			}
		}

		public void Unpause(int pauseId)
		{
			if (CurrentPauseId == pauseId)
			{
				director.Resume();
				StudyLogger.LogLine("timeline resumed at: " + Time.time.ToString());
			}
		}

		public void Unpause(int pauseId, bool broadcast)
		{
			foreach (var t in timelineManagers)
			{
				if (t == this || broadcast)
				{
					Unpause(pauseId);
				}
			}
		}

		public void SetTimeline(int index, PlayableAsset timeline)
		{
			timelines[index] = timeline;
		}
	}
}
