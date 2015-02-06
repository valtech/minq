using System;
using System.Collections.Generic;
using System.Linq;

namespace Minq.Linq
{
	sealed class LazyChildrenCollection<T> : LazyCollection<T>
		where T : class, new()
	{
		private SItem _item;
		private IList<T> _children;
		private Type _childType;

		public LazyChildrenCollection(SItem item)
			: this(item, null)
		{
			
		}

		public LazyChildrenCollection(SItem item, Type childType)
		{
			_item = item;
			_childType = childType;
        }

		private IList<T> Children
		{
			get
			{
				if (_children == null)
				{
					IEnumerable<SItem> items;

                    if (_childType != null)
					{
						items = _item.Items(item => item.Template.IsBasedOn(_childType));
                    }
					else
					{
						items = _item.Items();
					}

					items = items.Where(item => !SItem.IsNullOrUnversioned(item));

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
