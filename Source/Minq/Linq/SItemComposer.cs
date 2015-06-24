namespace Minq.Linq
{
	/// <summary>
	/// Provides facilities to compose/construct LINQ based Sitecore foundation classes. 
	/// </summary>
	public class SItemComposer
	{
		private ISitecoreItemGateway _itemGateway;
		private ISitecoreTemplateGateway _templateGateway;
		private ISitecoreMediaGateway _mediaGateway;

		/// <summary>
		/// Creates a new composer.
		/// </summary>
		/// <param name="itemGateway">The gateway supplying raw items.</param>
		/// <param name="templateGateway">The gateway supplying raw templates.</param>
		/// <param name="mediaGateway">The gateway supplying raw media.</param>
		public SItemComposer(ISitecoreItemGateway itemGateway, ISitecoreTemplateGateway templateGateway, ISitecoreMediaGateway mediaGateway)
		{
			_itemGateway = itemGateway;
			_templateGateway = templateGateway;
			_mediaGateway = mediaGateway;
		}

		/// <summary>
		/// Creates a new media item.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the media item.</param>
		/// <param name="languageName">The language of the medai item.</param>
		/// <param name="databaseName">The database from which to get the media item.</param>
		/// <returns>A media item.</returns>
		public virtual SMedia CreateMedia(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreMedia sitecoreMedia = _mediaGateway.GetMedia(keyOrPath, languageName, databaseName);

			if (sitecoreMedia != null)
			{
				return new SMedia(sitecoreMedia);
			}

			return null;
		}

		/// <summary>
		/// Creates a new item.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the item.</param>
		/// <param name="languageName">The language of the item.</param>
		/// <param name="databaseName">The database from which to get the item.</param>
		/// <returns>An item.</returns>
		public virtual SItem CreateItem(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreItem sitecoreItem = _itemGateway.GetItem(keyOrPath, languageName, databaseName);

			if (sitecoreItem != null)
			{
				return new SItem(sitecoreItem, this);
			}

			return null;
		}

		/// <summary>
		/// Creates a new template.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the template.</param>
		/// <param name="databaseName">The database from which to get the template.</param>
		/// <returns>An item.</returns>
		public virtual STemplate CreateTemplate(string keyOrPath, string databaseName)
		{
			return new STemplate(_templateGateway.GetTemplate(keyOrPath, databaseName));
		}		
	}
}
