using UnityEngine;

public enum DilemmaType { Moral, Standard, None };
public enum InfluenceType { Girl, Adults, None };
public enum Gender { Male, Female };

[CreateAssetMenu(fileName = "Cond", menuName = "Study Three/Conditions", order = 1)]
public class ConditionFile : ScriptableObject
{
	public int participantId;

	[Space]

	public DilemmaType dilemmaType;
	public InfluenceType influenceType;
	public bool swapElevators;
	public bool swapHands;

	[Space]

	public Gender gender;
}
