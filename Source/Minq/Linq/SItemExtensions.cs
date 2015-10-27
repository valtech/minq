using System.Collections.Generic;
using System.Linq;


namespace Minq.Linq
{
	/// <summary>
	/// Provides a set of static methods for querying and manipulating collections of Sitecore items.
	/// </summary>
	public static class SItemExtensions
	{
		/// <summary>
		/// Converts an IEnumerable of item objects to an IEnumerbale of the supplied POCO type.
		/// </summary>
		/// <typeparam name="T">The POCO type to convert the item objects to.</typeparam>
		/// <param name="source">The source of item objects.</param>
		/// <returns>An IEnumerable of POCO objects.</returns>
		public static IEnumerable<T> ToType<T>(this IEnumerable<SItem> source)
			where T : class, new()
		{
			return source.Select(item => item.ToType<T>());
		}
	}
}
