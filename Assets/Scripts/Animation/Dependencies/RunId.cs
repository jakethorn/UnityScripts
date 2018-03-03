using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "RunId", menuName = "RunId", order = 1)]
public class RunId : ScriptableObject
{
	public int runTimes;

	public static RunId instance;

	void Awake()
	{
		instance = this;
	}

	void OnEnable()
	{
		instance = this;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void IncrementRunTimes()
	{
		if (instance == null)
			instance = AssetDatabase.LoadAssetAtPath<RunId>("Assets/Misc/RunId.asset");

		instance.runTimes++;
	}
}
