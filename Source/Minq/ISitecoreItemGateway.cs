using System;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents access to the Sitecore item repository.
	/// </summary>
	public interface ISitecoreItemGateway
	{
		/// <summary>
		/// When overridden in a derived class, returns the Sitecore item for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the item to return.</param>
		/// <param name="languageName">The language of the item to return.</param>
		/// <param name="databaseName">Thedatabase of the item to return.</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		ISitecoreItem GetItem(string keyOrPath, string languageName, string databaseName);
	}
}
