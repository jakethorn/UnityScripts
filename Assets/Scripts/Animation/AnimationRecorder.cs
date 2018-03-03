#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using CommonCode.Editor;

namespace CommonCode.Animation
{
	public class AnimationRecorder : MonoBehaviour
	{
		public Transform[]	transforms	= null;
		public Space		space		= Space.World;

		public KeyCode		splitKey	= KeyCode.Space;
		
		private Dictionary<string, List<Frame>> recordedTransforms	= new Dictionary<string, List<Frame>>();
		private bool							recording			= false;
		private bool							split				= false;

		public void StartRecording()
		{
			if (recording)
				return;
			
			recordedTransforms.Clear();

			recording = true;
			StartCoroutine(Record_Coroutine());
		}

		public void StopRecording()
		{
			if (!recording)
				return;

			recording = false;
			Save();
		}

		public void Split()
		{
			split = true;
		}

		IEnumerator Record_Coroutine()
		{
			while (recording)
			{
				yield return new WaitForFixedUpdate();

				var splitFrame = Input.GetKey(splitKey) || split;
				split = false;
				foreach (var trn in transforms)
				{
					Record(trn, splitFrame);
				}
			}
		}
		
		private void Record(Transform trn)
		{
			Record(trn, false);
		}

		private void Record(Transform trn, bool split)
		{
			if (!recordedTransforms.ContainsKey(trn.name))
			{
				recordedTransforms.Add(trn.name, new List<Frame>());
			}

			var frame = new Frame();
			switch (space)
			{
				case Space.World:
					frame.position = trn.position;
					frame.rotation = trn.rotation;
					break;
				case Space.Self:
					frame.position = trn.localPosition;
					frame.rotation = trn.localRotation;
					break;
				default:
					throw new Exception("Bad space.");
			}

			// blank frame if space bar is down to mark section of animation
			if (split)
			{
				frame.position = Vector3.zero;
				frame.rotation = Quaternion.identity;
			}

			recordedTransforms[trn.name].Add(frame);
		}

		private void Save()
		{
			foreach (var pair in recordedTransforms)
			{
				var data = ScriptableObject.CreateInstance<Animation>();
				data.frames			= pair.Value.ToArray();
				data.positionOffset = Vector3.zero;
				data.rotationOffset = Quaternion.identity;
				data.playbackSpeed	= 1;

				EditorUtil.SaveAsset(data, pair.Key + " Animation (" + space.ToString() + " space) " + RunId.instance.runTimes);
			}

			AssetDatabase.SaveAssets();
		}
	}
}
#endif