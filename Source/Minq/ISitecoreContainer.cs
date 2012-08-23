using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Represents a container for all the Sitecore engine's functions for the current HTTP request.
	/// </summary>
	/// <remarks>
	/// This class can be backed by any ID/IoC container.
	/// Their are default implementations that support mocking and the standard Sitecore CMS implementation.
	/// </remarks>
	public interface ISitecoreContainer
	{
		/// <summary>
		/// Retrieves a component that represents an implementation of Sitecore engine specific functionality.
		/// </summary>
		/// <typeparam name="TType">The component to retrieve.</typeparam>
		/// <returns>The component.</returns>
		TType Resolve<TType>();
	}
}
