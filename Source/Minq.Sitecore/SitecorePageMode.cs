using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiContext = Sitecore.Context;

namespace Minq.Sitecore
{
	public class SitecorePageMode : ISitecorePageMode
	{
		public bool IsNormal
		{
			get
			{
				return ScapiContext.PageMode.IsNormal;
			}
		}

		public bool IsPageEditor
		{
			get
			{
				return ScapiContext.PageMode.IsPageEditor;
			}
		}

		public bool IsPreview
		{
			get
			{
				return ScapiContext.PageMode.IsPreview;
			}
		}

		public bool IsDebug
		{
			get
			{
				return ScapiContext.PageMode.IsDebugging;
			}
		}
	}
}
