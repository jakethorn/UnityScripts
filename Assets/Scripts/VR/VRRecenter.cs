using UnityEngine;
using UnityEngine.XR;

namespace Jake.VR
{
	public class VRRecenter : MonoBehaviour
	{
		public bool onAwake;
		public float delay;
		
		void Awake()
		{
			if (onAwake)
			{
				Runner.RunOnce(Recenter, delay);
			}
		}

		// for use in the unity editor
		public void Recenter_Instance()
		{
			Recenter();
		}

		public static void Recenter()
		{
			if (XRDevice.model == "Vive MV" && XRDevice.GetTrackingSpaceType() == TrackingSpaceType.RoomScale)
			{
				XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
			}

			InputTracking.Recenter();
		}
	}
}
