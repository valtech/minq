using System;
using System.Linq;
using System.Reflection;

namespace Minq.Linq
{
	/// <summary>
	/// Represents Sitecore media library content.
	/// </summary>
	public class SMedia
	{
		private ISitecoreMedia _sitecoreMedia;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreItem"/>.
		/// </summary>
		/// <param name="sitecoreMedia">The low level Sitecore media item that represents this LINQ media item.</param>
		public SMedia(ISitecoreMedia sitecoreMedia)
		{
			_sitecoreMedia = sitecoreMedia;
		}

		/// <summary>
		/// Determines if the string value of a field is encoded as raw value media data.
		/// </summary>
		/// <param name="value">The raw value media data.</param>
		/// <returns>true if the raw value is media data; false otherwise.</returns>
		public static bool IsMediaField(string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				if (value.StartsWith("<image ") && value.EndsWith(">"))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if the field is a media field.
		/// </summary>
		/// <param name="field">The field to check.</param>
		/// <returns>true if the field is a media field; false otherwise.</returns>
		public static bool IsMediaField(SField field)
		{
			if (!field.IsEmpty)
			{
				string value = field.Value<string>();

				return IsMediaField(value);
			}

			return false;
		}

		/// <summary>
		/// Gets the Sitecore ID associated with this media.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _sitecoreMedia.Key.Guid;
			}
		}

		/// <summary>
		/// Gets the URL of the media as it should be displayed in the browser.
		/// </summary>
		public SitecoreUrl Url
		{
			get
			{
				return new SitecoreUrl(_sitecoreMedia.Url);
			}
		}

		/// <summary>
		/// Converts the media to a plain old CLR object (POCO).
		/// </summary>
		/// <param name="type">The type to convert to.</param>
		/// <returns>The new type.</returns>
		public object ToType(Type type)
		{
			object instance = Activator.CreateInstance(type);

			ILookup<string, PropertyInfo> lookup = typeof(SMedia).GetProperties()
				.ToLookup(property => property.Name, StringComparer.OrdinalIgnoreCase);

			foreach (PropertyInfo property in type.GetProperties())
			{
				SitecoreMediaFieldAttribute fieldAttribute = (SitecoreMediaFieldAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreMediaFieldAttribute));

				if (fieldAttribute != null)
				{
					PropertyInfo found = lookup[property.Name].FirstOrDefault();

					if (found != null)
					{
						property.SetValue(instance, found.GetValue(this, null), null);
					}
				}
			}

			return instance;
		}

		/// <summary>
		/// Converts the media to a plain old CLR object (POCO).
		/// </summary>
		/// <typeparam name="T">The type to convert to.</typeparam>
		/// <returns>The new type.</returns>
		public T ToType<T>()
			where T : class, new()
		{
			return (T)ToType(typeof(T));
		}
	}
}
