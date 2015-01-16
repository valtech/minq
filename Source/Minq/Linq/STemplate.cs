using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents a Sitecore LINQ template.
	/// </summary>
	public class STemplate
	{
		private ISitecoreTemplate _sitecoreTemplate;

		/// <summary>
		/// Initializes the class for use based on the <see cref="ISitecoreTemplate" />.
		/// </summary>
		/// <param name="sitecoreTemplate">The low level Sitecore template that represents this LINQ template.</param>
		public STemplate(ISitecoreTemplate sitecoreTemplate)
		{
			_sitecoreTemplate = sitecoreTemplate;
		}

		/// <summary>
		/// The GUID of the template.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _sitecoreTemplate.Key.Guid;
			}
		}

		/// <summary>
		/// Returns a collection of the base templates making up this template.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="STemplate"/> containing the base templates of this template.</returns>
		public IEnumerable<STemplate> BaseTemplates()
		{
			foreach (ISitecoreTemplate baseTemplate in _sitecoreTemplate.BaseTemplates)
			{
				yield return new STemplate(baseTemplate);
			}
		}

		/// <summary>
		/// Returns a collection of templates that contain this template, and all base templates of this template.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> containing this template, and base templates of this template.</returns>
		public IEnumerable<STemplate> BaseTemplatesAndSelf()
		{
			yield return this;

			foreach (STemplate baseTemplate in BaseTemplates())
			{
				yield return baseTemplate;
			}
		}

		/// <summary>
		/// Determines if the specified GUID is based on this template or one of its base templates.
		/// </summary>
		/// <param name="templateGuid">The template GUID to interogate.</param>
		/// <returns>true if the specified GUID represents this template or one of its base templates, false otherwise.</returns>
		public bool IsBasedOn(Guid templateGuid)
		{
			if (Guid == templateGuid)
			{
				return true;
			}

			return BaseTemplates().Any(template => template.Guid == templateGuid);
		}

		/// <summary>
		/// Determines if the specified POCO type is based on this template or one of its base templates.
		/// </summary>
		/// <typeparam name="TItem">The POCO type to interogate.</typeparam>
		/// <returns>true if the specified POCO type represents this template or one of its base templates, false otherwise.</returns>
		/// <remarks>
		/// The POCO type must have a <see cref="SitecoreTemplateAttribute" /> in order for this method to work.
		/// </remarks>
		public bool IsBasedOn<TItem>()
		{
			return IsBasedOn(SitecoreTemplateAttribute.FindTemplateGuid(typeof(TItem)));
		}

		public bool IsBasedOn(Type type)
		{
			return IsBasedOn(SitecoreTemplateAttribute.FindTemplateGuid(type));
		}
	}
}
