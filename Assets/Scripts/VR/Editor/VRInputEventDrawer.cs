using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Jake.VR
{
	[CustomPropertyDrawer(typeof(VRInputEvent))]
	public class VRInputEventDrawer : FieldDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Init(position, property);
			
			if (Foldout(Title(), "foldout"))
			{
				PropertyField("button");
				PropertyField("unityEvent");
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Init(property);
			
			var foldout = GetRelativeValue<bool>("foldout");
			
			if (foldout)
			{
				var height = LineHeight * 4.25f;

				var target = GetTarget<VRInputEvent>();
				if (target != null)
				{
					var numEvents = target.unityEvent.GetPersistentEventCount();
					height += LineHeight * 2.4f * Mathf.Max(1, numEvents);
				}

				return height;
			}
			else
			{
				return LineHeight;
			}
		}

		private string Title()
		{
			var buttonNames = property.FindPropertyRelative("button").enumNames;
			var button = property.FindPropertyRelative("button").enumValueIndex;
			return buttonNames[button];
		}
	}
}
