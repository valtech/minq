using System;
using System.Collections.Generic;

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

		public ISitecoreTemplate GetTemplate(string keyOrPath, string databaseName)
		{
			ISitecoreTemplate template;

			if (_templates.TryGetValue(new SitecoreTemplateKey(new Guid(keyOrPath), databaseName), out template))
			{
				return template;
			}
			else
			{
				throw new MockSitecoreItemGatewayException(String.Format("Sitecore template {0} does not exist", keyOrPath));
			}
		}
	}
}
