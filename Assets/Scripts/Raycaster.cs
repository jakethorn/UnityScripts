using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jake
{
	[Serializable]
	public class GameObjectEvent : UnityEvent<GameObject>
	{

	}

	public class Raycaster : MonoBehaviour
	{
		public enum Direction { Left, Right, Forward, Back, Up, Down, Mouse };

		public Direction direction;
		public bool showRay;
		public GameObjectEvent onHit;

		private LineRenderer lineRenderer;

		public GameObject Hit
		{
			get
			{
				var hitInfo = default(RaycastHit);
				if (Physics.Raycast(DirectionToRay(direction), out hitInfo))
				{
					return hitInfo.collider.gameObject;
				}
				else
				{
					return null;
				}
			}
		}

		void Awake()
		{
			if (showRay)
			{
				lineRenderer = gameObject.AddComponent<LineRenderer>();
				lineRenderer.widthMultiplier = .01f;
			}
		}

		void Update()
		{
			if (lineRenderer)
			{
				var ray = DirectionToRay(direction);
				lineRenderer.SetPositions(new Vector3[]
				{
					ray.origin,
					ray.origin + ray.direction
				});
			}

			if (onHit.GetPersistentEventCount() > 0)
			{
				var hit = Hit;
				if (hit)
				{
					onHit.Invoke(hit);
				}
			}
		}

		private Ray DirectionToRay(Direction direction)
		{
			switch (direction)
			{
				case Direction.Back: return new Ray(transform.position, -transform.forward);
				case Direction.Forward: return new Ray(transform.position, transform.forward);
				case Direction.Left: return new Ray(transform.position, -transform.right);
				case Direction.Right: return new Ray(transform.position, transform.right);
				case Direction.Up: return new Ray(transform.position, transform.up);
				case Direction.Down: return new Ray(transform.position, -transform.up);
				case Direction.Mouse: return Camera.main.ScreenPointToRay(Input.mousePosition);
				default:
					throw new Exception("Ray Raycaster.DirectionToRay(Direction direction)");
			}
		}
	}
}
