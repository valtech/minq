using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	sealed class LazyChildrenCollection<T> : LazyCollection<T>
		where T : class, new()
	{
		private SItem _item;
		private IList<T> _children;

		public LazyChildrenCollection(SItem item)
		{
			_item = item;
		}

		private IList<T> Children
		{
			get
			{
				if (_children == null)
				{
					IEnumerable<SItem> items = _item.Items()
						.Where(item => !SItem.IsNullOrUnversioned(item));

					_children = new List<T>(items.Select(item => item.ToType<T>()));
				}

				return _children;
			}
		}

		public override int Count
		{
			get
			{
				return Children.Count;
			}
		}

		public override IEnumerator<T> GetEnumerator()
		{
			return Children.GetEnumerator();
		}
	}
}
