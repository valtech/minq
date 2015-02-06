using System.Collections.Generic;
using System.Linq;


namespace Minq.Linq
{
	public static class SItemExtensions
	{
		public static IEnumerable<T> ToType<T>(this IEnumerable<SItem> source)
			where T : class, new()
		{
			return source.Select(item => item.ToType<T>());
		}
	}
}
