using System;
using System.Collections.Generic;

namespace Minq
{
	/// <summary>
	/// Represents a <see cref="SitecoreItemKey"/> comparison operation.
	/// </summary>
	public class SitecoreItemKeyComparer : IEqualityComparer<SitecoreItemKey>
	{
		/// <summary>
		/// Indicates whether <see cref="SitecoreItemKey"/> objects are equal.
		/// </summary>
		/// <param name="x">An object to compare to y.</param>
		/// <param name="y">An object to compare to x.</param>
		/// <returns></returns>
		public bool Equals(SitecoreItemKey x, SitecoreItemKey y)
		{
			return x == y;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public int GetHashCode(SitecoreItemKey obj)
		{
			return obj.GetHashCode();
		}
	}
}
