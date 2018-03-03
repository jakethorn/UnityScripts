using UnityEngine;
using System;

namespace CommonCode.Animation
{
	[Serializable]
	public class Frame
	{
		public Vector3		position;
		public Quaternion	rotation;
		
		public Frame() : this(Vector3.zero, Quaternion.identity)
		{

		}

		public Frame(Vector3 p, Quaternion r)
		{
			position	= p;
			rotation	= r;
		}
	}
}
