using System;

namespace Minq.Mocks
{
	public class MockSitecoreRequest : MockSitecoreContext, ISitecoreRequest
	{
		private MockSitecorePageMode _pageMode;

		public SitecoreItemKey ItemKey
		{
			get;
			set;
		}

		public MockSitecorePageMode PageMode
		{
			get
			{
				if (_pageMode == null)
				{
					_pageMode = new MockSitecorePageMode();
				}

				return _pageMode;
			}
		}

		public string SiteName
		{
			get;
			set;
		}

		ISitecorePageMode ISitecoreRequest.PageMode
		{
			get
			{
				return PageMode;
			}
		}
	}
}
