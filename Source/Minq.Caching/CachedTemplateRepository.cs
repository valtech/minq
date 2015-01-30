using Minq.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	public class CachedTemplateRepository
	{
		private ConcurrentDictionary<SitecoreTemplateKey, STemplate> _templateDictionary = new ConcurrentDictionary<SitecoreTemplateKey, STemplate>();
		private ISitecoreTemplateGateway _templateGateway;

        public CachedTemplateRepository(ISitecoreTemplateGateway templateGateway)
		{
			_templateGateway = templateGateway;
		}

		public STemplate GetOrAdd(ISitecoreTemplate sitecoreTemplate)
		{
			STemplate speculativeTemplate = new CachedTemplate(sitecoreTemplate, this);

			return _templateDictionary.GetOrAdd(sitecoreTemplate.Key, speculativeTemplate);
		}

		public STemplate GetOrAdd(SitecoreTemplateKey key)
		{
			return _templateDictionary.GetOrAdd(key, TemplateFactory);
		}

		private STemplate TemplateFactory(SitecoreTemplateKey key)
		{
			return new CachedTemplate(_templateGateway.GetTemplate(key.Guid.ToString(), key.DatabaseName), this);
		}

		public void ClearCache()
		{
			_templateDictionary.Clear();
        }
	}
}
