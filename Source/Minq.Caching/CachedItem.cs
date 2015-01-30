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

		public override IEnumerable<SItem> Items()
		{
			//TODO: push to cache
			return base.Items();
		}

		public override SItem Parent
		{
			get
			{
				//TODO: push to cache
				return base.Parent;
			}
		}
	}
}
