using System;

namespace Minq.Caching
{
	public interface ICachedRepository<TKey, TValue>
	{
		TValue GetOrAdd(TKey key, Func<TKey, TValue> factory);

		TValue GetOrAdd(TKey key, TValue value);

		/// <summary>
		/// When overridden in a derived class, clears the repository cache.
		/// </summary>
		void ClearCache();
    }
}
