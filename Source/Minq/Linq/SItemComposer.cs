using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Minq.Linq
{
	public class SItemComposer
	{
		private ISitecoreItemGateway _itemGateway;
		private ISitecoreTemplateGateway _templateGateway;
		private ISitecoreMediaGateway _mediaGateway;

		public SItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
		{
			_itemGateway = itemGateway;
			_templateGateway = templateGateway;
			_mediaGateway = mediaGateway;
		}

		public SMedia CreateMedia(SField field, string languageName, string databaseName)
		{
			if (!SMedia.IsMediaField(field))
			{
				throw new Exception("Not a media field");
			}

			XElement element = XDocument.Parse(field.Value<string>()).Descendants("image").First();

			return CreateMedia((string)element.Attribute("mediaid"), languageName, databaseName);
		}

		public SItem CreateItem(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreItem sitecoreItem = _itemGateway.GetItem(keyOrPath, languageName, databaseName);

			if (sitecoreItem != null)
			{
				return new SItem(sitecoreItem, this);
			}

			return null;
		}

		public STemplate CreateTemplate(SitecoreTemplateKey templateKey)
		{
			return new STemplate(_templateGateway.GetTemplate(templateKey));
		}

		public SMedia CreateMedia(string keyOrPath, string languageName, string databaseName)
		{
			return new SMedia(_mediaGateway.GetMedia(keyOrPath, languageName, databaseName));
		}
	}
}
