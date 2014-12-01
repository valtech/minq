using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	sealed class LazyFieldCollection<T> : LazyCollection<T>
			where T : class, new()
	{
		private SField _field;
		private IList<T> _items;
		private IList<Guid> _guids;

		public LazyFieldCollection(SField field)
		{
			_field = field;

			IEnumerable<Guid> guids = field.Value<IEnumerable<Guid>>();

			if (guids != null)
			{
				_guids = field.Value<IEnumerable<Guid>>().ToList();
			}
			else
			{
				_guids = new List<Guid>();
			}
		}

		public override int Count
		{
			get
			{
				return _guids.Count;
			}
		}
		
		public override IEnumerator<T> GetEnumerator()
		{
			if (_items == null)
			{
				_items = _field.Value<IEnumerable<SItem>>()
					.Where(item => !SItem.IsNullOrUnversioned(item))
					.ToType<T>()
					.ToList();
			}

			return _items.GetEnumerator();
		}
	}
}
