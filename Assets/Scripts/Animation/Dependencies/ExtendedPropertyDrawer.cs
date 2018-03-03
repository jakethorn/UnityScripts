using UnityEngine;

#if UNITY_EDITOR
namespace UnityEditor.Extended
{
	public class ExtendedPropertyDrawer : PropertyDrawer
	{
		protected float TEXT_HEIGHT = 16;
		protected float LINE_HEIGHT = 18;

		private Rect position;
		private SerializedProperty property;
		private float startingHeight;
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position.height = TEXT_HEIGHT;
			this.position = position;
			this.property = property;
			startingHeight = position.y;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return position.y - startingHeight;
		}

		protected SerializedProperty PropertyField(string propertyName)
		{
			var relativeProperty = property.FindPropertyRelative(propertyName);
			EditorGUI.PropertyField(position, relativeProperty);
			position.y += LINE_HEIGHT;

			return relativeProperty;
		}
	}
}
#endif
