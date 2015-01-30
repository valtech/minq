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
		private ISitecoreTemplateGateway _templateGateway;
		private ConcurrentDictionary<string, STemplate> _templateDictionary = new ConcurrentDictionary<string, STemplate>();

		public CachedItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
			: base(itemGateway, templateGateway, mediaGateway)
		{
			_templateGateway = templateGateway;
        }

		public override STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				string key = String.Format("{0}/{1}", guid, databaseName.ToLowerInvariant());

				STemplate template;

				if (_templateDictionary.TryGetValue(key, out template))
				{
					return template;
                }

				template = new CachedTemplate(_templateGateway.GetTemplate(keyOrPath, databaseName));

				_templateDictionary[key] = template;

				return template;
            }

			return base.CreateTemplate(keyOrPath, databaseName);
		}
	}
}
