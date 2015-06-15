using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an interface that represents the low level processing context for operations against the Sitecore repository.
	/// </summary>
	public interface ISitecoreContext
	{
		/// <summary>
		/// When overridden in a derived class, gets or sets the NLS culture name for the current HTTP request.
		/// </summary>
		string LanguageName
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets or sets the Sitecore database used as the default item repository for the current HTTP request.
		/// </summary>
		string DatabaseName
		{
			get;
		}
	}
}
