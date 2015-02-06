﻿namespace Minq.Linq
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

		public virtual SMedia CreateMedia(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreMedia sitecoreMedia = _mediaGateway.GetMedia(keyOrPath, languageName, databaseName);

			if (sitecoreMedia != null)
			{
				return new SMedia(sitecoreMedia);
			}

			return null;
		}

		public virtual SItem CreateItem(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreItem sitecoreItem = _itemGateway.GetItem(keyOrPath, languageName, databaseName);

			if (sitecoreItem != null)
			{
				return new SItem(sitecoreItem, this);
			}

			return null;
		}

		public virtual STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			return new STemplate(_templateGateway.GetTemplate(keyOrPath, databaseName));
		}		
	}
}
