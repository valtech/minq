using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an interface that represents the low level processing context for request-level operations in Sitecore.
	/// </summary>
	public interface ISitecoreContext
	{
		/// <summary>
		/// When overridden in a derived class, gets or sets the NLS culture name for the current HTTP request.
		/// </summary>
		string LanguageName
		{
			get;
			set;
		}

		/// <summary>
		/// When overridden in a derived class, gets or sets the Sitecore database used as the default item repository for the current HTTP request.
		/// </summary>
		string DatabaseName
		{
			get;
			set;
		}

		/// <summary>
		/// When overridden in a derived class, gets the Sitecore item key associated with the current HTTP request.
		/// </summary>
		SitecoreItemKey ItemKey
		{
			get;
		}
	}
}
