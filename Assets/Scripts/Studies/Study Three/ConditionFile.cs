using UnityEngine;

namespace Jake.Studies.Four
{
	public enum DilemmaType { Moral, Standard, None };
	public enum InfluenceType { Girl, Adults, None };
	public enum Gender { Male, Female };

	[CreateAssetMenu(fileName = "Cond", menuName = "Study Three/Conditions", order = 1)]
	public class ConditionFile : ScriptableObject
	{
		public int participantId;
	}
}
