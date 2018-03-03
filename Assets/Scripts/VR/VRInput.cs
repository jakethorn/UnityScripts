using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Jake.VR
{
	public enum VRButton
	{
		None = 0,
		
		Rift_A = 1,
		Rift_B = 2,
		Rift_X = 4,
		Rift_Y = 8,
		Rift_Start = 16,
		Rift_LeftIndexTrigger = 32,
		Rift_RightIndexTrigger = 64,
		Rift_LeftMiddleTrigger = 128,
		Rift_RightMiddleTrigger = 256,
		Rift_LeftThumbstick = 512,
		Rift_RightThumbstick = 1024,
		Rift_LeftThumbRest = 2048,
		Rift_RightThumbRest = 4096,

		Vive_LeftMenu = 8192,
		Vive_RightMenu = 16384,
		Vive_LeftTrackpad = 32768,
		Vive_RightTrackpad = 65536,
		Vive_LeftTrigger = 131072,
		Vive_RightTrigger = 262144,
		Vive_LeftGrip = 524288,
		Vive_RightGrip = 1048576
	};
	
	public class VRInput : HandManager
	{
		/*
		 *	Static
		 */

		private readonly static Dictionary<VRButton, string> pressables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_A, "joystick button 0" },
			{ VRButton.Rift_B, "joystick button 1" },
			{ VRButton.Rift_X, "joystick button 2" },
			{ VRButton.Rift_Y, "joystick button 3" },
			{ VRButton.Rift_Start, "joystick button 7" },
			{ VRButton.Rift_LeftThumbstick, "joystick button 8" },
			{ VRButton.Rift_RightThumbstick, "joystick button 9" },

			{ VRButton.Vive_LeftMenu, "joystick button 2" },
			{ VRButton.Vive_RightMenu, "joystick button 0" },
			{ VRButton.Vive_LeftTrackpad, "joystick button 8" },
			{ VRButton.Vive_RightTrackpad, "joystick button 9" }
		};

		private readonly static Dictionary<VRButton, string > touchables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_A, "joystick button 10" },
			{ VRButton.Rift_B, "joystick button 11" },
			{ VRButton.Rift_X, "joystick button 12" },
			{ VRButton.Rift_Y, "joystick button 13" },
			{ VRButton.Rift_LeftThumbstick, "joystick button 16" },
			{ VRButton.Rift_RightThumbstick, "joystick button 17" },
			{ VRButton.Rift_LeftThumbRest, "joystick button 18" },
			{ VRButton.Rift_RightThumbRest, "joystick button 19" },
			{ VRButton.Rift_LeftIndexTrigger, "joystick button 14" },
			{ VRButton.Rift_RightIndexTrigger, "joystick button 15" },

			{ VRButton.Vive_LeftTrackpad, "joystick button 16" },
			{ VRButton.Vive_RightTrackpad, "joystick button 17" },
			{ VRButton.Vive_LeftTrigger, "joystick button 14" },
			{ VRButton.Vive_RightTrigger, "joystick button 15" }
		};

		private readonly static Dictionary<VRButton, string> hoverables = new Dictionary<VRButton, string>
		{
			{ VRButton.Rift_LeftThumbstick, "Left Rift Thumbstick Hover" },
			{ VRButton.Rift_LeftThumbRest, "Left Rift Thumb Rest Hover" },
			{ VRButton.Rift_LeftIndexTrigger, "Left Rift Index Trigger Hover" },
			{ VRButton.Rift_RightThumbstick, "Right Rift Thumbstick Hover" },
			{ VRButton.Rift_RightThumbRest, "Right Rift Thumb Rest Hover" },
			{ VRButton.Rift_RightIndexTrigger, "Right Rift Index Trigger Hover" }
		};

		private readonly static Dictionary<VRButton, string> axes = new Dictionary<VRButton, string>
		{
			{VRButton.Rift_LeftIndexTrigger, "Left Rift Index Trigger" },
			{VRButton.Rift_LeftMiddleTrigger, "Left Rift Middle Trigger" },
			{VRButton.Rift_RightIndexTrigger, "Right Rift Index Trigger" },
			{VRButton.Rift_RightMiddleTrigger, "Right Rift Middle Trigger" },

			{VRButton.Vive_LeftTrigger, "Left Vive Trigger" },
			{VRButton.Vive_LeftGrip, "Left Vive Grip" },
			{VRButton.Vive_RightTrigger, "Right Vive Trigger" },
			{VRButton.Vive_RightGrip, "Right Vive Grip" }
		};
		
		public static Vector3 LeftHandPosition { get { return InputTracking.GetLocalPosition(XRNode.LeftHand); } }
		public static Vector3 RightHandPosition { get { return InputTracking.GetLocalPosition(XRNode.RightHand); } }
		public static Quaternion LeftHandRotation { get { return InputTracking.GetLocalRotation(XRNode.LeftHand); } }
		public static Quaternion RightHandRotation { get { return InputTracking.GetLocalRotation(XRNode.RightHand); } }

		public static bool GetButtonDown(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKeyDown(pressables[button]) : false;
		}

		public static bool GetButton(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKey(pressables[button]) : false;
		}

		public static bool GetButtonUp(VRButton button)
		{
			return pressables.ContainsKey(button) ? Input.GetKeyUp(pressables[button]) : false;
		}

		public static bool GetTouchDown(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKeyDown(touchables[button]) : false;
		}
		
		public static bool GetTouch(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKey(touchables[button]) : false;
		}

		public static bool GetTouchUp(VRButton button)
		{
			return touchables.ContainsKey(button) ? Input.GetKeyUp(touchables[button]) : false;
		}

		public static float GetHovering(VRButton button)
		{
			return hoverables.ContainsKey(button) ? Input.GetAxis(hoverables[button]) : 0;
		}
		
		public static float GetAxis(VRButton button)
		{
			foreach (var pair in axes)
			{
				var axis = pair.Key;
				if ((axis & button) == axis)
				{
					var value = Input.GetAxis(axes[axis]);
					if (value > Mathf.Epsilon)
						return value;
				}
			}

			return 0;
		}

		public static Vector2 GetAxes(VRButton button)
		{
			if ((button & VRButton.Rift_LeftThumbstick) == VRButton.Rift_LeftThumbstick)
			{
				var value = new Vector2(
					Input.GetAxis("Left Rift Thumbstick X Axis"),
					Input.GetAxis("Left Rift Thumbstick Y Axis")
				);

				if (value.x > Mathf.Epsilon || value.x < -Mathf.Epsilon || value.y > Mathf.Epsilon || value.y < -Mathf.Epsilon)
					return value;
			}

			if ((button & VRButton.Rift_RightThumbstick) == VRButton.Rift_RightThumbstick)
			{
				var value = new Vector2(
					Input.GetAxis("Right Rift Thumbstick X Axis"),
					Input.GetAxis("Right Rift Thumbstick Y Axis")
				);

				if (value.x > Mathf.Epsilon || value.x < -Mathf.Epsilon || value.y > Mathf.Epsilon || value.y < -Mathf.Epsilon)
					return value;
			}

			if ((button & VRButton.Vive_LeftTrackpad) == VRButton.Vive_LeftTrackpad)
			{
				var value = new Vector2(
					Input.GetAxis("Left Vive Trackpad X Axis"),
					Input.GetAxis("Left Vive Trackpad Y Axis")
				);

				if (value.x > Mathf.Epsilon || value.x < -Mathf.Epsilon || value.y > Mathf.Epsilon || value.y < -Mathf.Epsilon)
					return value;
			}

			if ((button & VRButton.Vive_RightTrackpad) == VRButton.Vive_RightTrackpad)
			{
				var value = new Vector2(
					Input.GetAxis("Right Vive Trackpad X Axis"),
					Input.GetAxis("Right Vive Trackpad Y Axis")
				);

				if (value.x > Mathf.Epsilon || value.x < -Mathf.Epsilon || value.y > Mathf.Epsilon || value.y < -Mathf.Epsilon)
					return value;
			}
			
			return Vector2.zero;
		}

		/*
		 * Instance
		 */

		public bool applyOffset;
		public Vector3 leftPositionOffset;
		public Vector3 leftRotationOffset;
		public Vector3 rightPositionOffset;
		public Vector3 rightRotationOffset;

		public bool editLeftOffset;
		public bool editRightOffset;
		
		private GameObject realLeftHand;
		private GameObject realRightHand;

		void Awake()
		{
			if (editLeftOffset)
			{
				realLeftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
				realLeftHand.transform.localScale = new Vector3(.05f, .05f, .05f);
				realLeftHand.transform.parent = leftHand.parent;
			}

			if (editRightOffset)
			{
				realRightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
				realRightHand.transform.localScale = new Vector3(.05f, .05f, .05f);
				realRightHand.transform.parent = rightHand.parent;
			}
		}

		void Update()
		{
			if (editLeftOffset)
			{
				realLeftHand.transform.localRotation = LeftHandRotation;
				realLeftHand.transform.localPosition = LeftHandPosition;
				
				var posCoeff = Time.deltaTime * .1f;
				var vec = new Vector3(posCoeff, posCoeff, posCoeff);
				var leftStick = GetAxes(VRButton.Rift_LeftThumbstick | VRButton.Vive_LeftTrackpad);
				vec.x *= leftStick[0];
				vec.y *= leftStick[1];
				vec.z *= GetAxis(VRButton.Rift_LeftIndexTrigger | VRButton.Vive_LeftTrigger) - GetAxis(VRButton.Rift_LeftMiddleTrigger | VRButton.Vive_LeftGrip);
				leftPositionOffset += vec;

				var rotCoeff = Time.deltaTime * 10f;
				vec = new Vector3(rotCoeff, rotCoeff, rotCoeff);
				var rightStick = GetAxes(VRButton.Rift_RightThumbstick | VRButton.Vive_RightTrackpad);
				vec.x *= rightStick[0];
				vec.y *= rightStick[1];
				vec.z *= GetAxis(VRButton.Rift_RightIndexTrigger | VRButton.Vive_RightTrigger) - GetAxis(VRButton.Rift_RightMiddleTrigger | VRButton.Vive_RightGrip);
				leftRotationOffset += vec;
			}

			if (editRightOffset)
			{
				realRightHand.transform.localRotation = RightHandRotation;
				realRightHand.transform.localPosition = RightHandPosition;

				var posCoeff = Time.deltaTime * .1f;
				var vec = new Vector3(posCoeff, posCoeff, posCoeff);
				var leftStick = GetAxes(VRButton.Rift_LeftThumbstick | VRButton.Vive_LeftTrackpad);
				vec.x *= leftStick[0];
				vec.y *= leftStick[1];
				vec.z *= GetAxis(VRButton.Rift_LeftIndexTrigger | VRButton.Vive_LeftTrigger) - GetAxis(VRButton.Rift_LeftMiddleTrigger | VRButton.Vive_LeftGrip);
				rightPositionOffset += vec;

				var rotCoeff = Time.deltaTime * 10f;
				vec = new Vector3(rotCoeff, rotCoeff, rotCoeff);
				var rightStick = GetAxes(VRButton.Rift_RightThumbstick | VRButton.Vive_RightTrackpad);
				vec.x *= rightStick[0];
				vec.y *= rightStick[1];
				vec.z *= GetAxis(VRButton.Rift_RightIndexTrigger | VRButton.Vive_RightTrigger) - GetAxis(VRButton.Rift_RightMiddleTrigger | VRButton.Vive_RightGrip);
				rightRotationOffset += vec;
			}

			if (leftHand)
			{
				leftHand.localPosition = LeftHandPosition;
				leftHand.localRotation = LeftHandRotation;
			}
			
			if (rightHand)
			{
				rightHand.localPosition = RightHandPosition;
				rightHand.localRotation = RightHandRotation;
			}
			
			if (applyOffset)
			{
				if (leftHand != null)
				{
					leftHand.localRotation *= Quaternion.Euler(leftRotationOffset);
					leftHand.localPosition += LeftHandRotation * leftPositionOffset;
				}
				
				if (rightHand != null)
				{
					rightHand.localRotation *= Quaternion.Euler(rightRotationOffset);
					rightHand.localPosition += RightHandRotation * rightPositionOffset;
				}
			}
		}

		void OnApplicationQuit()
		{
			Destroy(realLeftHand);
			Destroy(realRightHand);
		}
	}
}
