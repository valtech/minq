using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;
using System.Web;
using System.Globalization;
using System.Web.Routing;

namespace Minq.Mvc
{
	/// <summary>
	/// Represents support for Sitecore controls in an application.
	/// </summary>
	public class SitecoreHelper<TModel>
	{
		private HtmlHelper<TModel> _htmlHelper;
		private ISitecoreFieldMarkupStrategy _markupStrategy;

		public SitecoreHelper(HtmlHelper<TModel> htmlHelper, ISitecoreFieldMarkupStrategy markupStrategy)
		{
			_htmlHelper = htmlHelper;
			_markupStrategy = markupStrategy;
		}

		/// <summary>
		/// Returns the correct markup for a Sitecore field for each property in the object that is represented by the specified expression.
		/// </summary>
		/// <typeparam name="TProperty">The type of the value.</typeparam>
		/// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
		/// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
		/// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
		/// <returns>Markup for the Sitecore field.</returns>
		public IHtmlString FieldFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			return FieldFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		private IHtmlString FieldFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			SitecoreFieldMetadata fieldMetadata = SitecoreFieldMetadata.FromLambdaExpression<TModel, TProperty>(expression, _htmlHelper.ViewData);

			SitecoreFieldAttributeDictionary fieldAttributes = SitecoreFieldAttributeDictionary.FromAttributes(htmlAttributes);

			ISitecoreFieldMarkup markup = _markupStrategy.GetFieldMarkup(fieldMetadata, fieldAttributes);

			return new HtmlString(markup.GetHtml(null));
		}

		/// <summary>
		/// Returns the correct markup for a Sitecore hyperlink field for each property in the object that is represented by the specified expression.
		/// </summary>
		/// <typeparam name="TProperty">The type of the value.</typeparam>
		/// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
		/// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
		/// <returns>>Markup for the Sitecore hyperlink.</returns>
		public SitecoreFieldString<TModel> LinkFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			return LinkFor<TProperty>(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		private SitecoreFieldString<TModel> LinkFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			SitecoreFieldMetadata fieldMetadata = SitecoreFieldMetadata.FromLambdaExpression<TModel, TProperty>(expression, _htmlHelper.ViewData);

			SitecoreFieldAttributeDictionary fieldAttributes = SitecoreFieldAttributeDictionary.FromAttributes(htmlAttributes);

			ISitecoreFieldMarkup markup = _markupStrategy.GetFieldMarkup(fieldMetadata, fieldAttributes);

			return new SitecoreFieldString<TModel>(markup);
		}

		public IHtmlString ImageFor<TProperty>(Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
		{
			return FieldFor(expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		private IHtmlString ImageFor<TProperty>(Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			SitecoreFieldMetadata fieldMetadata = SitecoreFieldMetadata.FromLambdaExpression<TModel, TProperty>(expression, _htmlHelper.ViewData);

			SitecoreFieldAttributeDictionary fieldAttributes = SitecoreFieldAttributeDictionary.FromImageAttributes(htmlAttributes);

			ISitecoreFieldMarkup markup = _markupStrategy.GetFieldMarkup(fieldMetadata, fieldAttributes);

			return new HtmlString(markup.GetHtml(null));
		}
	}
}
