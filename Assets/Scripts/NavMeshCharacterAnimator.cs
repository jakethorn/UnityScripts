using UnityEngine;
using UnityEngine.AI;

namespace Jake
{
	public class NavMeshCharacterAnimator : MonoBehaviour
	{
		public Animator animator;
		public NavMeshAgent agent;

		[Space]

		public string animatorParameter;
		public Transform target;

		[Space]

		[Range(0, 1)]
		public float forwardCoeff = .5f;

		[Space]

		public ObstacleAvoidanceType obstacleAvoidance;

		private bool targetSet;
		private int animatorParameterHash;

		void Awake()
		{
			animatorParameterHash = Animator.StringToHash(animatorParameter);

			agent.obstacleAvoidanceType = obstacleAvoidance;
		}

		void Update()
		{
			if (target.gameObject.activeSelf)
			{
				if (targetSet == false)
				{
					targetSet = true;
					agent.destination = target.position;
				}
			}
			else
			{
				agent.destination = agent.transform.position;
			}

			animator.SetFloat(
				animatorParameterHash,
				forwardCoeff * agent.velocity.magnitude / agent.speed
			);
		}
	}
}
