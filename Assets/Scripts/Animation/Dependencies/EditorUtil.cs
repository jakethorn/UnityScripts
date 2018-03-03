using UnityEngine;
using CommonCode.RuntimeModelEditing;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Jake;

namespace CommonCode.Editor
{
	public static class EditorUtil
	{
		public static bool SaveAsset(Object asset, string name = null)
		{
#if UNITY_EDITOR
			string path = EditorUtility.SaveFilePanel("Save Asset", "Assets/", name != null ? name : asset.name, "asset");
			if (string.IsNullOrEmpty(path)) return false;
			path = FileUtil.GetProjectRelativePath(path);
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();

			return true;
#else
		return false;
#endif
		}

		public static bool LoadMeshes()
		{
#if UNITY_EDITOR
			var filters = Object.FindObjectsOfType<MeshFilter>();
			foreach (var filter in filters)
			{
				var name = filter.mesh.name;
				var mesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/Meshes/" + name + ".asset");
				if (mesh != null)
					Runner.RunCoroutine(filter.Lerp(mesh, 1));
			}

			return true;
#else
		return false;
#endif
		}
	}
}
