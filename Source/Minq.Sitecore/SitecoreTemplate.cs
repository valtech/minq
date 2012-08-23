using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiTemplate = Sitecore.Data.Templates.Template;

namespace Minq.Sitecore
{
	public class SitecoreTemplate : ISitecoreTemplate
	{
		private ScapiTemplate _scapiTemplate;
		private string _databaseName;

		public SitecoreTemplate(ScapiTemplate scapiTemplate, string databaseName)
		{
			_scapiTemplate = scapiTemplate;
			_databaseName = databaseName;
		}

		public SitecoreTemplateKey Key
		{
			get
			{
				return new SitecoreTemplateKey(_scapiTemplate.ID.Guid, _databaseName);
			}
		}

		public IEnumerable<ISitecoreTemplate> BaseTemplates
		{
			get
			{
				return EnumerateBaseTemplates(_scapiTemplate).Select(scapiTemplate => new SitecoreTemplate(scapiTemplate, _databaseName));
			}
		}

		private IEnumerable<ScapiTemplate> EnumerateBaseTemplates(ScapiTemplate owner)
		{
			ScapiTemplate[] templates = owner.BaseIDs
				.Select(id => SitecoreTemplateGateway.GetScapiTemplate(new SitecoreTemplateKey(id.Guid, _databaseName), true))
				.ToArray();

			return templates.Union(templates.Where(t => t.ID != owner.ID).SelectMany(t => EnumerateBaseTemplates(t)));
		}
	}
}
