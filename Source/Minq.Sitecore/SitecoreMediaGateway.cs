using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Resources.Media;

namespace Minq.Sitecore
{
	public class SitecoreMediaGateway : ISitecoreMediaGateway
	{
		public ISitecoreMedia GetMedia(string keyOrPath, string languageName, string databaseName)
		{
			return new SitecoreMedia(SitecoreItemGateway.GetScapiItem(keyOrPath, languageName, databaseName, true));
		}
	}
}
