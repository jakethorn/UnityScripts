using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jake
{
	using ArrayExtensions;

	public class FieldDrawer : PropertyDrawer
	{
		/*
		 * Static 
		 */

		protected static Dictionary<string, object> sharedData = new Dictionary<string, object>();

		protected static float LineHeight
		{
			get
			{
				return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}
		}

		/*
		 * Instance
		 */

		protected Rect position;
		protected SerializedProperty property;

		/*
		 * Private fields
		 */

		private bool beganLine;
		private bool beganRatioLine;
		private int ratioIndex;
		private float[] lineRatios;
		private float guiWidth;

		private object target;

		/*
		 *	Protected properties
		 */

		protected object Target
		{
			get
			{
				var parent = property.serializedObject.targetObject;
				var name = property.propertyPath;

				if (name.Contains("."))
				{
					// is array
					var firstDot = name.IndexOf(".");
					var realName = name.Remove(firstDot);
					var targetField = parent.GetType().GetField(realName);
					var array = targetField.GetValue(parent) as object[];

					var startChar = name.IndexOf("[") + 1;
					var endChar = name.IndexOf("]");
						
					var index = int.Parse(name.Substring(name.Length - 2, endChar - startChar));
					if (index < array.Length)
					{
						target = array[index];
					}
					else
					{
						target = null;
					}
				}
				else
				{
					// is normal
					var targetField = parent.GetType().GetField(name);
					target = targetField.GetValue(parent);
				}

				return target;
			}
		}

		/*
		 * Protected methods
		 */

		protected void Init(Rect position, SerializedProperty property)
		{
			this.position = position;
			this.position.height = EditorGUIUtility.singleLineHeight;

			this.property = property;

			beganLine = false;
			guiWidth = position.width;
		}

		protected void Init(SerializedProperty property)
		{
			this.property = property;
		}

		protected T GetTarget<T>() where T : class
		{
			return Target as T;
		}
		
		protected void BeginLine(int fields)
		{
			beganLine = true;
			guiWidth = position.width;
			position.width /= fields;
		}
		
		protected void BeginLine(int fields, params float[] ratios)
		{
			beganRatioLine = true;
			lineRatios = ratios;
			guiWidth = position.width;
			position.width = guiWidth * lineRatios[ratioIndex++];
		}
		
		protected void EndLine()
		{
			beganLine = beganRatioLine = false;
			ratioIndex = 0;
			position.width = guiWidth;
			NewLine();
		}

		protected void NewLine()
		{
			if (beganLine)
			{
				position.x += position.width;
			}
			else if (beganRatioLine)
			{
				position.x += position.width;
				position.width = guiWidth * lineRatios[ratioIndex++];
			}
			else
			{
				position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
			}
		}

		protected void Label(string relativePropertyPath)
		{
			GUI.enabled = false;
			EditorGUI.TextField(position, property.FindPropertyRelative(relativePropertyPath).stringValue);
			GUI.enabled = true;
			NewLine();
		}

		protected bool Foldout(string title, string relativePropertyPath)
		{
			var rect = position;
			rect.width = EditorGUIUtility.singleLineHeight;

			var foldedProperty = property.FindPropertyRelative(relativePropertyPath);
			foldedProperty.boolValue = EditorGUI.Foldout(rect, foldedProperty.boolValue, title);
			NewLine();
			return foldedProperty.boolValue;
		}

		protected int Popup(string[] listLabels, ref int index)
		{
			return Popup(null, listLabels, ref index);
		}

		protected int Popup(string label, string[] listLabels, ref int index)
		{
			if (label != null)
				index = EditorGUI.Popup(position, label, index, listLabels);
			else
				index = EditorGUI.Popup(position, index, listLabels);

			NewLine();
			return index;
		}

		protected void DrawRect(int lines)
		{
			var rect = position;
			rect.y -= EditorGUIUtility.standardVerticalSpacing;
			rect.height *= lines;
			rect.height += EditorGUIUtility.standardVerticalSpacing * (lines + 1);

			var style = new GUIStyle();
			style.normal.background = new Texture2D(1, 1);
			style.normal.background.SetPixels(
				new[] { new Color(.1f, .1f, .1f, .1f) });
			
			EditorGUI.LabelField(rect, GUIContent.none, style);
		}

		/*
		 * "member field" methods
		 */

		protected Component ComponentField(GameObject gameObject, string relativePropertyPath)
		{
			if (gameObject == null)
				return null;

			var componentProperty = property.FindPropertyRelative(relativePropertyPath);
			var component = componentProperty.objectReferenceValue as Component;
			var components = gameObject.GetComponents<Component>();
			var componentIndex = components.IndexOfFirstOrZero((c) => c == component);

			Popup("Component", components.Convert((c) => c.GetType().Name), ref componentIndex);

			componentProperty.objectReferenceValue = components[componentIndex];
			return components[componentIndex];
		}

		protected Component ComponentField(string gameObjectPropertyPath, string relativePropertyPath)
		{
			var gameObject = PropertyField<GameObject>(gameObjectPropertyPath);
			var component = ComponentField(gameObject, relativePropertyPath);

			return component;
		}

		protected T MemberField<T>(Type type, string gameObjectPropertyPath, string componentPropertyPath, string signatureProperyPath) where T : MemberInfo
		{
			var component	= ComponentField(gameObjectPropertyPath, componentPropertyPath);
			var memberInfo	= MemberField<T>(type, component, signatureProperyPath);

			return memberInfo;
		}
		
		protected T MemberField<T>(Type type, Component component, string signatureProperyPath) where T : MemberInfo
		{
			if (component == null)
				return default(T);

			var members = default(MemberInfo[]);
			var binderFlags = BindingFlags.Instance | BindingFlags.Public;
			if (typeof(T) == typeof(FieldInfo))
			{
				members = component.GetType().GetFields(binderFlags)
					.Where((f) => f.FieldType == type);
			}
			else if (typeof(T) == typeof(MethodInfo))
			{
				members = component.GetType().GetMethods(binderFlags).Where((m) =>
					(!m.Name.StartsWith("get_")					&& !m.Name.StartsWith("set_"))						&&
					((m.ReturnType						== type	&& m.GetParameters().Length				== 0)		||
					(m.GetParameters().Length			== 1	&& m.GetParameters()[0].ParameterType	== type)	||
					(type								== null && m.GetParameters().Length				== 0)
				));
			}
			else if (typeof(T) == typeof(PropertyInfo))
			{
				members = component.GetType().GetProperties(binderFlags)
					.Where((p) => p.PropertyType == type);
			}

			return MemberField<T>(members, signatureProperyPath);
		}

		protected T MemberField<T>(MemberInfo[] members, string signatureProperyPath) where T : MemberInfo
		{
			if (members == null || members.Length == 0)
				return default(T);
			
			var memberSignatureProperty = property.FindPropertyRelative(signatureProperyPath);
			var memberIndex				= members.IndexOfFirstOrZero((m) => m.ToString() == memberSignatureProperty.stringValue);

			Popup("Member", members.Convert((m) => m.Name), ref memberIndex);

			memberSignatureProperty.stringValue = members[memberIndex].ToString();
			return members[memberIndex] as T;
		}

		/*
		 * "generic field" methods
		 */

		protected T PropertyField<T>(string relativePropertyPath)
		{
			return (T)PropertyField(typeof(T), relativePropertyPath);
		}

		protected T PropertyField<T>(string title, string relativePropertyPath)
		{
			return (T)PropertyField(typeof(T), title, relativePropertyPath);
		}

		protected object PropertyField(Type type, string relativePropertyPath)
		{
			return PropertyField(type, null, relativePropertyPath);
		}

		protected object PropertyField(Type type, string title, string relativePropertyPath)
		{
			var relativeProperty = property.FindPropertyRelative(relativePropertyPath);
			if (title != null)
			{
				EditorGUI.PropertyField(position, relativeProperty, title != "" ? new GUIContent(title) : GUIContent.none);
			}
			else
			{
				EditorGUI.PropertyField(position, relativeProperty);
			}

			NewLine();

			return type == null ? relativeProperty : GetRelativeValue(type, relativePropertyPath);
		}

		protected SerializedProperty PropertyField(string relativeProperyPath)
		{
			return PropertyField(title: null, relativePropertyPath: relativeProperyPath);
		}

		protected SerializedProperty PropertyField(string title, string relativePropertyPath)
		{
			return PropertyField(null, title, relativePropertyPath) as SerializedProperty;
		}

		protected T GetRelativeValue<T>(string relativePropertyPath)
		{
			return (T)GetRelativeValue(typeof(T), relativePropertyPath);
		}

		protected object GetRelativeValue(Type type, string relativePropertyPath)
		{
			var relativeProperty = property.FindPropertyRelative(relativePropertyPath);

			if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				return relativeProperty.objectReferenceValue;
			}
			else if (type.IsEnum)
			{
				return relativeProperty.enumValueIndex;
			}
			else if (type == typeof(bool))
			{
				return relativeProperty.boolValue;
			}
			else if (type == typeof(float))
			{
				return relativeProperty.floatValue;
			}
			else if (type == typeof(int))
			{
				return relativeProperty.intValue;
			}
			else if (type == typeof(Quaternion))
			{
				return relativeProperty.quaternionValue;
			}
			else if (type == typeof(string))
			{
				return relativeProperty.stringValue;
			}
			else if (type == typeof(Vector3))
			{
				return relativeProperty.vector3Value;
			}

			throw new Exception("Type not supported.");
		}

		protected void SetRelativeValue<T>(string relativePropertyPath, T value)
		{
			SetRelativeValue(typeof(T), relativePropertyPath, value);
		}

		protected void SetRelativeValue(Type type, string relativePropertyPath, object value)
		{
			var relativeProperty = property.FindPropertyRelative(relativePropertyPath);

			if (type == typeof(UnityEngine.Object) || type.IsSubclassOf(typeof(UnityEngine.Object)))
			{
				relativeProperty.objectReferenceValue = value as UnityEngine.Object;
			}
			else if (type.IsEnum)
			{
				relativeProperty.enumValueIndex = (int)value;
			}
			else if (type == typeof(bool))
			{
				relativeProperty.boolValue = (bool)value;
			}
			else if (type == typeof(float))
			{
				relativeProperty.floatValue = (float)value;
			}
			else if (type == typeof(int))
			{
				relativeProperty.intValue = (int)value;
			}
			else if (type == typeof(Quaternion))
			{
				relativeProperty.quaternionValue = (Quaternion)value;
			}
			else if (type == typeof(string))
			{
				relativeProperty.stringValue = (string)value;
			}
			else if (type == typeof(Vector3))
			{
				relativeProperty.vector3Value = (Vector3)value;
			}
			else
			{
				throw new Exception("Type not supported.");
			}
		}
	}
}
