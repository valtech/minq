using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiContext = global::Sitecore.Context;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore
{
	public class SitecoreRequest : SitecoreContext, ISitecoreRequest
	{
		private ISitecorePageMode _pageMode;

		/// <summary>
		/// Gets the Sitecore item key associated with the current HTTP request.
		/// </summary>
		public SitecoreItemKey ItemKey
		{
			get
			{
				ScapiItem item = ScapiContext.Item;

				return new SitecoreItemKey(item.ID.Guid, item.Language.Name, item.Database.Name);
			}
			set
			{
				ScapiContext.Item = SitecoreItemGateway.GetScapiItem(value, true);
			}
		}

		public ISitecorePageMode PageMode
		{
			get
			{
				if (_pageMode == null)
				{
					_pageMode = new SitecorePageMode();
				}

				return _pageMode;
			}
		}
	}
}
