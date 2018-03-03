using UnityEngine;
using System;
using System.Collections;

namespace Jake
{
	public enum CallType { Update, FixedUpdate, LateUpdate };

	public static class Runner
	{
		public static Coroutine RunCoroutine(IEnumerator coroutine)
		{
			return UnityEngine.Object.FindObjectOfType<MonoBehaviour>().StartCoroutine(coroutine);
		}

		public static Coroutine Run(Action action, CallType callType = CallType.Update)
		{
			return RunCoroutine(Repeat_Coroutine(() => { action(); return true; }, callType));
		}
		
		public static Coroutine RunOnce(Action action, float delay = 0)
		{
			return RunCoroutine(Once_Coroutine(action, delay));
		}

		public static Coroutine RunConditional(Func<bool> func, CallType callType = CallType.Update)
		{
			return RunCoroutine(Repeat_Coroutine(func, callType));
		}
		
		private static IEnumerator Once_Coroutine(Action action, float delay = 0)
		{
			if (delay > 0)
			{
				yield return new WaitForSeconds(delay);
			}

			action();
		}

		private static IEnumerator Repeat_Coroutine(Func<bool> func, CallType callType)
		{
			switch (callType)
			{
				case CallType.Update:
					do
					{
						yield return null;
					} while (func());
					break;
				case CallType.FixedUpdate:
					do
					{
						yield return new WaitForFixedUpdate();
					} while (func());
					break;
				case CallType.LateUpdate:
					do
					{
						yield return new WaitForEndOfFrame();
					} while (func());
					break;
			}
		}
	}
}
