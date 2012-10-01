using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiTemplateField = global::Sitecore.Data.Templates.TemplateField;

namespace Minq.Sitecore
{
	public class SitecoreFieldTemplate : ISitecoreFieldTemplate
	{
		private ScapiTemplateField _scapiTemplateField;

		public SitecoreFieldTemplate(ScapiTemplateField scapiTemplateField)
		{
			_scapiTemplateField = scapiTemplateField;
		}

		public string DefaultValue
		{
			get
			{
				return _scapiTemplateField.DefaultValue;
			}
		}

		public Type FieldType
		{
			get
			{
				switch (_scapiTemplateField.TypeKey)
				{
					case "checkbox":
						return typeof(bool);
					case "tristate":
						return typeof(bool?);
					case "datetime":
						return typeof(DateTime);
					case "date":
						return typeof(DateTime);
				}

				return typeof(string);
			}
		}
	}
}
