using System;
using UnityEngine;

namespace CommonCode
{
	[Serializable]
	public class TransformLite
	{
		public Vector3		position;
		public Quaternion	rotation;
		
		// 1
		public TransformLite(Vector3 position, Quaternion rotation)
		{
			this.position = position;
			this.rotation = rotation;
		}
		
		// 2
		public TransformLite() : this(Vector3.zero, Quaternion.identity)
		{

		}
		
		// 3
		public TransformLite(Vector3 position) : this(position, Quaternion.identity)
		{

		}
		
		// 4
		public TransformLite(Quaternion rotation) : this(Vector3.zero, rotation)
		{

		}
		
		// 5
		public TransformLite(TransformLite other) : this(other.position, other.rotation)
		{

		}
		
		// 6
		public TransformLite(float x, float y, float z, float qx, float qy, float qz, float qw) : this(new Vector3(x, y, z), new Quaternion(qx, qy, qz, qw))
		{

		}
		
		public void Copy(TransformLite other)
		{
			position.x = other.position.x;
			position.y = other.position.y;
			position.z = other.position.z;
			
			rotation.x = other.rotation.x;
			rotation.y = other.rotation.y;
			rotation.z = other.rotation.z;
			rotation.w = other.rotation.w;
		}

		public void Set(float x, float y, float z, float qx, float qy, float qz, float qw)
		{
			position = new Vector3(x, y, z);
			rotation = new Quaternion(qx, qy, qz, qw);
		}

		public bool Approx(TransformLite other, float distance, float angle)
		{
			return	Vector3.Distance(position, other.position) < distance && 
					Quaternion.Angle(rotation, other.rotation) < angle;
			
		}

		public static TransformLite Lerp(TransformLite a, TransformLite b, float ratio)
		{
			return new TransformLite(Vector3.Lerp(a.position, b.position, ratio), Quaternion.Lerp(a.rotation, b.rotation, ratio));
		}
	}
}
