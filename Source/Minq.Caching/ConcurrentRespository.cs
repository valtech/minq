using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	class ConcurrentRespository<TKey, TValue> : ICachedRepository<TKey, TValue>
	{
		private ConcurrentDictionary<TKey, TValue> _dictionary;

		public ConcurrentRespository()
			: this(EqualityComparer<TKey>.Default)
		{
		}

		public ConcurrentRespository(IEqualityComparer<TKey> comparer)
		{
			_dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
		}

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			return _dictionary.GetOrAdd(key, factory);
		}

		public void Clear()
		{
			_dictionary.Clear();
        }

		public TValue GetOrAdd(TKey key, TValue value)
		{
			return _dictionary
				.GetOrAdd(key, value);
		}
	}
}
