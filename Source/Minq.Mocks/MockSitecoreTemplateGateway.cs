using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	public class MockSitecoreTemplateGateway : ISitecoreTemplateGateway
	{
		private IDictionary<SitecoreTemplateKey, ISitecoreTemplate> _templates = new Dictionary<SitecoreTemplateKey, ISitecoreTemplate>(new SitecoreTemplateKeyComparer());

		/// <summary>
		/// Adds an item to this mock Sitecore gateway's internal repository.
		/// </summary>
		/// <param name="template">The child item to add.</param>
		public void AddTemplate(ISitecoreTemplate template)
		{
			_templates[template.Key] = template;
		}

		public ISitecoreTemplate GetTemplate(SitecoreTemplateKey key)
		{
			ISitecoreTemplate template;

			if (_templates.TryGetValue(key, out template))
			{
				return template;
			}
			else
			{
				throw new MockSitecoreItemGatewayException(String.Format("Sitecore template {0} does not exist", key));
			}
		}
	}
}
