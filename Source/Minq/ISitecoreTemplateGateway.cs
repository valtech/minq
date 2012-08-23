using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents access to the Sitecore template repository.
	/// </summary>
	public interface ISitecoreTemplateGateway
	{
		/// <summary>
		/// When overridden in a derived class, returns the Sitecore template for the given <see cref="SitecoreTemplateKey" />.
		/// </summary>
		/// <param name="key">The <see cref="SitecoreTemplateKey" /> unqiuely identifying the template to return.</param>
		/// <returns>A <see cref="ISitecoreTemplate" />.</returns>
		ISitecoreTemplate GetTemplate(SitecoreTemplateKey key);
	}
}
