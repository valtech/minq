using System;

namespace Minq.Caching
{
	class FetchLock<TValue> where TValue : class
	{
		private volatile bool _got;
		private TValue _value;

		public TValue Fetch(TValue value)
		{
			if (!_got)
			{
				lock (this)
				{
					if (!_got)
					{
						_value = value;

						_got = true;
					}
				}
			}

			return _value;
		}
		
		public TValue Fetch<TKey>(TKey key, Func<TKey, TValue> factory)
		{
			if (!_got)
			{
				lock (this)
				{
					if (!_got)
					{
						_value = factory(key);

						_got = true;
					}
				}
			}

			return _value;
		}
	}
}
