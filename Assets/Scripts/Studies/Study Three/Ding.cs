using UnityEngine;

namespace Jake.Studies.Four
{
	public class Ding : MonoBehaviour
	{
		public AudioSource elevatorOne;
		public AudioSource elevatorTwo;
		public AudioClip clip;

		public float elevator;

		private int previousElevator;

		private int Elevator
		{
			get
			{
				return Mathf.RoundToInt(elevator);
			}
		}

		void Update()
		{
			if (Elevator == 1 && previousElevator != 1)
			{
				previousElevator = 1;
				elevatorOne.PlayOneShot(clip);
			}
			else if (Elevator == 2 && previousElevator != 2)
			{
				previousElevator = 2;
				elevatorTwo.PlayOneShot(clip);
			}
			else if (Elevator == 3 && previousElevator != 3)
			{
				previousElevator = 3;
				elevatorOne.PlayOneShot(clip);
				elevatorTwo.PlayOneShot(clip);
			}
		}
	}
}
