using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using ScapiRenderFieldResult = global::Sitecore.Xml.Xsl.RenderFieldResult;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreFieldMarkup : ISitecoreFieldMarkup
	{
		private ScapiRenderFieldResult _scapiRenderFieldResult;

		public SitecoreFieldMarkup(ScapiRenderFieldResult scapiRenderFieldResult)
		{
			_scapiRenderFieldResult = scapiRenderFieldResult;
		}

		public string GetHtml(string childContent)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(_scapiRenderFieldResult.FirstPart);

			if (!String.IsNullOrEmpty(childContent))
			{
				builder.Append(childContent);
			}

			builder.Append(_scapiRenderFieldResult.LastPart);

			return builder.ToString();
		}
	}
}
