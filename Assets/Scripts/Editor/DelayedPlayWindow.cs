using UnityEditor;
using UnityEngine;
using System.Threading;
using System;

namespace Jake
{
	public class DelayedPlayWindow : EditorWindow
	{
		void OnGUI()
		{
			var delay = EditorPrefs.GetFloat("delay", 0);
			delay = EditorGUILayout.FloatField("Delay", delay);
			EditorPrefs.SetFloat("delay", delay);

			if (GUILayout.Button("Play"))
			{
				Play();
			}
		}

		[MenuItem("Jake/Delay Settings", priority = 2)]
		static void ShowWindow()
		{
			GetWindow<DelayedPlayWindow>("Delay Settings").Show();
		}

		[MenuItem("Jake/Delayed Play %#r", priority = 3)]
		static void Play()
		{
			Thread.Sleep(TimeSpan.FromSeconds(EditorPrefs.GetFloat("delay", 0)));
			EditorApplication.isPlaying = true;
		}
	}
}
