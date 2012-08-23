using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace Minq.Mvc
{
	/// <summary>
	/// Defines an object that represents a HTML hyperlink for use in fluent MVC syntax.
	/// </summary>
	/// <typeparam name="TModel">The model associated with this fluent hyperlink.</typeparam>
	public class SitecoreFieldLink<TModel> : IHtmlString
	{
		private readonly SitecoreHelper<TModel> _sitecorelHelper;
		private LinkedList<IHtmlString> _htmlStringList;
		private ISitecoreFieldMarkup _markup;

		/// <summary>
		/// Initializes the class for use based on a <see cref="SitecoreHelper<TModel>"/> and <see cref="ISitecoreFieldMarkup"/>.
		/// </summary>
		/// <param name="sitecorelHelper">The Sitecore helper to use as part of this fluent htyperlink.</param>
		/// <param name="markup">The field markup for this hyperlink.</param>
		public SitecoreFieldLink(SitecoreHelper<TModel> sitecorelHelper, ISitecoreFieldMarkup markup)
		{
			_sitecorelHelper = sitecorelHelper;
			_markup = markup;
			_htmlStringList = new LinkedList<IHtmlString>();
		}

		private SitecoreFieldLink<TModel> Html(IHtmlString html)
		{
			_htmlStringList.AddLast(html);

			return this;
		}

		public SitecoreFieldLink<TModel> FieldForIfEmpty<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			if (SitecoreFieldMarkupParser.IsEmptyMarkupElement(_markup.GetHtml(null)))
			{
				return Html(_sitecorelHelper.FieldFor<TProperty>(expression, htmlAttributes));
			}
			
			return this;
		}

		/*
		public SitecoreFieldLink<TModel> ImageIfEmpty<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			return Html(_sitecorelHelper.ImageFor<TProperty>(expression, htmlAttributes));
		}*/

		/// <summary>
		/// Gets the HTML markup for this fluent hyperlink.
		/// </summary>
		/// <returns>The HTML markup for this fluent hyperlink.</returns>
		public string ToHtmlString()
		{
			StringBuilder builder = new StringBuilder();

			foreach (IHtmlString htmlString in _htmlStringList)
			{
				builder.Append(htmlString);
			}

			return _markup.GetHtml(builder.ToString());
		}
	}
}
