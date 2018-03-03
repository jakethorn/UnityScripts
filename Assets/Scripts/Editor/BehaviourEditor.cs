using UnityEditor;
using UnityEngine;

namespace Jake
{
	public class BehaviourEditor<T> : Editor where T : MonoBehaviour
	{
		protected new T target;

		public void BeginInspector(bool drawHeader = true)
		{
			serializedObject.Update();

			target = base.target as T;

			if (drawHeader)
			{
				GUI.enabled = false;
				EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(target), typeof(T), false);
				GUI.enabled = true;
			}
		}

		public void EndInspector()
		{
			serializedObject.ApplyModifiedProperties();
		}

		protected void Field(string member)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty(member));
		}

		protected void Field(string member, bool includeChildren)
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty(member), includeChildren);
		}

		protected void Space()
		{
			EditorGUILayout.Space();
		}

		protected void Label(string label)
		{
			EditorGUILayout.LabelField(label);
		}

		protected bool Foldout(bool fold, string title)
		{
			return EditorGUILayout.Foldout(fold, title);
		}
	}
}
