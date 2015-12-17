using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;

namespace Minq.Mocks.Mvc
{
	public class MockSitecoreRendering : ISitecoreRendering
	{
		private IDictionary<string, string> _parameters;

		SitecoreItemKey ISitecoreRendering.DataSourceKey
		{
			get
			{
				return DataSourceKey;
			}
		}

		public SitecoreItemKey DataSourceKey
		{
			get;
			set;
		}

        public SitecoreItemKey ItemKey
        {
            get;
            set;
        }

        public IDictionary<string, string> Parameters
		{
			get
			{
				if (_parameters == null)
				{
					_parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				}

				return _parameters;
			}
		}
	}
}
