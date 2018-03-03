using UnityEngine;
using System;
namespace CommonCode
{
	public static class TransformExtensions
	{
		/*
			Lite methods
		*/

		public static void FromLite(this Transform that, TransformLite lite, Space space = Space.World)
		{
			if (lite == null)
				return;

			switch (space)
			{
				case Space.Self:
					that.localPosition = lite.position;
					that.localRotation = lite.rotation;
					break;
				case Space.World:
					that.position = lite.position;
					that.rotation = lite.rotation;
					break;
			}
		}

		public static TransformLite ToLite(this Transform that, Space space = Space.World)
		{
			switch(space)
			{
				case Space.Self:
					return new TransformLite(that.localPosition, that.localRotation);
				case Space.World:
					return new TransformLite(that.position, that.rotation);
			}

			throw new Exception("I should not be here");
		}
	}
}
