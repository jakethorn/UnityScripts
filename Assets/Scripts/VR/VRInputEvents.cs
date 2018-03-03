using UnityEngine;
using UnityEngine.Events;

namespace Jake.VR
{
	public class VRInputEvents : MonoBehaviour
	{
		public VRInputEvent[] events;

		void Update()
		{ 
			foreach (var e in events)
			{
				if (VRInput.GetButtonDown(e.button))
				{
					e.unityEvent.Invoke();
				}
			}
		}
	}
}
