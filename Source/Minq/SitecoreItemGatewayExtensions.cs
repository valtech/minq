using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;

namespace Minq
{
	/// <summary>
	/// Provides a set of static methods for querying objects that implement <see cref="ISitecoreItemGateway" />.
	/// </summary>
	public static class SitecoreItemGatewayExtensions
	{
		public static ISitecoreItem GetItem(this ISitecoreItemGateway source, SitecoreItemKey key)
		{
			return source.GetItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
		}
	}
}
