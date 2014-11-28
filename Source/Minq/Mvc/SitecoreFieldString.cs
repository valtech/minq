using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.WebPages;

namespace Minq.Mvc
{
	/// <summary>
	/// Defines an object that represents a HTML markup string for use in fluent MVC syntax.
	/// </summary>
	/// <typeparam name="TModel">The model associated with this fluent HTML markup string.</typeparam>
	public class SitecoreFieldString<TModel> : IHtmlString
	{
		private ISitecoreFieldMarkup _markup;

		/// <summary>
		/// Initializes the class for use based on a <see cref="SitecoreHelper<TModel>"/> and <see cref="ISitecoreFieldMarkup"/>.
		/// </summary>
		/// <param name="sitecorelHelper">The Sitecore helper to use as part of this fluent HTML markup string.</param>
		/// <param name="markup">The field markup for this HTML markup string.</param>
		public SitecoreFieldString(ISitecoreFieldMarkup markup)
		{
			_markup = markup;
		}

		/// <summary>
		/// Inserts content into an HTML tag if there is no content between the tags.
		/// </summary>
		/// <param name="ifEmptyPredicate">The predicate to supply the supplementary markup.</param>
		/// <returns>The supplemented markup.</returns>
		public IHtmlString IfEmpty<THtmlString>(Func<THtmlString> ifEmptyPredicate)
			where THtmlString : IHtmlString
		{
			if (SitecoreFieldMarkupParser.IsEmptyMarkupElement(_markup.GetHtml(null)))
			{
				return new HtmlString(_markup.GetHtml(ifEmptyPredicate().ToHtmlString()));
			}

			return this;
		}

		/// <summary>
		/// Inserts content into an HTML tag if there is no content between the tags.
		/// </summary>
		/// <param name="htmlPredicate">The HTML template to supply the supplementary markup.</param>
		/// <returns>The supplemented markup.</returns>
		public IHtmlString IfEmpty(Func<object, object> htmlPredicate)
		{
			if (SitecoreFieldMarkupParser.IsEmptyMarkupElement(_markup.GetHtml(null)))
			{
				object helperResult = htmlPredicate(null);

				if (helperResult != null)
				{
					return new HtmlString(_markup.GetHtml(helperResult.ToString()));
				}

				return this;
			}

			return this;
		}

		public IHtmlString Content<THtmlString>(Func<THtmlString> contentPredicate)
			where THtmlString : IHtmlString
		{
			string content = contentPredicate().ToHtmlString();

			if (content != null)
			{
				return new HtmlString(_markup.GetHtml(content));
			}

			return this;
		}

		public IHtmlString Content(Func<object, object> htmlPredicate)
		{
			object helperResult = htmlPredicate(null);

			if (helperResult != null)
			{
				return new HtmlString(_markup.GetHtml(helperResult.ToString()));
			}

			return this;
		}

		/// <summary>
		/// Gets the HTML markup for this fluent HTML markup string.
		/// </summary>
		/// <returns>The evaluated HTML markup.</returns>
		public string ToHtmlString()
		{
			return _markup.GetHtml(null);
		}
	}
}
