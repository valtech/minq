using System;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore
{
	public class SitecoreMediaGateway : ISitecoreMediaGateway
	{
		/// <summary>
		/// When overridden in a derived class, returns the Sitecore media item for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the media to return.</param>
		/// <param name="languageName">The language of the media to return.</param>
		/// <param name="databaseName">Thedatabase of the media to return.</param>
		/// <returns>A <see cref="ISitecoreMedia" />.</returns>
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
