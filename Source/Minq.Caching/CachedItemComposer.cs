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
		private ConcurrentDictionary<Guid, STemplate> _templateDictionary = new ConcurrentDictionary<Guid, STemplate>();

		public CachedItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
			: base(itemGateway, templateGateway, mediaGateway)
		{
			_templateGateway = templateGateway;
        }

		public override STemplate CreateTemplate(string keyOrPath, string languageName)
		{
			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				STemplate template;

				if (_templateDictionary.TryGetValue(guid, out template))
				{
					return template;
                }

				template = new CachedTemplate(_templateGateway.GetTemplate(keyOrPath, languageName));

				_templateDictionary[guid] = template;

				return template;
            }

			return base.CreateTemplate(keyOrPath, languageName);
		}
	}
}
