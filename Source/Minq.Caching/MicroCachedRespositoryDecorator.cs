using System;
using System.Threading;

namespace Minq.Caching
{
	public class MicroCachedRespositoryDecorator<TKey, TValue> : ICachedRepository<TKey, TValue>, IDisposable
	{
		private Timer _timer;
		private ICachedRepository<TKey, TValue> _repositoryToDecorate;

		public MicroCachedRespositoryDecorator(ICachedRepository<TKey, TValue> repositoryToDecorate, TimeSpan period)
		{
			_repositoryToDecorate = repositoryToDecorate;
			_timer = new Timer(Callback, null, period, period);
		}

		private void Callback(object state)
		{
			ClearCache();
		}

		public void ClearCache()
		{
			_repositoryToDecorate.ClearCache();
        }

		public TValue GetOrAdd(TKey key, TValue value)
		{
			return _repositoryToDecorate.GetOrAdd(key, value);
        }

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			return _repositoryToDecorate.GetOrAdd(key, factory);
		}

		public void Dispose()
		{
			_timer.Dispose();
        }
	}
}
