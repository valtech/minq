using Minq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	/// <summary>
	/// Defines an object that represents a repository of cached media.
	/// </summary>
	public class CachedMediaRepository : ISitecoreMediaGateway, ICachedRepository<SitecoreItemKey, SMedia>
	{
		private ISitecoreMediaGateway _gateway;
		private ICachedRepository<SitecoreItemKey, SMedia> _repository;

		/// <summary>
		/// Creates a new cached media item repository.
		/// </summary>
		/// <param name="gateway">The media item gateway.</param>
		/// <param name="repository">The underlying repository to use for caching.</param>
		public CachedMediaRepository(ISitecoreMediaGateway gateway, ICachedRepository<SitecoreItemKey, SMedia> repository)
		{
			_gateway = gateway;
			_repository = repository;
		}

		/// <summary>
		/// Returns the Sitecore media item for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the item to return.</param>
		/// <param name="languageName">The language of the item to return.</param>
		/// <param name="databaseName">Thedatabse of the item to return.</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public ISitecoreMedia GetMedia(string keyOrPath, string languageName, string databaseName)
		{
			return _gateway.GetMedia(keyOrPath, languageName, databaseName);
		}

		/// <summary>
		/// Clear the repository cache.
		/// </summary>
		public void ClearCache()
		{
			_repository.ClearCache();
		}

		/// <summary>
		/// Get or add a media item from the cache.
		/// </summary>
		/// <param name="key">The key of the media item.</param>
		/// <param name="value">The value to add if the media item is not in the cache.</param>
		/// <returns>The cached media item.</returns>
		public SMedia GetOrAdd(SitecoreItemKey key, SMedia value)
		{
			return _repository.GetOrAdd(key, value);
		}

		/// <summary>
		/// Get or add a media item from the cache.
		/// </summary>
		/// <param name="key">The key of the media item.</param>
		/// <param name="factory">The factory to use to create the media item is not in the cache.</param>
		/// <returns>The cached media item.</returns>
		/// <remakrs>
		/// If the cost of creating the media item is comparable to the cost of creating the factory, do not use this version of the method.
		/// </remakrs>
		public SMedia GetOrAdd(SitecoreItemKey key, Func<SitecoreItemKey, SMedia> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
