using UnityEngine;

namespace Jake
{
	using ArrayExtensions;

	/// <summary>
	/// Helpful math related methods.
	/// </summary>
	public static class Math
	{
		/// <summary>
		/// Reduce angle in degrees to be between 0 and 360.
		/// </summary>
		public static float ReduceAngle(float a)
		{
			while (a < 0)
			{
				a += 360;
			}

			while (a > 360)
			{
				a -= 360;
			}

			return a;
		}
		
		public static Vector3 Min(params Vector3[] vs)
		{
			return new Vector3(
				Mathf.Min(vs.Convert((v) => v.x)), 
				Mathf.Min(vs.Convert((v) => v.y)), 
				Mathf.Min(vs.Convert((v) => v.z))
			);
		}

		public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
		{
			return new Vector3(
				Mathf.Clamp(v.x, min.x, max.x),
				Mathf.Clamp(v.y, min.y, max.y),
				Mathf.Clamp(v.z, min.z, max.z)
			);
		}
	}
}
