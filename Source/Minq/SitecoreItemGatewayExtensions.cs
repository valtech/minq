using System;

namespace Minq
{
	/// <summary>
	/// Provides a set of static methods for querying objects that implement <see cref="ISitecoreItemGateway" />.
	/// </summary>
	public static class SitecoreItemGatewayExtensions
	{
		/// <summary>
		/// Gets an item by key.
		/// </summary>
		/// <param name="source">A <see cref="ISitecoreItemGateway" /> to query.</param>
		/// <param name="key">The key of the item.</param>
		/// <returns>The item.</returns>
		public static ISitecoreItem GetItem(this ISitecoreItemGateway source, SitecoreItemKey key)
		{
			return source.GetItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
		}
	}
}
