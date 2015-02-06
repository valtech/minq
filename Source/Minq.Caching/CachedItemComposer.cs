using Minq.Linq;
using System;

namespace Minq.Caching
{
	public class CachedItemComposer : SItemComposer
	{
		private CachedTemplateRepository _templateRepository;
		private CachedItemRepository _itemRepository;
		private CachedMediaRepository _mediaRepository;

		public CachedItemComposer(CachedItemRepository itemRepository, CachedTemplateRepository templateRepository, CachedMediaRepository mediaRepository)
			: base(itemRepository, templateRepository, mediaRepository)
		{
			_itemRepository = itemRepository;
			_templateRepository = templateRepository;
			_mediaRepository = mediaRepository;
        }

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
			return new CachedItem(_itemRepository.GetItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName), this);
		}

		private STemplate TemplateFactory(SitecoreTemplateKey key)
		{
			return new CachedTemplate(_templateRepository.GetTemplate(key.Guid.ToString(), key.DatabaseName), this);
		}

		private SMedia MediaFactory(SitecoreItemKey key)
		{
			return new SMedia(_mediaRepository.GetMedia(key.Guid.ToString(), key.LanguageName, key.DatabaseName));
		}
	}
}
