using System;
using System.Collections.Generic;

namespace Minq.Mocks
{
	public class MockSitecoreTemplate : ISitecoreTemplate
	{
		private SitecoreTemplateKey _key;
		private IList<ISitecoreTemplate> _baseTemplates = new List<ISitecoreTemplate>();

		public MockSitecoreTemplate(SitecoreTemplateKey key)
		{
			_key = key;
		}

		public SitecoreTemplateKey Key
		{
			get
			{
				return _key;
			}
		}

		public IEnumerable<ISitecoreTemplate> BaseTemplates
		{
			get
			{
				return _baseTemplates;
			}
		}

		/// <summary>
		/// Adds a base template to this mock Sitecore template.
		/// </summary>
		/// <param name="baseTemplate">The child item to add.</param>
		public void AddBaseTemplate(MockSitecoreTemplate baseTemplate)
		{
			_baseTemplates.Add(baseTemplate);
		}
	}
}
