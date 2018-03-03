using UnityEditor;
using UnityEngine;

namespace Jake.VR
{
	[CustomEditor(typeof(VRRecenter))]
	public class VRRecenterEditor : BehaviourEditor<VRRecenter>
	{
		public override void OnInspectorGUI()
		{
			BeginInspector(false);

			DrawDefaultInspector();

			if (GUILayout.Button("Recenter"))
			{
				VRRecenter.Recenter();
			}

			EndInspector();
		}
	}
}
