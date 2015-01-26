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
		/// <returns>A <see cref="ISitecoreTemplate" />.</returns>
		ISitecoreTemplate GetTemplate(string keyOrPath, string databaseName);
	}
}
