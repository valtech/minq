using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mvc
{
	/// <summary>
	/// Defines an object that represents a Sitecore field markup parser.
	/// </summary>
	public static class SitecoreFieldMarkupParser
	{
		/// <summary>
		/// Determines if the given HTML markup is empty content or a tag with no content.
		/// </summary>
		/// <param name="html">The HTML to evaluate.</param>
		/// <returns>true if the HTML markup is empty content or a tag with empty content, false otherwise.</returns>
		public static bool IsEmptyMarkupElement(string html)
		{
			if (String.IsNullOrEmpty(html))
			{
				return true;
			}

			int index = html.Length - 1;
			bool end = false;
			char c = html[index--];

			if (c != '>')
			{
				return false;
			}

			while (index >= 0 && (c = html[index--]) != '<')
			{
				end = c == '/';

				if (end || !Char.IsLetter(c))
				{
					break;
				}
			}

			if (end)
			{
				c = html[index--];

				if (c != '<')
				{
					return false;
				}

				while (index >= 0)
				{
					c = html[index--];

					if (c == '>')
					{
						return true;
					}
					else
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}

		public static string ReplaceContent(string html, string content)
		{
			int first = html.IndexOf('>');
			int last = html.LastIndexOf('<');

			if (first != -1 && last != -1)
			{
				string start = html.Substring(0, first + 1);

				string end = html.Substring(last, html.Length - last);

				return start + content + end;
			}

			return content;
		}
	}
}
