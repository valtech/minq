using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	abstract class LazyCollection<T> : ICollection<T>
			where T : class, new()
	{
		public void Add(T item)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			throw new NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new NotImplementedException();
		}

		public abstract int Count
		{
			get;
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public abstract IEnumerator<T> GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
