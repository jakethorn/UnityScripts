using UnityEngine.XR;

namespace Jake.VR
{
	public enum VRModel { None, Rift, Vive };

	public class VRDevice
	{
		public static VRModel Model
		{
			get
			{
				if (XRDevice.model == "Oculus Rift CV1")
				{
					return VRModel.Rift;
				}
				else if (XRDevice.model == "Vive MV")
				{
					return VRModel.Vive;
				}
				else
				{
					return VRModel.None;
				}
			}
		}
	}
}
