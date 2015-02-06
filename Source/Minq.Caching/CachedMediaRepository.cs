using Minq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	public class CachedMediaRepository : ISitecoreMediaGateway, ICachedRepository<SitecoreItemKey, SMedia>
	{
		private ISitecoreMediaGateway _gateway;
		private ICachedRepository<SitecoreItemKey, SMedia> _repository;

		public CachedMediaRepository(ISitecoreMediaGateway gateway, ICachedRepository<SitecoreItemKey, SMedia> repository)
		{
			_gateway = gateway;
			_repository = repository;
		}

		public ISitecoreMedia GetMedia(string keyOrPath, string languageName, string databaseName)
		{
			return _gateway.GetMedia(keyOrPath, languageName, databaseName);
		}

		public void ClearCache()
		{
			_repository.ClearCache();
		}

		public SMedia GetOrAdd(SitecoreItemKey key, SMedia value)
		{
			return _repository.GetOrAdd(key, value);
		}

		public SMedia GetOrAdd(SitecoreItemKey key, Func<SitecoreItemKey, SMedia> factory)
		{
			return _repository.GetOrAdd(key, factory);
		}
	}
}
