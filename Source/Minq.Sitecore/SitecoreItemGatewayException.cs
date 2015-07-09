using System;

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
