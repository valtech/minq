using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents the processing context for request-level operations in Sitecore via LINQ.
	/// </summary>
	public class SRequest
	{
		private SItemComposer _itemComposer;
		private ISitecoreRequest _itemRequest;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="container"></param>
		public SRequest(SItemComposer itemComposer, ISitecoreRequest sitecoreRequest)
		{
			_itemComposer = itemComposer;
			_itemRequest = sitecoreRequest;
		}

		/// <summary>
		/// Gets the Sitecore LINQ item for the current HTTP request.
		/// </summary>
		public SItem Item
		{
			get
			{
				SitecoreItemKey key = _itemRequest.ItemKey;

				return _itemComposer.CreateItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
			}
			set
			{
				_itemRequest.ItemKey = new SitecoreItemKey(value.Guid, value.LanguageName, value.Db.Name);
			}
		}
	}
}
