namespace Minq.Mocks
{
	public class MockSitecoreMedia : ISitecoreMedia
	{
		private SitecoreItemKey _key;
		private string _url;

		public MockSitecoreMedia(SitecoreItemKey key)
		{
			_key = key;
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
			get
			{
				return _url;
			}
			set
			{
				_url = value;
			}
		}

		string ISitecoreMedia.Url
		{
			get
			{
				return _url;
			}
		}
	}
}
