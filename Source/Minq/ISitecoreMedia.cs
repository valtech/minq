using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a Sitecore media item.
	/// </summary>
	public interface ISitecoreMedia
	{
		/// <summary>
		/// When overridden in a derived class, gets the <see cref="SitecoreItemKey" /> used to uniquely identify the media item.
		/// </summary>
		SitecoreItemKey Key
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets the URL that could be used in a browser to request this media item.
		/// </summary>
		string Url
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets the alternate (alt) text of the media when rendered as HTML.
		/// </summary>
		string AlternateText
		{
			get;
		}
	}
}
