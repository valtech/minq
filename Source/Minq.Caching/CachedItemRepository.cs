using Minq.Linq;
using System;

namespace Minq.Caching
{
	public class CachedItemRepository : ISitecoreItemGateway, ICachedRepository<SitecoreItemKey, SItem>
	{
		private ISitecoreItemGateway _gateway;
		private ICachedRepository<SitecoreItemKey, SItem> _repository;

		public CachedItemRepository(ISitecoreItemGateway gateway, ICachedRepository<SitecoreItemKey, SItem> repository)
		{
			_gateway = gateway;
			_repository = repository;
        }

		public void ClearCache()
		{
			_repository.ClearCache();
        }

		public ISitecoreItem GetItem(string keyOrPath, string languageName, string databaseName)
		{
			return _gateway.GetItem(keyOrPath, languageName, databaseName);
        }

		public SItem GetOrAdd(SitecoreItemKey key, SItem value)
		{
			return _repository.GetOrAdd(key, value);
        }

		public SItem GetOrAdd(SitecoreItemKey key, Func<SitecoreItemKey, SItem> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
