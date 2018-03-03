using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jake.VR
{
	[Serializable]
	public class VRInputEvent
	{
		public VRButton button;
		public UnityEvent unityEvent;

		// inspector
		public bool foldout;
	}
}
