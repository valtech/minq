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
		private ISitecoreMedia _sitecoreMedia;

		public SMedia(ISitecoreMedia sitecoreMedia)
		{
			_sitecoreMedia = sitecoreMedia;
		}

		public static bool IsMediaField(string value)
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

		public static bool IsMediaField(SField field)
		{
			if (!field.IsEmpty)
			{
				string value = field.Value<string>();

				return IsMediaField(value);
			}

			return false;
		}

		public Guid Guid
		{
			get
			{
				return _sitecoreMedia.Key.Guid;
			}
		}

		public string Url
		{
			get
			{
				return _sitecoreMedia.Url;
			}
		}

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

		public T ToType<T>()
			where T : class, new()
		{
			return (T)ToType(typeof(T));
		}
	}
}
