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
		public ISitecoreTemplate GetTemplate(SitecoreTemplateKey key)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(key.DatabaseName);

			ScapiTemplate template = ScapiTemplateManager.GetTemplate(new ScapiId(key.Guid), scapiDatabase);

			return new SitecoreTemplate(template, key.DatabaseName);
		}

		public static ScapiTemplate GetScapiTemplate(SitecoreTemplateKey key, bool throwErrorIfNotFound)
		{
			ScapiDatabase scapiDatabase = ScapiFactory.GetDatabase(key.DatabaseName);

			ScapiTemplate template = ScapiTemplateManager.GetTemplate(new ScapiId(key.Guid), scapiDatabase);

			if (template != null)
			{
				return template;
			}

			if (throwErrorIfNotFound)
			{
				throw new SitecoreTemplateGatewayException(String.Format("Sitecore template {0} was not found", key));
			}

			return null;
		}
	}
}