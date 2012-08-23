using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

			if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
			{
				return true;
			}

			return !String.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Converts the value of the specified string to an equivalent <see cref="Guid"/> value.
		/// </summary>
		/// <param name="value">The field data to convert.</param>
		/// <param name="default">The default value if the conversion fails.</param>
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

			if (type == typeof(bool))
			{
				bool value;

				if (TryBoolean(input, out value))
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
			else
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(string));

				if (converter.CanConvertTo(type))
				{
					output = converter.ConvertTo(input, type);

					return true;
				}
			}

			return false;
		}
	}
}
