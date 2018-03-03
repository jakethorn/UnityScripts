using UnityEditor;
using UnityEngine;

namespace Jake.Tcp
{
	[CustomEditor(typeof(TcpConnection))]
	public class TcpConnectionEditor : BehaviourEditor<TcpConnection>
	{
		SerializedProperty autoConnect;
		SerializedProperty socketType;
		SerializedProperty ip;
		SerializedProperty port;
		SerializedProperty useJson;
		SerializedProperty jsonFile;

		bool jsonLoaded;

		void OnEnable()
		{
			autoConnect	= serializedObject.FindProperty("autoConnect");
			socketType	= serializedObject.FindProperty("socketType");
			ip			= serializedObject.FindProperty("ip");
			port		= serializedObject.FindProperty("port");
			useJson		= serializedObject.FindProperty("useJson");
			jsonFile	= serializedObject.FindProperty("jsonFile");
		}

		public override void OnInspectorGUI()
		{
			BeginInspector();

			EditorGUILayout.PropertyField(autoConnect);
			EditorGUILayout.PropertyField(socketType);

			if (target.useJson)
			{
				if (!jsonLoaded)
				{
					try
					{
						target.LoadJson();
						jsonLoaded = true;
					}
					catch
					{

					}
				}

				GUI.enabled = false;
			}
			else
			{
				jsonLoaded = false;
			}

			EditorGUILayout.PropertyField(ip);
			EditorGUILayout.PropertyField(port);

			if (target.useJson)
			{
				GUI.enabled = true;
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(useJson);

			if (target.useJson)
			{
				GUILayout.FlexibleSpace();
				EditorGUILayout.PropertyField(jsonFile, GUIContent.none);
			}

			EditorGUILayout.EndHorizontal();

			EndInspector();
		}
	}
}
