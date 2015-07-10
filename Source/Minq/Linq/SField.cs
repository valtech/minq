using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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
		/// /// <param name="owner">The owner item of this field.</param>
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
			Type type = typeof(TType);

            if (type == typeof(string))
			{
				return (TType)Value(type, String.Empty);
			}

			return Value<TType>(default(TType));
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
					if (guids.Any())
					{
						return _owner.Db.Item(guids.First(), _owner.LanguageName);
					}
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
					return ToMedia();
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
			else if (type.IsGenericType)
			{
				Type genericType = type.GetGenericTypeDefinition();

				if (genericType == typeof(ICollection<>))
				{
					Type genericParameter = type.GetGenericArguments()[0];

					Type collectionType = typeof(LazyFieldCollection<>).MakeGenericType(new Type[] { genericParameter });

					object collection = Activator.CreateInstance(collectionType, new object[] { this });

					return collection;
				}
			}

			object value;

			if (_field.TryConvertValue(type, out value))
			{
				return value;
			}
			else if (type.IsDefined(typeof(SitecoreTemplateAttribute), false))
			{
				return Value<SItem>().ToType(type);
			}

			return @default;
		}
		
		/// <summary>
		/// Converts the field to a medai item.
		/// </summary>
		/// <returns>The media item.</returns>
		/// <remarks>
		/// Will throw an exception if the field is not a media field.
		/// </remarks>
		public SMedia ToMedia()
		{
			if (!SMedia.IsMediaField(this))
			{
				throw new Exception("Not a media field");
			}

			XElement element = XDocument.Parse(Value<string>()).Descendants("image").First();

			if (element != null)
			{
				XAttribute attribute = element.Attribute("mediaid");

				if (attribute != null && attribute.Value.Length > 0)
				{
					return _owner.Composer.CreateMedia(attribute.Value, _owner.LanguageName, _owner.Db.Name);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets whether this field is empty or not.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				string value = Value<string>(null);

				return String.IsNullOrEmpty(value);
			}
		}

		/// <summary>
		/// Gets the field type as a .NET data type.
		/// </summary>
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

		/// <summary>
		/// Gets the item that this field belongs to.
		/// </summary>
		public SItem Owner
		{
			get
			{
				return _owner;
			}
		}
	}
}
