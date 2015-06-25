using Minq.Linq;
using System;

namespace Minq.Caching
{
	/// <summary>
	/// Provides facilities to compose/construct LINQ based Sitecore foundation classes via a caching policy.
	/// </summary>
	public class CachedItemComposer : SItemComposer
	{
		private CachedTemplateRepository _templateRepository;
		private CachedItemRepository _itemRepository;
		private CachedMediaRepository _mediaRepository;

		/// <summary>
		/// Creates a new composer.
		/// </summary>
		/// <param name="itemRepository">The repository supplying cached items.</param>
		/// <param name="templateRepository">The repository supplying cached templates.</param>
		/// <param name="mediaRepository">The repository supplying cached media.</param>
		public CachedItemComposer(CachedItemRepository itemRepository, CachedTemplateRepository templateRepository, CachedMediaRepository mediaRepository)
			: base(itemRepository, templateRepository, mediaRepository)
		{
			_itemRepository = itemRepository;
			_templateRepository = templateRepository;
			_mediaRepository = mediaRepository;
        }

		/// <summary>
		/// Creates a new item.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the item.</param>
		/// <param name="languageName">The language of the item.</param>
		/// <param name="databaseName">The database from which to get the item.</param>
		/// <returns>An item.</returns>
		public override SItem CreateItem(string keyOrPath, string languageName, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				SitecoreItemKey key = new SitecoreItemKey(guid, languageName, databaseName);

				return _itemRepository.GetOrAdd(key, ItemFactory);
			}
			else
			{
				return base.CreateItem(keyOrPath, languageName, databaseName);
			}
		}

		/// <summary>
		/// Creates a new media item.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the media item.</param>
		/// <param name="languageName">The language of the medai item.</param>
		/// <param name="databaseName">The database from which to get the media item.</param>
		/// <returns>A media item.</returns>
		public override SMedia CreateMedia(string keyOrPath, string languageName, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				SitecoreItemKey key = new SitecoreItemKey(guid, languageName, databaseName);

				return _mediaRepository.GetOrAdd(key, MediaFactory);
			}
			else
			{
				return base.CreateMedia(keyOrPath, languageName, databaseName);
			}
		}

		/// <summary>
		/// Creates a new template.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the template.</param>
		/// <param name="databaseName">The database from which to get the template.</param>
		/// <returns>An item.</returns>
		public override STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				SitecoreTemplateKey key = new SitecoreTemplateKey(guid, databaseName);

				return _templateRepository.GetOrAdd(key, TemplateFactory);
			}
			else
			{
				return base.CreateTemplate(keyOrPath, databaseName);
			}
		}

		/// <summary>
		/// Clears all caches.
		/// </summary>
		public void ClearCaches()
		{
			_itemRepository.ClearCache();
			_templateRepository.ClearCache();
			_mediaRepository.ClearCache();
		}

		internal SItem GetOrAdd(ISitecoreItem sitecoreItem)
		{
			CachedItem speculativeItem = new CachedItem(sitecoreItem, this);

			return _itemRepository.GetOrAdd(sitecoreItem.Key, speculativeItem);
		}

		internal STemplate GetOrAdd(ISitecoreTemplate sitecoreTemplate)
		{
			STemplate speculativeTemplate = new CachedTemplate(sitecoreTemplate, this);

			return _templateRepository.GetOrAdd(sitecoreTemplate.Key, speculativeTemplate);
		}

		private SItem ItemFactory(SitecoreItemKey key)
		{
			ISitecoreItem sitecoreItem = _itemRepository.GetItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);

			if (sitecoreItem != null)
			{
				return new CachedItem(sitecoreItem, this);
			}
			else
			{
				return null;
			}
		}

		private STemplate TemplateFactory(SitecoreTemplateKey key)
		{
			ISitecoreTemplate sitecoreTemplate = _templateRepository.GetTemplate(key.Guid.ToString(), key.DatabaseName);

			if (sitecoreTemplate != null)
			{
				return new CachedTemplate(sitecoreTemplate, this);
			}
			else
			{
				return null;
			}
		}

		private SMedia MediaFactory(SitecoreItemKey key)
		{
			ISitecoreMedia sitecoreMedia = _mediaRepository.GetMedia(key.Guid.ToString(), key.LanguageName, key.DatabaseName);

			if (sitecoreMedia != null)
			{
				return new SMedia(sitecoreMedia);
			}
			else
			{
				return null;
			}
		}
	}
}
