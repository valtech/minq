using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	public class MockSitecoreRequest : MockSitecoreContext, ISitecoreRequest
	{
		public SitecoreItemKey ItemKey
		{
			get;
			set;
		}
	}
}
