using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// When applied to a type, specifies that the type represents a given template.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class SitecoreTemplateAttribute : Attribute
	{
		private Guid _guid;

		/// <summary>
		/// Initializes the class for use based on a given template GUID.
		/// </summary>
		/// <param name="guid">The GUID of the template that this type represents.</param>
		public SitecoreTemplateAttribute(string guid)
		{
			_guid = new Guid(guid);
		}

		/// <summary>
		/// The GUID of the template that the type represents.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _guid;
			}
		}

		/// <summary>
		/// Gets a <see cref="Guid"/> for a given type by looking for an
		/// <see cref="SitecoreTemplateAttribute"/>.
		/// </summary>
		/// <remarks>
		/// This method throws an exception if it cannot find a <see cref="SitecoreTemplateAttribute"/> on the given instance.
		/// </remarks>
		/// <param name="type">The type to search.</param>
		/// <returns>A <see cref="Guid"/> if found, otherwise an exception is thrown.</returns>
		public static Guid FindTemplateGuid(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			SitecoreTemplateAttribute attribute = (SitecoreTemplateAttribute)Attribute.GetCustomAttribute(type, typeof(SitecoreTemplateAttribute));

			if (attribute != null)
			{
				return attribute.Guid;
			}

			throw new Exception(String.Format("Item of type {0} has no property with a SitecoreItemKeyAttribute.", type));
		}
	}
}
