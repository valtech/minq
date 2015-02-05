using Minq.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	public class CachedItemComposer : SItemComposer
	{
		private ISitecoreItemGateway _itemGateway;
		private ISitecoreTemplateGateway _templateGateway;
		private ICachedRepository<SitecoreTemplateKey, STemplate> _templateDictionary = new ConcurrentRespository<SitecoreTemplateKey, STemplate>();
		private ICachedRepository<SitecoreItemKey, SItem> _itemDictionary = new ConcurrentRespository<SitecoreItemKey, SItem>();

		public CachedItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
			: base(itemGateway, templateGateway, mediaGateway)
		{
			_itemGateway = itemGateway;
			_templateGateway = templateGateway;
        }

		public override STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				SitecoreTemplateKey key = new SitecoreTemplateKey(guid, databaseName);

				return _templateDictionary.GetOrAdd(key, TemplateFactory);
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

				return _itemDictionary.GetOrAdd(key, ItemFactory);
			}
			else
			{
				return base.CreateItem(keyOrPath, languageName, databaseName);
			}
		}

		public SItem GetOrAdd(ISitecoreItem sitecoreItem)
		{
			CachedItem speculativeItem = new CachedItem(sitecoreItem, this);

			return _itemDictionary.GetOrAdd(sitecoreItem.Key, speculativeItem);
		}

		private SItem ItemFactory(SitecoreItemKey key)
		{
			return new CachedItem(_itemGateway.GetItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName), this);
		}

		private STemplate TemplateFactory(SitecoreTemplateKey key)
		{
			return new CachedTemplate(_templateGateway.GetTemplate(key.Guid.ToString(), key.DatabaseName), this);
		}

		public STemplate GetOrAdd(ISitecoreTemplate sitecoreTemplate)
		{
			STemplate speculativeTemplate = new CachedTemplate(sitecoreTemplate, this);

			return _templateDictionary.GetOrAdd(sitecoreTemplate.Key, speculativeTemplate);
		}

		public void ClearCaches()
		{
			_templateDictionary.Clear();
        }
	}
}
