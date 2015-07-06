using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Minq.Caching
{
	/// <summary>
	/// Defines a thread-safe cache with the further property which guarentees uniqueness of a cache entry.
	/// </summary>
	/// <typeparam name="TKey">The type of cache key.</typeparam>
	/// <typeparam name="TValue">The type of cache value.</typeparam>
	public class ConcurrentRespository<TKey, TValue> : ICachedRepository<TKey, TValue>
	{
		private ConcurrentDictionary<TKey, TValue> _dictionary;

		/// <summary>
		/// Creates a new concurrent repository.
		/// </summary>
		public ConcurrentRespository()
			: this(EqualityComparer<TKey>.Default)
		{
		}

		/// <summary>
		/// Creates a new concurrent repository.
		/// </summary>
		/// <param name="comparer">The equality comparer to use to compare keys.</param>
		public ConcurrentRespository(IEqualityComparer<TKey> comparer)
		{
			_dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Get or add an element from the cache.
		/// </summary>
		/// <param name="key">The key of the element.</param>
		/// <param name="factory">The factory to use to create the element if it is not already in the cache.</param>
		/// <returns>The cached element.</returns>
		/// <remakrs>
		/// If the cost of creating the element is comparable to the cost of creating the factory do not use this version of the method.
		/// </remakrs>
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			return _dictionary.GetOrAdd(key, factory);
		}

		/// <summary>
		/// Clear the repository cache.
		/// </summary>
		public void ClearCache()
		{
			_dictionary.Clear();
        }

		/// <summary>
		/// Get or add an element from the cache.
		/// </summary>
		/// <param name="key">The key of the element.</param>
		/// <param name="value">The element to add if there is no element for the given key aleady in the cache.</param>
		/// <returns>The cached element.</returns>
		public TValue GetOrAdd(TKey key, TValue value)
		{
			return _dictionary
				.GetOrAdd(key, value);
		}
	}
}
