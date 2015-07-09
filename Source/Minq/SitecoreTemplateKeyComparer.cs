using System;
using System.Collections.Generic;

namespace Minq
{
	/// <summary>
	/// Represents a <see cref="SitecoreTemplateKey"/> comparison operation.
	/// </summary>
	public class SitecoreTemplateKeyComparer : IEqualityComparer<SitecoreTemplateKey>
	{
		/// <summary>
		/// Indicates whether <see cref="SitecoreItemKey"/> objects are equal.
		/// </summary>
		/// <param name="x">An object to compare to y.</param>
		/// <param name="y">An object to compare to x.</param>
		/// <returns></returns>
		public bool Equals(SitecoreTemplateKey x, SitecoreTemplateKey y)
		{
			return x == y;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public int GetHashCode(SitecoreTemplateKey obj)
		{
			return obj.GetHashCode();
		}
	}
}
