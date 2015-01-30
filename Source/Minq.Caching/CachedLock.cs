using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	class CachedLock
	{
		private volatile bool _got;
		private object _value;

		public TValue Get<TValue>(Func<TValue> factory)
		{
			if (!_got)
			{
				lock (this)
				{
					if (!_got)
					{
						_value = factory();

						_got = true;
					}
				}
			}

			return (TValue)_value;
		}
	}
}
