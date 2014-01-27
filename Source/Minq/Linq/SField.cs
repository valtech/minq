using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	///  Defines an object that represents a Sitecore LINQ field.
	/// </summary>
	public class SField
	{
		private ISitecoreField _field;
		private SItem _owner;

		/// <summary>
		/// Initializes the class for use based on the <see cref="ISitecoreField" />.
		/// </summary>
		/// <param name="field">The low level Sitecore field that represents this LINQ field.</param>
		public SField(ISitecoreField field, SItem owner)
		{
			_field = field;
			_owner = owner;
		}

		public static bool IsNullOrEmpty(SField field)
		{
			return field == null || String.IsNullOrEmpty(field.Value<string>());
		}

		/// <summary>
		/// Gets the value of the field as the specified type.
		/// </summary>
		/// <typeparam name="TType">The data type to convert the field to.</typeparam>
		/// <returns>The value of the field.</returns>
		public TType Value<TType>()
		{
			return (TType)Value(typeof(TType));
		}

		/// <summary>
		/// Gets the value of the field as the specified type, substituting a default value
		/// if the field cannot be converted the the given type.
		/// </summary>
		/// <typeparam name="TType">The data type to convert the field to.</typeparam>
		/// <param name="default">The default value to use if the field cannot be converted to the specified type.</param>
		/// <returns>The value of the field.</returns>
		public TType Value<TType>(TType @default)
		{
			return (TType)Value(typeof(TType), @default);
		}

		/// <summary>
		/// Gets the value of the field as the specified type.
		/// </summary>
		/// <param name="type">The data type to convert the field to.</param>
		/// <returns>The value of the field.</returns>
		public object Value(Type type)
		{
			if (type == typeof(string))
			{
				return Value(String.Empty);
			}

			return Value(type, null);
		}

		/// <summary>
		/// Gets the value of the field as the specified type, substituting a default value
		/// if the field cannot be converted the the given type.
		/// </summary>
		/// <param name="type">The data type to convert the field to.</param>
		/// <param name="default">The default value to use if the field cannot be converted to the specified type.</param>
		/// <returns>The value of the field.</returns>
		public object Value(Type type, object @default)
		{
			if (_field == null)
			{
				return @default;
			}

			if (type == typeof(SItem))
			{
				IEnumerable<Guid> guids;

				if (_field.TryConvertValue<IEnumerable<Guid>>(out guids))
				{
					return guids.Select(guid => _owner.Db.Item(guid, _owner.LanguageName)).ToArray();
				}

				if (guids.Any())
				{
					return _owner.Db.Item(guids.First(), _owner.LanguageName);
				}
			}
			else if (type == typeof(IEnumerable<SItem>) || type == typeof(SItem[]))
			{
				IEnumerable<Guid> guids;

				if (_field.TryConvertValue<IEnumerable<Guid>>(out guids))
				{
					return guids.Select(guid => _owner.Db.Item(guid, _owner.LanguageName)).ToArray();
				}
			}
			else if (type == typeof(SMedia))
			{
				if (SMedia.IsMediaField(this))
				{
					return _owner.Composer.CreateMedia(this, _owner.LanguageName, _owner.Db.Name);
				}
			}
			else if (type == typeof(SLink))
			{
				if (SLink.IsLinkField(this))
				{
					return new SLink(this);
				}
			}
			else if (type == typeof(IEnumerable<SMedia>) || type == typeof(SMedia[]))
			{
				IEnumerable<Guid> guids;

				if (_field.TryConvertValue<IEnumerable<Guid>>(out guids))
				{
					return guids.Select(guid => _owner.Composer.CreateMedia(guid.ToString(), _owner.LanguageName, _owner.Db.Name)).ToArray();
				}
			}

			object value;

			if (_field.TryConvertValue(type, out value))
			{
				return value;
			}

			return @default;
		}

		public bool IsEmpty
		{
			get
			{
				string value = Value<string>(null);

				return String.IsNullOrEmpty(value);
			}
		}

		public Type FieldType
		{
			get
			{
				if (_field == null)
				{
					return typeof(string);
				}

				return _field.Template.FieldType;
			}
		}

		public SItem Owner
		{
			get
			{
				return _owner;
			}
		}
	}
}
