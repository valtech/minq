using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// When applied to the member of a type, specifies that the member represents a collection of child items of the given POCO type.
	/// </summary>
	/// <remarks>
	/// Multiple attributes can be used if a collection can include different implementations of a super type.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class SitecoreChildTypeAttribute : Attribute
	{
		private Type _childType;

		/// <summary>
		/// Initializes the class for use based on a given child type.
		/// </summary>
		/// <param name="childType">The type of the child type in the collection.</param>
		public SitecoreChildTypeAttribute(Type childType)
		{
			if (childType == null)
			{
				throw new ArgumentNullException("childType");
			}

			_childType = childType;
		}

		/// <summary>
		/// The POCO type of the children. 
		/// </summary>
		public Type ChildType
		{
			get
			{
				return _childType;
			}
		}
	}
}
