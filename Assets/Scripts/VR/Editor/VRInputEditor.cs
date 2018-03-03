using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace Jake.VR
{
	[CustomEditor(typeof(VRInput))]
	public class VRInputEditor : BehaviourEditor<VRInput>
	{
		public override void OnInspectorGUI()
		{
			BeginInspector();

			Field("leftHand");
			Field("rightHand");

			Space();
			Field("applyOffset");

			if (target.applyOffset)
			{
				Field("editLeftOffset");
				Field("leftPositionOffset");
				Field("leftRotationOffset");

				Space();

				Field("editRightOffset");
				Field("rightPositionOffset");
				Field("rightRotationOffset");
			}

			Space();

			Space();
			if (Foldout(Application.isPlaying, "Controller State"))
			{
				if (XRDevice.model == "Oculus Rift CV1")
				{
					Space();
					Label("Rift_LeftController");
					EditorGUI.indentLevel++;

					var pos = VRInput.LeftHandPosition;
					var rot = VRInput.LeftHandRotation.eulerAngles;
					EditorGUILayout.BeginHorizontal();
					Label("Position X: " + pos.x.ToString("0.000"));
					Label("Rotation X: " + rot.x.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Y: " + pos.y.ToString("0.000"));
					Label("Rotation Y: " + rot.y.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Z: " + pos.z.ToString("0.000"));
					Label("Rotation Z: " + rot.z.ToString("0.000"));
					EditorGUILayout.EndHorizontal();

					ButtonStateLabel(VRButton.Rift_X);
					ButtonStateLabel(VRButton.Rift_Y);
					ButtonStateLabel(VRButton.Rift_Start);
					ButtonStateLabel(VRButton.Rift_LeftThumbstick);
					ButtonStateLabel(VRButton.Rift_LeftThumbRest);
					ButtonStateLabel(VRButton.Rift_LeftIndexTrigger);
					AxisStateLabel(VRButton.Rift_LeftIndexTrigger);
					AxisStateLabel(VRButton.Rift_LeftMiddleTrigger);
					AxesStateLabel(VRButton.Rift_LeftThumbstick);
					EditorGUI.indentLevel--;

					Space();
					Label("Rift_RightController");
					EditorGUI.indentLevel++;

					pos = VRInput.RightHandPosition;
					rot = VRInput.RightHandRotation.eulerAngles;
					EditorGUILayout.BeginHorizontal();
					Label("Position X: " + pos.x.ToString("0.000"));
					Label("Rotation X: " + rot.x.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Y: " + pos.y.ToString("0.000"));
					Label("Rotation Y: " + rot.y.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Z: " + pos.z.ToString("0.000"));
					Label("Rotation Z: " + rot.z.ToString("0.000"));
					EditorGUILayout.EndHorizontal();

					ButtonStateLabel(VRButton.Rift_A);
					ButtonStateLabel(VRButton.Rift_B);
					ButtonStateLabel(VRButton.Rift_RightThumbstick);
					ButtonStateLabel(VRButton.Rift_RightThumbRest);
					ButtonStateLabel(VRButton.Rift_RightIndexTrigger);
					AxisStateLabel(VRButton.Rift_RightIndexTrigger);
					AxisStateLabel(VRButton.Rift_RightMiddleTrigger);
					AxesStateLabel(VRButton.Rift_RightThumbstick);
					EditorGUI.indentLevel--;
				}
                else if (XRDevice.model == "Vive MV")
                {
					Space();
					Label("Vive_LeftController");
					EditorGUI.indentLevel++;

					var pos = VRInput.LeftHandPosition;
					var rot = VRInput.LeftHandRotation.eulerAngles;
					EditorGUILayout.BeginHorizontal();
					Label("Position X: " + pos.x.ToString("0.000"));
					Label("Rotation X: " + rot.x.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Y: " + pos.y.ToString("0.000"));
					Label("Rotation Y: " + rot.y.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Z: " + pos.z.ToString("0.000"));
					Label("Rotation Z: " + rot.z.ToString("0.000"));
					EditorGUILayout.EndHorizontal();

					ButtonStateLabel(VRButton.Vive_LeftMenu);
					ButtonStateLabel(VRButton.Vive_LeftTrackpad);
					ButtonStateLabel(VRButton.Vive_LeftTrigger);
					AxisStateLabel(VRButton.Vive_LeftTrigger);
					AxisStateLabel(VRButton.Vive_LeftGrip);
					AxesStateLabel(VRButton.Vive_LeftTrackpad);
					EditorGUI.indentLevel--;

					Space();
					Label("Vive_RightController");
					EditorGUI.indentLevel++;

					pos = VRInput.RightHandPosition;
					rot = VRInput.RightHandRotation.eulerAngles;
					EditorGUILayout.BeginHorizontal();
					Label("Position X: " + pos.x.ToString("0.000"));
					Label("Rotation X: " + rot.x.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Y: " + pos.y.ToString("0.000"));
					Label("Rotation Y: " + rot.y.ToString("0.000"));
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					Label("Position Z: " + pos.z.ToString("0.000"));
					Label("Rotation Z: " + rot.z.ToString("0.000"));
					EditorGUILayout.EndHorizontal();

					ButtonStateLabel(VRButton.Vive_RightMenu);
					ButtonStateLabel(VRButton.Vive_RightTrackpad);
					ButtonStateLabel(VRButton.Vive_RightTrigger);
					AxisStateLabel(VRButton.Vive_RightTrigger);
					AxisStateLabel(VRButton.Vive_RightGrip);
					AxesStateLabel(VRButton.Vive_RightTrackpad);
					EditorGUI.indentLevel--;
				}

				Repaint();
			}

			EndInspector();
		}

		private void ButtonStateLabel(VRButton button)
		{
			var state = "Up";
			if (VRInput.GetButton(button))
			{
				state = "Down";
			}
			else if (VRInput.GetTouch(button))
			{
				state = "Touching";
			}
			else if (VRInput.GetHovering(button) > Mathf.Epsilon)
			{
				state = "Hovering";
			}

			Label(button.ToString() + ": " + state);
		}

		private void AxisStateLabel(VRButton button)
		{
			Label(button.ToString() + ": " + VRInput.GetAxis(button).ToString("0.000"));
		}

		private void AxesStateLabel(VRButton button)
		{
			var axes = VRInput.GetAxes(button);
			Label(button.ToString() + " X: " + axes[0].ToString("0.000"));
			Label(button.ToString() + " Y: " + axes[1].ToString("0.000"));
		}
	}
}
