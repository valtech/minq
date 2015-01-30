using Minq.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minq.Caching
{
	class CachedItem : SItem
	{
		public CachedItem(ISitecoreItem sitecoreItem, SItemComposer itemComposer)
			: base(sitecoreItem, itemComposer)
		{

		}
	}
}
