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
		/// <summary>
		/// Gets an item from the Sitecore CMS repository.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the item to return.</param>
		/// <param name="languageName">The language of the item to return.</param>
		/// <param name="databaseName">Thedatabse of the item to return.</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public ISitecoreItem GetItem(string keyOrPath, string languageName, string databaseName)
		{
			ScapiItem scapiItem = GetScapiItem(keyOrPath, languageName, databaseName, false);

			if (IsValidItem(scapiItem))
			{
				return new SitecoreItem(scapiItem);
			}

			return null;

			//throw new SitecoreItemGatewayException(String.Format("No version of the Sitecore item {0}/{1}/{2} exists", keyOrPath, languageName, databaseName));
		}

		/// <summary>
		/// Gets an item from the Sitecore CMS repository.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the item to return.</param>
		/// <param name="languageName">The language of the item to return.</param>
		/// <param name="databaseName">Thedatabse of the item to return.</param>
		/// <param name="throwErrorIfNotFound">true to throw an exception of the item is not found; false otherwise</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public static ScapiItem GetScapiItem(string keyOrPath, string languageName, string databaseName, bool throwErrorIfNotFound)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(databaseName);

			ScapiLanguage scapiLanguage;

			if (ScapiLanguage.TryParse(languageName, out scapiLanguage))
			{
				ScapiItem scapiItem = null;

				Guid guid;

				if (Guid.TryParse(keyOrPath, out guid))
				{
					scapiItem = ScapiItemManager.Provider.GetItem(new ScapiId(guid), scapiLanguage, ScapiVersion.Latest, scapiDatabase, ScapiSecurityCheck.Enable);
				}
				else
				{
					scapiItem = ScapiItemManager.Provider.GetItem(keyOrPath, scapiLanguage, ScapiVersion.Latest, scapiDatabase, ScapiSecurityCheck.Enable);
				}

				if (IsValidItem(scapiItem))
				{
					return scapiItem;
				}
			}

			if (throwErrorIfNotFound)
			{
				throw new SitecoreItemGatewayException(String.Format("Sitecore item {0}/{1}/{2} was not found", keyOrPath, languageName, databaseName));
			}

			return null;
		}

		private static bool IsValidItem(ScapiItem scapiItem)
		{
			return scapiItem != null; /*&& scapiItem.Version.Number > 0 && scapiItem.Versions.Count > 0*/
		}

		public static ScapiItem GetScapiItem(SitecoreItemKey key, bool throwErrorIfNotFound)
		{
			return GetScapiItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName, throwErrorIfNotFound);
		}
	}
}
