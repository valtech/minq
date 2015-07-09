using System;
using System.Collections.Generic;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a Sitecore template.
	/// </summary>
	public interface ISitecoreTemplate
	{
		/// <summary>
		/// When overridden in a derived class, gets the <see cref="SitecoreTemplateKey" /> used to uniquely identify the template.
		/// </summary>
		SitecoreTemplateKey Key
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets all the base templates defined for this template.
		/// </summary>
		IEnumerable<ISitecoreTemplate> BaseTemplates
		{
			get;
		}
	}
}
