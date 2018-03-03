using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Extended;
#endif
#if USING_LIPSYNC
using RogoDigital.Lipsync;
#endif
using System;

namespace CommonCode.Animation
{

	public class Animation : ScriptableObject
	{
		// loop
		public bool loop;
		public bool mirror;

		// playback speed
		public float playbackSpeed;

		// offset
		public Vector3 positionOffset;
		public Quaternion rotationOffset;

		// audio
		public Line[] lines;

		// animation
		public Frame[] frames;
	}
	
	[Serializable]
	public class Line
	{
		public LineType type;
		public AudioClip clip;
#if USING_LIPSYNC
		public LipSyncData lipSyncedClip;
#endif
		public float time;
	}
	
	public enum LineType { AudioOnly, LipSynced };

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(Line))]
	public class LineDrawer : ExtendedPropertyDrawer
	{	
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			base.OnGUI(position, property, label);
			
			var type = PropertyField("type");
			var clipType = ((LineType)type.enumValueIndex == LineType.AudioOnly) ? "clip" : "lipSyncedClip";
			PropertyField(clipType);
			PropertyField("time");
		}
	}
#endif
}
