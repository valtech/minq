using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// When applied to the member of a type, specifies that the member represents a collection of child items for the parent item.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class SitecoreChildrenAttribute : Attribute
	{
	}
}
