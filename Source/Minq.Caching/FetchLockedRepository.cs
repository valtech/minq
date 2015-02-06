using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Minq.Caching
{
	public class FetchLockedRepository<TKey, TValue> : ICachedRepository<TKey, TValue>
		where TValue : class
	{
		private ConcurrentDictionary<TKey, FetchLock<TValue>> _dictionary;

		public FetchLockedRepository()
			: this(EqualityComparer<TKey>.Default)
		{
		}

		public FetchLockedRepository(IEqualityComparer<TKey> comparer)
		{
			_dictionary = new ConcurrentDictionary<TKey, FetchLock<TValue>>(comparer);
		}

		public void ClearCache()
		{
			_dictionary.Clear();
		}

		private FetchLock<TValue> CachedLockFactory(TKey key)
		{
			return new FetchLock<TValue>();
        }

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			return _dictionary
				.GetOrAdd(key, CachedLockFactory)
				.Fetch(key, factory);
		}

		public TValue GetOrAdd(TKey key, TValue value)
		{
			return _dictionary
				.GetOrAdd(key, CachedLockFactory)
				.Fetch(value);
		}
	}
}
