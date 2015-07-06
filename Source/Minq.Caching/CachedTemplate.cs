using Minq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Minq.Caching
{
	/// <summary>
	/// Defines an object that represents a cached Sitecore LINQ media item.
	/// </summary>
	class CachedTemplate : STemplate
	{
		private IReadOnlyList<STemplate> _baseTemplates;
		private IReadOnlyDictionary<Guid, bool> _baseTemplateDictionary;
		private CachedItemComposer _itemComposer;
		private ISitecoreTemplate _sitecoreTemplate;

		public CachedTemplate(ISitecoreTemplate sitecoreTemplate, CachedItemComposer itemComposer)
			: base(sitecoreTemplate)
		{
			_sitecoreTemplate = sitecoreTemplate;
			_itemComposer = itemComposer;
		}

		/// <summary>
		/// Gets the base templates that this template inherits from.
		/// </summary>
		/// <returns>An enumerable of <see cref="STemplate" /> objects.</returns>
		public override IEnumerable<STemplate> BaseTemplates()
		{
			if (_baseTemplates == null)
			{
				_baseTemplates = _sitecoreTemplate.BaseTemplates
					.Select(template => _itemComposer.GetOrAdd(template))
					.ToList();
            }

			return _baseTemplates;
		}

		/// <summary>
		/// Determines if this template is based on the template with the given GUID.
		/// </summary>
		/// <param name="templateGuid">The GUID of the inherited template to test.</param>
		/// <returns>true if this template is based on the supplied template GUID; false otherwise.</returns>
		public override bool IsBasedOn(Guid templateGuid)
		{
			if (_baseTemplateDictionary == null)
			{
				_baseTemplateDictionary = CreateBaseTemplateDictionary();
            }

			return _baseTemplateDictionary.ContainsKey(templateGuid);
		}

		private IReadOnlyDictionary<Guid, bool> CreateBaseTemplateDictionary()
		{
			Dictionary<Guid, bool> dictionary = new Dictionary<Guid, bool>();

			foreach (Guid guid in BaseTemplatesAndSelf().Select(template => template.Guid))
			{
				dictionary[guid] = true;
			}

			return dictionary;
		}
    }
}
