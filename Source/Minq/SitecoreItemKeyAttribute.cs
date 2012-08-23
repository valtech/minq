using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Minq
{
	/// <summary>
	/// When applied to the member of a type, specifies that the member is a <see cref="SitecoreItemKey" />.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class SitecoreItemKeyAttribute : Attribute
	{
		/// <summary>
		/// Gets a <see cref="SitecoreItemKey"/> for a given object instance of an object by searching the members for a
		/// <see cref="SitecoreItemKeyAttribute"/>.
		/// </summary>
		/// <remarks>
		/// This method throws an exception if it cannot find a <see cref="SitecoreItemKey"/> on the given instance.
		/// </remarks>
		/// <param name="instance">The object to search the members of.</param>
		/// <returns>A <see cref="SitecoreItemKey"/> if found, otherwise an exception is thrown.</returns>
		public static SitecoreItemKey FindKey(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			Type type = instance.GetType();

			foreach (PropertyInfo property in type.GetProperties())
			{
				SitecoreItemKeyAttribute itemKeyAttribute = (SitecoreItemKeyAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreItemKeyAttribute));

				if (itemKeyAttribute != null)
				{
					return (SitecoreItemKey)property.GetValue(instance, null);
				}
			}

			throw new Exception(String.Format("Item of type {0} has no property with a SitecoreItemKeyAttribute.", type));
		}
	}
}
