using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using Minq.Linq;
using System.Collections.Specialized;

namespace Minq.Mvc.Linq
{
	public class SRendering
	{
		private ISitecoreContainer _container;
		private NameValueCollection _parameters;

		public SRendering(ISitecoreContainer container)
		{
			_container = container;
		}

		public SItem DataItem
		{
			get
			{
				ISitecoreContext context = _container.Resolve<ISitecoreContext>();

				ISitecoreItemGateway itemGateway = _container.Resolve<ISitecoreItemGateway>();

				ISitecoreRendering rendering = _container.Resolve<ISitecoreRendering>();

				return new SItem(itemGateway.GetItem(rendering.DataSourceKey), _container);
			}
		}

		public NameValueCollection Parameters
		{
			get
			{
				if (_parameters == null)
				{
					_parameters = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

					ISitecoreRendering rendering = _container.Resolve<ISitecoreRendering>();

					foreach (KeyValuePair<string, string> pair in rendering.Parameters)
					{
						_parameters[pair.Key] = pair.Value;
					}
				}

				return _parameters;
			}
		}
	}
}
