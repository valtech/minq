using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	public class SItemComposer
	{
		private ISitecoreItemGateway _itemGateway;
		private ISitecoreTemplateGateway _templateGateway;

		public SItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway)
		{
			_itemGateway = itemGateway;
			_templateGateway = templateGateway;
		}

		public SItem CreateItem(string keyOrPath, string languageName, string databaseName)
		{
			return new SItem(_itemGateway.GetItem(keyOrPath, languageName, databaseName), this);
		}

		public STemplate CreateTemplate(SitecoreTemplateKey templateKey)
		{
			return new STemplate(_templateGateway.GetTemplate(templateKey));
		}
	}
}
