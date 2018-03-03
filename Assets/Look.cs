using UnityEngine;
using Jake;
#if USING_LIPSYNC
using RogoDigital.Lipsync;
#endif
public class Look : MonoBehaviour
{
	public Transform body, head, target;
#if USING_LIPSYNC
	public LipSync face;
	public LipSyncData smile;
#endif
	public float lookToward, lookAway;

	private bool firstUpdate = true;

	void Update()
	{
		if (firstUpdate)
		{
			firstUpdate = false;
			StartLooking();
		}
	}

	private void StartLooking()
	{
		// start to look at user
		Runner.RunOnce(() =>
		{
			Lerper.LerpOverTime((v) => { head.forward = v; }, head.forward, target.position - head.position, 1);
		},
		lookToward);
#if USING_LIPSYNC
		// smile at user
		Runner.RunOnce(() =>
		{
			face.Play(smile);
		},
		lookToward + 1);
#endif
		// look away from user
		Runner.RunOnce(() =>
		{
			Lerper.LerpOverTime((v) => { head.forward = v; }, head.forward, body.forward, 1);
		},
		lookAway);
	}
}
