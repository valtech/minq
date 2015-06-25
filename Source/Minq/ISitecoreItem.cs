using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a Sitecore item.
	/// </summary>
	public interface ISitecoreItem
	{
		/// <summary>
		/// When overridden in a derived class, gets the <see cref="SitecoreItemKey" /> used to uniquely identify the item.
		/// </summary>
		SitecoreItemKey Key
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets all the Sitecore fields defined for this item
		/// based on its template definition.
		/// </summary>
		IDictionary<string, ISitecoreField> FieldDictionary
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets all the Sitecore children defined for this item.
		/// </summary>
		/// <remarks>
		/// The current Sitecore security rules should be enforced based on the user associated with the current HTTP request. This will typically
		/// be the anonymous user for unauthenticated requests.
		/// </remarks>
		IEnumerable<ISitecoreItem> Children
		{
			get;
		}

		/// <summary>
		/// When overridden in a derived class, gets the Sitecore URL of this item.
		/// </summary>
		SitecoreUrl Url
		{
			get;
		}

		/// <summary>
		/// Gets the parent <see cref="ISitecoreItem"/> of this item.
		/// </summary>
		ISitecoreItem Parent
		{
			get;
		}

		/// <summary>
		/// Gets the versions of this item.
		/// </summary>
		int[] Versions
		{
			get;
		}

		/// <summary>
		/// Gets the languages of this item.
		/// </summary>
		string[] Languages
		{
			get;
		}

		/// <summary>
		/// Gets the name of this item.
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the key of the template that this item is based on.
		/// </summary>
		SitecoreTemplateKey TemplateKey
		{
			get;
		}
	}
}
