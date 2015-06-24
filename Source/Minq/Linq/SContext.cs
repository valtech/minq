using System;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents the processing context for request-level operations in Sitecore via LINQ.
	/// </summary>
	public class SContext
	{
		private ISitecoreContext _sitecoreContext;
		private SItemComposer _itemComposer;
		private SDb _db;

		public SContext(ISitecoreContext context, SItemComposer itemComposer)
		{
			_sitecoreContext = context;
			_itemComposer = itemComposer;
		}

		/// <summary>
		/// Gets the Sitecore LINQ database for the current site.
		/// </summary>
		public SDb Db
		{
			get
			{
				if (_db == null)
				{
					_db = new SDb(_sitecoreContext.DatabaseName, _itemComposer);
				}

				return _db;
			}
		}

		/// <summary>
		/// Gets an item from the default database
		/// </summary>
		/// <param name="guid">The GUID of the item to get.</param>
		/// <returns>The item.</returns>
		public SItem Item(Guid guid)
		{
			return Item(guid.ToString());
		}

		/// <summary>
		/// Gets an item from the default database
		/// </summary>
		/// <param name="keyOrPath">The key or path of the item to get.</param>
		/// <returns>The item.</returns>
		public SItem Item(string keyOrPath)
		{
			return _itemComposer.CreateItem(keyOrPath, _sitecoreContext.LanguageName, Db.Name);
		}

		/// <summary>
		/// Gets a media item from the default database.
		/// </summary>
		/// <param name="guid">The GUID of the media item to get.</param>
		/// <returns>The media.</returns>
		public SMedia Media(Guid guid)
		{
			return Media(guid.ToString());
		}

		/// <summary>
		/// Gets a media item from the default database.
		/// </summary>
		/// <param name="keyOrPath">The  key or path of the media item to get.</param>
		/// <returns>The media.</returns>
		public SMedia Media(string keyOrPath)
		{
			return _itemComposer.CreateMedia(keyOrPath, _sitecoreContext.LanguageName, Db.Name);
		}
	}
}
