using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mvc
{
	/// <summary>
	/// Represents the HTML markup for a field.
	/// </summary>
	public interface ISitecoreFieldMarkup
	{
		/// <summary>
		/// Gets the HTML markup for a field, wrapping whatever child content is passed to it.
		/// </summary>
		/// <param name="childContent">The child content to wrap, or null/empty for no child content</param>
		/// <returns>The HTML markup for the field.</returns>
		string GetHtml(string childContent);
	}
}
