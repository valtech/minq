using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using ScapiRendering = Sitecore.Mvc.Presentation.Rendering;
using ScapiRenderingContext = Sitecore.Mvc.Presentation.RenderingContext;
using ScapiContext = global::Sitecore.Context;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreRendering : ISitecoreRendering
	{
		private IDictionary<string, string> _parameters;

		private ScapiRendering ScapiRendering
		{
			get
			{
				ScapiRenderingContext context = ScapiRenderingContext.CurrentOrNull;

				if (context != null)
				{
					return context.Rendering;
				}

				return null;
			}
		}

		public SitecoreItemKey DataSourceKey
		{
			get
			{
				string dataSource = ScapiRendering.DataSource;

				if (!String.IsNullOrEmpty(dataSource))
				{
					ScapiItem scapiItem = ScapiContext.Database.GetItem(dataSource);

					if (scapiItem != null)
					{
						return new SitecoreItemKey(scapiItem.ID.Guid, new SitecoreContext());
					}
				}

				return null;
			}
		}

		public IDictionary<string, string> Parameters
		{
			get
			{
				if (_parameters == null)
				{
					_parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

					foreach (KeyValuePair<string, string> pair in ScapiRendering.Parameters)
					{
						_parameters.Add(pair);
					}
				}

				return _parameters;
			}
		}
	}
}
