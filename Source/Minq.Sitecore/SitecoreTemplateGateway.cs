using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiDatabase = global::Sitecore.Data.Database;
using ScapiFactory = global::Sitecore.Configuration.Factory;
using ScapiTemplateManager = Sitecore.Data.Managers.TemplateManager;
using ScapiId = global::Sitecore.Data.ID;
using ScapiTemplate = Sitecore.Data.Templates.Template;

namespace Minq.Sitecore
{
	public class SitecoreTemplateGateway : ISitecoreTemplateGateway
	{
		public ISitecoreTemplate GetTemplate(string keyOrPath, string databaseName)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(databaseName);

			ScapiTemplate template = ScapiTemplateManager.GetTemplate(new ScapiId(keyOrPath), scapiDatabase);

			return new SitecoreTemplate(template, databaseName);
		}

		public static ScapiTemplate GetScapiTemplate(string keyOrPath, string databaseName, bool throwErrorIfNotFound)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(databaseName);

			ScapiTemplate template = ScapiTemplateManager.GetTemplate(new ScapiId(keyOrPath), scapiDatabase);

			if (template != null)
			{
				return template;
			}

			if (throwErrorIfNotFound)
			{
				throw new SitecoreTemplateGatewayException(String.Format("Sitecore template {0} was not found", keyOrPath));
			}

			return null;
		}
	}
}