using System;

namespace Minq.Mocks
{
	public class MockSitecoreMedia : ISitecoreMedia
	{
		private SitecoreItemKey _key;

		public MockSitecoreMedia(SitecoreItemKey key)
		{
			_key = key;
		}

		public string AlternateText
		{
			get;
			set;
		}

		public SitecoreItemKey Key
		{
			get
			{
				return _key;
			}
		}

		public string Url
		{
			get;
			set;
		}
	}
}
