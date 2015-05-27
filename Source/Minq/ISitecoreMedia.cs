using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	public interface ISitecoreMedia
	{
		SitecoreItemKey Key
		{
			get;
		}

		/// <summary>
		/// The URL that could be used in a browser to request this media item.
		/// </summary>
		string Url
		{
			get;
		}

		string AlternateText
		{
			get;
		}
	}
}
