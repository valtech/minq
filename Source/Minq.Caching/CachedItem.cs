using Minq.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Minq.Caching
{
	/// <summary>
	/// Defines an object that represents a cached Sitecore LINQ item.
	/// </summary>
	/// <remarks>
	/// This implementation makes sure that any related items (children/ancestors) are routed through the caching policy.
	/// </remarks>
	class CachedItem : SItem
	{
		private ISitecoreItem _sitecoreItem;
		private CachedItemComposer _itemComposer;
		private IReadOnlyList<SItem> _items;
		private SItem _parent;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreItem"/> and <see cref="CachedItemComposer"/>.
		/// </summary>
		/// <param name="sitecoreItem">The low level Sitecore item that represents this LINQ item.</param>
		/// <param name="itemComposer">The Sitecore item composer.</param>
		public CachedItem(ISitecoreItem sitecoreItem, CachedItemComposer itemComposer)
			: base(sitecoreItem, itemComposer)
		{
			_sitecoreItem = sitecoreItem;
			_itemComposer = itemComposer;
		}

		/// <summary>
		/// Returns a collection of the cached child items including unversioned items of this item or document, in order.
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="SItem"/> containing the child items of this item, in order.</returns>
		public override IEnumerable<SItem> ItemsIncludingUnversioned()
		{
			if (_items == null)
			{
				_items = _sitecoreItem.Children
					.Select(item => _itemComposer.GetOrAdd(item))
					.ToList();
			}

			return _items;
		}

		/// <summary>
		/// Gets the cached parent <see cref="SItem"/> of this LINQ item.
		/// </summary>
		public override SItem Parent
		{
			get
			{
				if (_parent == null)
				{
					ISitecoreItem sitecoreParent = _sitecoreItem.Parent;

					if (sitecoreParent != null)
					{
						_parent = _itemComposer.GetOrAdd(sitecoreParent);
					}
				}

				return _parent;
			}
		}
	}
}
