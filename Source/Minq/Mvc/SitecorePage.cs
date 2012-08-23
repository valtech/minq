using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Minq.Mvc
{
	/// <summary>
	/// Represents the properties and methods to render a Sitecore view.
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	public abstract class SitecorePage<TModel> : WebViewPage<TModel>
	{
		/// <summary>
		/// Gets the Sitecore helper for the view.
		/// </summary>
		public SitecoreHelper<TModel> Sitecore
		{
			get;
			set;
		}
	}
}
