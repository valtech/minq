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
		private delegate bool TypeParser<T>(string value, out T result);

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

			if (type == typeof(string))
			{
				output = input;

				return true;
			}
			else if (TryStruct<double>(type, input, out output, TryDouble))
			{
				return true;
			}
			else if (TryStruct<float>(type, input, out output, TrySingle))
			{
				return true;
			}
			else if (TryStruct<int>(type, input, out output, TryInt32))
			{
				return true;
			}
			else if (TryStruct<long>(type, input, out output, TryInt64))
			{
				return true;
			}
			else if (TryStruct<decimal>(type, input, out output, TryDecimal))
			{
				return true;
			}
			else if (TryStruct<bool>(type, input, out output, TryBoolean))
			{
				return true;
			}
			else if (TryStruct<DateTime>(type, input, out output, TryDateTime))
			{
				return true;
			}
			else if (TryStruct<Guid>(type, input, out output, TryGuid))
			{
				return true;
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

		private static bool TryInt32(string value, out int result)
		{
			return Int32.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryInt64(string value, out long result)
		{
			return Int64.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryDouble(string value, out double result)
		{
			return Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TrySingle(string value, out float result)
		{
			return Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryDecimal(string value, out decimal result)
		{
			return Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		private static bool TryNullable<T>(string value, out T? result, TypeParser<T> parser) where T : struct
		{
			result = null;

			if (String.IsNullOrEmpty(value))
			{
				return true;
			}
			else
			{
				T number = default(T);

				if (parser(value, out number))
				{
					result = number;

					return true;
				}
			}

			return false;
		}

		private static bool TryStruct<T>(Type type, string input, out object output, TypeParser<T> parser) where T : struct
		{
			if (type == typeof(T))
			{
				T value;

				if (parser(input, out value))
				{
					output = value;

					return true;
				}
			}
			else if (type == typeof(T?))
			{
				T? value;

				if (TryNullable<T>(input, out value, parser))
				{
					output = value;

					return true;
				}
			}

			output = null;

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
