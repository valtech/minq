using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		/// <summary>
		/// Initializes the class for use based on a <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="container"></param>
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

		public SItem Item(Guid guid)
		{
			return Item(guid.ToString());
		}

		public SItem Item(string keyOrPath)
		{
			return _itemComposer.CreateItem(keyOrPath, _sitecoreContext.LanguageName, Db.Name);
		}

		public SMedia Media(Guid guid)
		{
			return Media(guid.ToString());
		}

		public SMedia Media(string keyOrPath)
		{
			return _itemComposer.CreateMedia(keyOrPath, _sitecoreContext.LanguageName, Db.Name);
		}
	}
}
