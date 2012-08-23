using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Sitecore
{
	public class SitecoreTemplateGatewayException : Exception
	{
		public SitecoreTemplateGatewayException(string message)
			: base(message)
		{
		}
	}
}
