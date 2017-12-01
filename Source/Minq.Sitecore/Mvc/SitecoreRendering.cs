using System;
using System.Collections.Generic;
using Minq.Mvc;
using ScapiRendering = Sitecore.Mvc.Presentation.Rendering;
using ScapiRenderingContext = Sitecore.Mvc.Presentation.RenderingContext;
using ScapiContext = global::Sitecore.Context;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreRendering : ISitecoreRendering
	{
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
						return new SitecoreItemKey(scapiItem.ID.Guid, ScapiContext.Language.Name, ScapiContext.Database.Name);
					}
				}

				return null;
			}
		}

	    public SitecoreItemKey ItemKey
	    {
	        get
	        {
	            ScapiItem scapiItem = ScapiRendering.Item;

	            if (scapiItem != null)
	            {
                    return new SitecoreItemKey(scapiItem.ID.Guid, ScapiContext.Language.Name, ScapiContext.Database.Name);
	            }

	            return null;
	        }
	    }

		public IDictionary<string, string> Parameters
		{
			get
			{
				IDictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

				foreach (KeyValuePair<string, string> pair in ScapiRendering.Parameters)
				{
					parameters.Add(pair);
				}
				
				return parameters;
			}
		}

		public string Placeholder
		{
			get { return ScapiRendering.Placeholder; }
		}
	}
}
