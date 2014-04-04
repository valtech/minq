using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Resources.Media;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore
{
	public class SitecoreMediaGateway : ISitecoreMediaGateway
	{
		public ISitecoreMedia GetMedia(string keyOrPath, string languageName, string databaseName)
		{
			ScapiItem item = SitecoreItemGateway.GetScapiItem(keyOrPath, languageName, databaseName, false);

			if (item != null)
			{
				return new SitecoreMedia(item);
			}

			return null;
		}
	}
}
