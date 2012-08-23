using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiContext = global::Sitecore.Context;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore
{
	/// <summary>
	/// Provides a Sitecore context based on the <see ref="ISitecoreContext" /> interface.
	/// </summary>
	public class SitecoreContext : ISitecoreContext
	{
		/// <summary>
		/// Gets or sets the NLS culture name for the Sitecore context.
		/// </summary>
		public string LanguageName
		{
			get
			{
				return ScapiContext.Language.Name;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Gets or sets the Sitecore database used as the default item repository for the Sitecore context.
		/// </summary>
		public string DatabaseName
		{
			get
			{
				return ScapiContext.Database.Name;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

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
		}
	}
}
