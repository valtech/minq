using System.Collections.Generic;

namespace Minq.Mvc
{
	public interface ISitecoreRendering
	{
		SitecoreItemKey DataSourceKey
		{
			get;
		}

        SitecoreItemKey ItemKey
        {
            get;
        }

        IDictionary<string, string> Parameters
		{
			get;
		}

		string PlaceholderKey
		{
			get;
		}
	}
}
