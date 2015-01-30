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
		private CachedTemplateRepository _templateRepository;

		public CachedItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
			: base(itemGateway, templateGateway, mediaGateway)
		{
			_templateRepository = new CachedTemplateRepository(templateGateway);
        }

		public override STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				SitecoreTemplateKey key = new SitecoreTemplateKey(guid, databaseName);

				return _templateRepository.GetOrAdd(key);
			}
			else
			{
				return base.CreateTemplate(keyOrPath, databaseName);
			}
		}
		
		public void ClearCaches()
		{
			_templateRepository.ClearCache();
        }
	}
}
