using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Minq.Mvc
{
	/// <summary>
	/// Defines an object that represents a field attribute parser.
	/// </summary>
	public static class SitecoreFieldAttributeParser
	{
		/// <summary>
		/// Converts a CSS size string to an <see cref="Int32"/>.
		/// </summary>
		/// <param name="size">The CSS size string to convert.</param>
		/// <returns>The CSS size as an <see cref="Int32"/>.</returns>
		/// <remarks>
		/// Assumes pixels are the unit.
		/// </remarks>
		public static int ConvertCssSizeUnitToInt32(string size)
		{
			size = Regex.Replace(size, @"[^\d]+$", "");

			return Int32.Parse(size, NumberStyles.Integer, CultureInfo.InvariantCulture);
		}
	}
}
