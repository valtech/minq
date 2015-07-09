using Minq.Linq;
using System;

namespace Minq.Caching
{
	/// <summary>
	/// Defines an object that represents a repository of cached Sitecore items.
	/// </summary>
	public class CachedItemRepository : ISitecoreItemGateway, ICachedRepository<SitecoreItemKey, SItem>
	{
		private ISitecoreItemGateway _gateway;
		private ICachedRepository<SitecoreItemKey, SItem> _repository;

		/// <summary>
		/// Creates a new cached item repository.
		/// </summary>
		/// <param name="gateway">The item gateway.</param>
		/// <param name="repository">The underlying repository to use for caching.</param>
		public CachedItemRepository(ISitecoreItemGateway gateway, ICachedRepository<SitecoreItemKey, SItem> repository)
		{
			_gateway = gateway;
			_repository = repository;
        }

		/// <summary>
		/// Clear the repository cache.
		/// </summary>
		public void ClearCache()
		{
			_repository.ClearCache();
        }

		/// <summary>
		/// Returns the Sitecore item for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the item to return.</param>
		/// <param name="languageName">The language of the item to return.</param>
		/// <param name="databaseName">The database of the item to return.</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public ISitecoreItem GetItem(string keyOrPath, string languageName, string databaseName)
		{
			return _gateway.GetItem(keyOrPath, languageName, databaseName);
        }

		/// <summary>
		/// Get or add an item from the cache.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="value">The value to add if the item is not in the cache.</param>
		/// <returns>The cached item.</returns>
		public SItem GetOrAdd(SitecoreItemKey key, SItem value)
		{
			return _repository.GetOrAdd(key, value);
        }

		/// <summary>
		/// Get or add an item from the cache.
		/// </summary>
		/// <param name="key">The key of the item.</param>
		/// <param name="factory">The factory to use to create the item if it is not already in the cache.</param>
		/// <returns>The cached item.</returns>
		/// <remakrs>
		/// If the cost of creating the item is comparable to the cost of creating the factory do not use this version of the method.
		/// </remakrs>
		public SItem GetOrAdd(SitecoreItemKey key, Func<SitecoreItemKey, SItem> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
