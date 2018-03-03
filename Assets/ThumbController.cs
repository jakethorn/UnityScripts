using UnityEngine;

namespace Jake.VR
{ 
	public class ThumbController : MonoBehaviour
	{
		public Transform	leftThumb, 
							rightThumb;

		public Vector3		leftThumbRest		= new Vector3(40, -10, -30), 
							leftThumbTrackpad	= new Vector3(15, -10, -30), 
							rightThumbRest		= new Vector3(40, 10, 30), 
							rightThumbTrackpad	= new Vector3(15, 10, 30);

		public float		speed = 5;
	
		private Vector3 leftThumbTarget, rightThumbTarget;

		void Update()
		{
			if (VRInput.GetTouchDown(VRButton.Vive_LeftTrackpad))
			{
				leftThumbTarget = leftThumbTrackpad;
			}
			else if (VRInput.GetTouchUp(VRButton.Vive_LeftTrackpad))
			{
				leftThumbTarget = leftThumbRest;
			}

			if (VRInput.GetTouchDown(VRButton.Vive_RightTrackpad))
			{
				rightThumbTarget = rightThumbTrackpad;
			}
			else if (VRInput.GetTouchUp(VRButton.Vive_RightTrackpad))
			{
				rightThumbTarget = rightThumbRest;
			}
		}

		void FixedUpdate()
		{
			Lerp(leftThumb,	leftThumbTarget);
			Lerp(rightThumb, rightThumbTarget);
		}

		private void Lerp(Transform thumb, Vector3 target)
		{
			var delta = target - thumb.localEulerAngles;
			delta = Math.Clamp(delta, new Vector3(-speed, 0, 0), new Vector3(speed, 0, 0));
			thumb.localEulerAngles += delta;
		}
	}
}
