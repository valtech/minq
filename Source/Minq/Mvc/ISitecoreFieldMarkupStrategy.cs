using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Minq.Mvc
{
	/// <summary>
	/// Represents the HTML markup strategy for Sitecore fields.
	/// </summary>
	/// <remarks>
	/// This interface abstracts away the Sitecore rendering pipeline in order to test the HTML generation
	/// in the MVC library's Sitecore helper.
	/// </remarks>
	public interface ISitecoreFieldMarkupStrategy
	{
		/// <summary>
		/// Get the field markup for a given field.
		/// </summary>
		/// <param name="fieldMetadata">Metadata for the Sitecore field.</param>
		/// <param name="fieldAttributes">Additional HTML/Sitecore attributes.</param>
		/// <returns>A <see cref="ISitecoreFieldMarkup"/> representing the markup generator for the field.</returns>
		ISitecoreFieldMarkup GetFieldMarkup(SitecoreFieldMetadata fieldMetadata, SitecoreFieldAttributeDictionary fieldAttributes);
	}
}
