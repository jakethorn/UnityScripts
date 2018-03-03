using System;
using UnityEngine;

namespace Jake
{
	public static class Lerper
	{
		public static Coroutine LerpOverTime(Action<float> action, float from, float to, float time)
		{
			return LerpOverTime(action, Mathf.Lerp, from, to, time);
		}

		public static Coroutine LerpOverTime(Action<Vector3> action, Vector3 from, Vector3 to, float time)
		{
			from = new Vector3(from.x, from.y, from.z);

			return LerpOverTime(action, Vector3.Lerp, from, to, time);
		}

		public static Coroutine LerpOverTime<T>(Action<T> action, Func<T, T, float, T> lerper, T from, T to, float time)
		{
			var startTime = Time.time;

			return Runner.RunConditional(() =>
			{
				var ratio = (Time.time - startTime) / time;

				action(lerper(from, to, ratio));

				return ratio < 1;
			},
			CallType.Update);
		}
	}
}
