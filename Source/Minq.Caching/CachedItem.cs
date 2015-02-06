using Minq.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Minq.Caching
{
	class CachedItem : SItem
	{
		private ISitecoreItem _sitecoreItem;
		private CachedItemComposer _itemComposer;
		private IReadOnlyList<SItem> _items;
		private SItem _parent;

		public CachedItem(ISitecoreItem sitecoreItem, CachedItemComposer itemComposer)
			: base(sitecoreItem, itemComposer)
		{
			_sitecoreItem = sitecoreItem;
			_itemComposer = itemComposer;
        }

		public override IEnumerable<SItem> Items()
		{
			if (_items == null)
			{
				_items = _sitecoreItem.Children
					.Select(item => _itemComposer.GetOrAdd(item))
					.ToList();
			}
			
			return _items;
		}

		public override SItem Parent
		{
			get
			{
				if (_parent == null)
				{
					_parent = _itemComposer.GetOrAdd(_sitecoreItem.Parent);
                }
				
				return _parent;
			}
		}
	}
}
