using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiItem = global::Sitecore.Data.Items.Item;
using ScapiDatabase = global::Sitecore.Data.Database;
using ScapiLanguage = global::Sitecore.Globalization.Language;
using ScapiFactory = global::Sitecore.Configuration.Factory;
using ScapiId = global::Sitecore.Data.ID;
using ScapiItemManager = global::Sitecore.Data.Managers.ItemManager;
using ScapiVersion = global::Sitecore.Data.Version;
using ScapiSecurityCheck = global::Sitecore.SecurityModel.SecurityCheck;

namespace Minq.Sitecore
{
	/// <summary>
	/// Provides a Sitecore version of the <see ref="ISitecoreItemGateway" /> interface.
	/// </summary>
	public class SitecoreItemGateway : ISitecoreItemGateway
	{
		public ISitecoreItem GetItem(SitecoreItemKey key)
		{
			ScapiItem scapiItem = GetScapiItem(key, true);

			if (scapiItem != null && scapiItem.Version.Number > 0 && scapiItem.Versions.Count > 0)
			{
				return new SitecoreItem(scapiItem);
			}

			throw new SitecoreItemGatewayException(String.Format("No version of the Sitecore item {0} exists", key));
		}

		/// <summary>
		/// Gets an item from the Sitecore CMS repository.
		/// </summary>
		/// <param name="key">The <see cref="SitecoreItemKey" /> unqiuely identifying the item to return.</param>
		/// <param name="throwErrorIfNotFound">true to throw an exception of the item is not found; false otherwise</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public static ScapiItem GetScapiItem(SitecoreItemKey key, bool throwErrorIfNotFound)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(key.DatabaseName);

			ScapiLanguage scapiLanguage;

			if (ScapiLanguage.TryParse(key.LanguageName, out scapiLanguage))
			{
				ScapiItem scapiItem = ScapiItemManager.Provider.GetItem(new ScapiId(key.Guid), scapiLanguage, ScapiVersion.Latest, scapiDatabase, ScapiSecurityCheck.Enable);

				if (scapiItem != null && scapiItem.Version.Number > 0 && scapiItem.Versions.Count > 0)
				{
					return scapiItem;
				}
			}

			if (throwErrorIfNotFound)
			{
				throw new SitecoreItemGatewayException(String.Format("Sitecore item {0} was not found", key));
			}

			return null;
		}
	}
}
