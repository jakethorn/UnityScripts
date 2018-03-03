using UnityEngine;

namespace Jake.GameObjectExtensions
{
	public static class GameObjectExtensions
	{
		public static GameObject GetParent(this GameObject that)
		{
			return that.transform.parent.gameObject;
		}

		public static void SetParent(this GameObject that, GameObject parent)
		{
			that.transform.parent = parent.transform;
		}

		public static GameObject Child(this GameObject that, params string[] children)
		{
			var go = that;
			foreach (var c in children)
			{
				go = go.transform.Find(c).gameObject;
			}

			return go;
		}

		public static void RemoveComponent<T>(this GameObject that) where T : Component
		{
			if (Application.isPlaying)
			{
				Object.Destroy(that.GetComponent<T>());
			}
			else
			{
				Object.DestroyImmediate(that.GetComponent<T>());
			}
		}

		public static Vector3 GetPosition(this GameObject that)
		{
			return that.transform.position;
		}

		public static void SetPosition(this GameObject that, Vector3 position)
		{
			that.transform.position = position;
		}

		public static Quaternion GetRotation(this GameObject that)
		{
			return that.transform.rotation;
		}

		public static void SetRotation(this GameObject that, Quaternion rotation)
		{
			that.transform.rotation = rotation;
		}
	}
}
