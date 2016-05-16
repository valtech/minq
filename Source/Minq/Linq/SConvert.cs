using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.ComponentModel;

namespace Minq.Linq
{
	/// <summary>
	/// Converts Sitecore field data to another base data type.
	/// </summary>
	public static class SConvert
	{
		/// <summary>
		/// Converts the value of the specified string to an equivalent <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="default">The default value if the conversion fails.</param>
		/// <returns>The converted <see cref="Boolean"/> value.</returns>
		public static bool ToBoolean(string value, bool @default)
		{
			bool result;

			if (TryBoolean(value, out result))
			{
				return result;
			}

			return @default;
		}

		/// <summary>
		/// Tries to converts the value of the specified string to an equivalent <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="result">The converted value.</param>
		/// <returns>true if the value could be converted, false otherwise.</returns>
		public static bool TryBoolean(string value, out bool result)
		{
			result = String.Equals(value, "1", StringComparison.OrdinalIgnoreCase) || String.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

			return true;
		}

		/// <summary>
		/// Converts the value of the specified string to an equivalent <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="default">The default value if the conversion fails.</param>
		/// <returns>The converted <see cref="DateTime"/> value.</returns>
		public static DateTime ToDateTime(string value, DateTime @default)
		{
			DateTime result;

			if (TryDateTime(value, out result))
			{
				return result;
			}

			return @default;
		}

		/// <summary>
		/// Tries to converts the value of the specified string to an equivalent <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="result">The converted value.</param>
		/// <returns>true if the value could be converted, false otherwise.</returns>
		public static bool TryDateTime(string value, out DateTime result)
		{
			if (String.IsNullOrEmpty(value))
			{
				result = DateTime.MaxValue;

				return false;
			}

			string lowerValue = value.ToLowerInvariant();

			if (lowerValue == "today" || lowerValue == "now")
			{
				result = DateTime.Now;

				return true;
			}
			else if (lowerValue == "tomorrow")
			{
				result = DateTime.Now + TimeSpan.FromDays(1);

				return true;
			}
			else if (lowerValue == "yesterday")
			{
				result = DateTime.Now - TimeSpan.FromDays(1);

				return true;
			}

			if (TryParseRFC1123DateTime(value, out result))
			{
				return true;
			}

			//.NET
			if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return true;
			}

			if (value.EndsWith("Z"))
			{
				if (TryParseRFC1123DateTime(value.TrimEnd('Z'), out result))
				{
					result = new DateTime(result.Ticks, DateTimeKind.Utc);

					return true;
				}
			}

			int colon = value.IndexOf(':');

			if (colon != -1)
			{
				if (TryParseRFC1123DateTime(value.Substring(0, colon), out result))
				{
					return true;
				}
			}

			return !String.IsNullOrEmpty(value);
		}

		private static bool TryParseRFC1123DateTime(string value, out DateTime result)
		{
			return DateTime.TryParseExact(value, "yyyyMMddTHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

		/// <summary>
		/// Converts the value of the specified string to an equivalent <see cref="Guid"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <returns>The converted <see cref="Guid"/> value.</returns>
		public static Guid ToGuid(string value)
		{
			Guid result;

			if (TryGuid(value, out result))
			{
				return result;
			}

			return Guid.Empty;
		}

		/// <summary>
		/// Tries to converts the value of the specified string to an equivalent <see cref="Guid"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="result">The converted value.</param>
		/// <returns>true if the value could be converted, false otherwise.</returns>
		public static bool TryGuid(string value, out Guid result)
		{
			if (String.IsNullOrEmpty(value))
			{
				result = Guid.Empty;

				return false;
			}
			else
			{
				result = new Guid(value);

				return true;
			}
		}

		private static bool TryDouble(string value, out double result)
		{
			return Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryFloat(string value, out float result)
		{
			return Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryDecimal(string value, out decimal result)
		{
			return Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryNullableDouble(string value, out double? result)
		{
			result = null;

			if (String.IsNullOrEmpty(value))
			{
				return true;
			}
			else
			{
				double number = 0;

				if (TryDouble(value, out number))
				{
					result = number;

					return true;
				}
			}

			return false;
		}

		private static bool TryNullableFloat(string value, out float? result)
		{
			result = null;

			if (String.IsNullOrEmpty(value))
			{
				return true;
			}
			else
			{
				float number = 0;

				if (TryFloat(value, out number))
				{
					result = number;

					return true;
				}
			}

			return false;
		}

		private static bool TryNullableDecimal(string value, out decimal? result)
		{
			result = null;

			if (String.IsNullOrEmpty(value))
			{
				return true;
			}
			else
			{
				decimal number = 0;

				if (TryDecimal(value, out number))
				{
					result = number;

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns an object of the specified type and whose value is equivalent to the field data input.
		/// </summary>
		/// <param name="type">The type to convert the field data to.</param>
		/// <param name="input">The field data input to convert.</param>
		/// <param name="output">The converted output.</param>
		/// <returns>true if the value could be converted, false otherwise.</returns>
		public static bool TryChangeType(Type type, string input, out object output)
		{
			output = null;

			if (type == typeof(bool))
			{
				bool value;

				if (TryBoolean(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(double))
			{
				double value;

				if (TryDouble(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(decimal))
			{
				decimal value;

				if (TryDecimal(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(float))
			{
				float value;

				if (TryFloat(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(double?))
			{
				double? value;

				if (TryNullableDouble(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(decimal?))
			{
				decimal? value;

				if (TryNullableDecimal(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(float?))
			{
				float? value;

				if (TryNullableFloat(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(DateTime))
			{
				DateTime value;

				if (TryDateTime(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(DateTime?))
			{
				DateTime value;

				if (TryDateTime(input, out value))
				{
					output = value;

					return true;
				}
				else if (String.IsNullOrEmpty(input))
				{
					output = null;

					return true;
				}
			}
			else if (type == typeof(string))
			{
				output = input;

				return true;
			}
			else if (type == typeof(Guid))
			{
				Guid value;

				if (TryGuid(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(IEnumerable<Guid>))
			{
				if (!String.IsNullOrEmpty(input))
				{
					string[] parts = input.Split('|');

					IList<Guid> guids = new List<Guid>();

					foreach (string part in parts)
					{
						Guid guid;

						if (Guid.TryParse(part, out guid))
						{
							guids.Add(guid);
						}
						else
						{
							return false;
						}
					}

					output = guids;

					return true;
				}

				output = Enumerable.Empty<Guid>();

				return true;
			}
			else
			{
				if (TryTypeConverter(type, input, out output, CultureInfo.InvariantCulture))
				{
					return true;
				}
			}

			return false;
		}

		private static bool TryTypeConverter(Type type, string input, out object value, CultureInfo culture)
		{
			try
			{
				TypeConverter converter = TypeDescriptor.GetConverter(type);

				value = converter.ConvertTo(null, culture, input, type);

				return true;
			}
			catch
			{
				try
				{
					value = Convert.ChangeType(input, type);

					return true;
				}
				catch
				{
					value = null;

					return false;
				}
			}
		}
	}
}
