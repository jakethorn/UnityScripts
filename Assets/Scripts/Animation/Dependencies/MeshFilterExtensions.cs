using UnityEngine;
using System.Collections;

namespace CommonCode.RuntimeModelEditing
{
	public static class MeshFilterExtensions
	{
		public static IEnumerator Lerp(this MeshFilter that, Mesh newMesh, float time)
		{
			var initialMesh = Object.Instantiate(that.mesh);
			var timeTaken = 0f;
			while (timeTaken < time)
			{
				that.mesh.vertices = LerpVertices(initialMesh.vertices, newMesh.vertices, timeTaken / time);
				that.mesh.RecalculateNormals();
				timeTaken += Time.deltaTime;

				yield return null;
			}

			Object.Destroy(initialMesh);
		}

		public static Vector3 GetTransformedVertex(this MeshFilter that, int index)
		{
			return that.transform.TransformPoint(that.mesh.vertices[index]);
		}

		public static Vector3 GetInverseTransformedVertex(this MeshFilter that, int index)
		{
			return that.transform.InverseTransformPoint(that.mesh.vertices[index]);
		}

		private static Vector3[] LerpVertices(Vector3[] v0, Vector3[] v1, float ratio)
		{
			for (int i = 0; i < v0.Length; ++i)
			{
				var diff = v1[i] - v0[i];
				v0[i] += diff * ratio;
			}

			return v0;
		}
	}
}
