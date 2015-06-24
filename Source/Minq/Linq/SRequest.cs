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
		private ISitecoreRequest _sitecoreRequest;

		/// <summary>
		///  Initializes the class for use based on a <see cref="SItemComposer"/>.
		/// </summary>
		/// <param name="itemComposer">The item composer.</param>
		/// /// <param name="sitecoreRequest">The Sitecore request.</param>
		public SRequest(SItemComposer itemComposer, ISitecoreRequest sitecoreRequest)
		{
			_itemComposer = itemComposer;
			_sitecoreRequest = sitecoreRequest;
		}

		/// <summary>
		/// Gets the Sitecore LINQ item for the current HTTP request.
		/// </summary>
		public SItem Item
		{
			get
			{
				SitecoreItemKey key = _sitecoreRequest.ItemKey;

				return _itemComposer.CreateItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
			}
			set
			{
				_sitecoreRequest.ItemKey = new SitecoreItemKey(value.Guid, value.LanguageName, value.Db.Name);
			}
		}

		/// <summary>
		/// Gets the page mode of the current request, i.e. if the request is a user accessing the content
		/// deleivery site, or a CMS author using page editor.
		/// </summary>
		public ISitecorePageMode PageMode
		{
			get
			{
				return _sitecoreRequest.PageMode;
			}
		}

		/// <summary>
		/// Gets the name of the Sitecore site associated with this request (as labelled in the config).
		/// </summary>
		public string SiteName
		{
			get
			{
				return _sitecoreRequest.SiteName;
			}
		}
	}
}
