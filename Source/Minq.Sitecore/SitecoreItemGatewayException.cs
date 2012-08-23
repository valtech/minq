using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Sitecore
{
	public class SitecoreItemGatewayException : Exception
	{
		public SitecoreItemGatewayException(string message)
			: base(message)
		{
		}
	}
}
