using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	public interface ICachedRepository<TKey, TValue>
	{
		TValue GetOrAdd(TKey key, Func<TKey, TValue> factory);

		TValue GetOrAdd(TKey key, TValue value);

		void Clear();
    }
}
