using UnityEngine;
using System;

namespace CommonCode
{
	public abstract class Processor<T> : MonoBehaviour
	{
		public virtual T Process(T t)
		{
			throw new NotImplementedException();
		}
		
		public virtual T Process(T t, object a)
		{
			throw new NotImplementedException();
		}

		public virtual T Process(T t, object a, object b)
		{
			throw new NotImplementedException();
		}

		public virtual T Process(T t, object a, object b, object c)
		{
			throw new NotImplementedException();
		}

		public virtual void Process(ref T t)
		{
			throw new NotImplementedException();
		}
		
		public virtual void Process(ref T t, object a)
		{
			throw new NotImplementedException();
		}

		public virtual void Process(ref T t, object a, object b)
		{
			throw new NotImplementedException();
		}

		public virtual void Process(ref T t, object a, object b, object c)
		{
			throw new NotImplementedException();
		}
	}
}
