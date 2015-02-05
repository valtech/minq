using Minq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
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
