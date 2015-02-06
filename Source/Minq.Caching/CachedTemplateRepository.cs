using Minq.Linq;
using System;

namespace Minq.Caching
{
	public class CachedTemplateRepository : ISitecoreTemplateGateway, ICachedRepository<SitecoreTemplateKey, STemplate>
	{
		private ISitecoreTemplateGateway _gateway;
		private ICachedRepository<SitecoreTemplateKey, STemplate> _repository;

		public CachedTemplateRepository(ISitecoreTemplateGateway gateway, ICachedRepository<SitecoreTemplateKey, STemplate> repository)
		{
			_gateway = gateway;
			_repository = repository;
		}

		public ISitecoreTemplate GetTemplate(string keyOrPath, string databaseName)
		{
			return _gateway.GetTemplate(keyOrPath, databaseName);
        }

		public void ClearCache()
		{
			_repository.ClearCache();
		}

		public STemplate GetOrAdd(SitecoreTemplateKey key, STemplate value)
		{
			return _repository.GetOrAdd(key, value);
		}

		public STemplate GetOrAdd(SitecoreTemplateKey key, Func<SitecoreTemplateKey, STemplate> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
