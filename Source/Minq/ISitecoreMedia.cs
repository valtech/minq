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

		string Url
		{
			get;
		}
	}
}
