using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Minq.Caching
{
	class CachedRepository<TKey>
	{
		private ConcurrentDictionary<TKey, CachedLock> _dictionary;
		private Timer _timer;

		public CachedRepository(TimeSpan period)
			: this(period, EqualityComparer<TKey>.Default)
		{
		}

		public CachedRepository(TimeSpan period, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new ConcurrentDictionary<TKey, CachedLock>(comparer);

			_timer = new Timer(Callback, null, period, period);
		}

		private void Callback(object state)
		{
			Clear();
        }

		public void Clear()
		{
			_dictionary.Clear();
		}

		private CachedLock CachedLockFactory(TKey key)
		{
			return new CachedLock();
        }

		public TValue GetOrAdd<TValue>(TKey key, Func<TValue> factory)
		{
			CachedLock lazy = _dictionary.GetOrAdd(key, CachedLockFactory);

			return lazy.Get(factory);
		}
	}
}
