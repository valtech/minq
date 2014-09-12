using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Minq.Mvc;

namespace Minq.Sitecore.Mvc
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

		public override void InitHelpers()
		{
			base.InitHelpers();

			Sitecore = new SitecoreHelper<TModel>(ViewData, new SitecoreMarkupStrategy(), new SitecorePageMode());
		}
	}

	public abstract class SitecorePage : WebViewPage
	{

	}
}