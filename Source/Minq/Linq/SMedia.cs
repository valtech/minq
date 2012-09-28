using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace Minq.Linq
{
	public class SMedia
	{
		private Guid _guid;
		private string _imageUrl;

		public SMedia(string value)
		{
			if (!String.IsNullOrEmpty(value) && !IsMedia(value))
			{
				throw new Exception("Not a media field");
			}

			XElement element = XDocument.Parse(value).Descendants("image").First();

			Guid.TryParse((string)element.Attribute("mediaid"), out _guid);

			_imageUrl = (string)element.Attribute("src");
		}

		public SMedia(SField field)
			: this(field.Value<string>(null))
		{
			
		}

		public static bool IsMedia(string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				if (value.StartsWith("<image ") && value.EndsWith("/>"))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsMedia(SField field)
		{
			if (!field.IsEmpty)
			{
				string value = field.Value<string>();

				return IsMedia(value);
			}

			return false;
		}

		public Guid Guid
		{
			get
			{
				return _guid;
			}
		}

		public string ImageUrl
		{
			get
			{
				return _imageUrl;
			}
		}

		internal object ToType(Type type)
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

		public T ToType<T>()
			where T : class, new()
		{
			return (T)ToType(typeof(T));
		}
	}
}
