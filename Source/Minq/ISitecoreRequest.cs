using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an interface that represents the low level processing context for HTTP requests for Sitecore.
	/// </summary>
	public interface ISitecoreRequest : ISitecoreContext
	{
		/// <summary>
		/// When overridden in a derived class, gets or sets the Sitecore item key associated with the current HTTP request.
		/// </summary>
		SitecoreItemKey ItemKey
		{
			get;
			set;
		}

		ISitecorePageMode PageMode
		{
			get;
		}
	}
}
