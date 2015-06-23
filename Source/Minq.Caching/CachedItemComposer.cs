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
