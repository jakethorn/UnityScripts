using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jake
{
	[Serializable]
	public class Event
	{
		public KeyCode keyCode;
		public UnityEvent unityEvent;
	}
	
	public class KeyInput : MonoBehaviour
	{
		public Event[] events;

		void Update()
		{
			foreach (var e in events)
			{
				if (Input.GetKeyDown(e.keyCode))
				{
					e.unityEvent.Invoke();
				}
			}
		}
	}
}
