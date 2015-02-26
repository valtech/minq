using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Minq.Mvc
{
	/// <summary>
	/// Defines an object that represents the HTML/Sitecore attributes for a Sitecore field as part of its
	/// representation in markup.
	/// </summary>
	public class SitecoreFieldAttributeDictionary : Dictionary<string, object>
	{
		private SitecoreFieldAttributeDictionary()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="SitecoreFieldAttributeDictionary"/> based on an existing dictionary of general field attributes.
		/// </summary>
		/// <param name="htmlAttributes">The dictionary of general field attributes to base this dictionary on.</param>
		/// <returns>A new <see cref="SitecoreFieldAttributeDictionary"/>.</returns>
		public static SitecoreFieldAttributeDictionary FromAttributes(IDictionary<string, object> htmlAttributes)
		{
			SitecoreFieldAttributeDictionary parameters = new SitecoreFieldAttributeDictionary();

			if (htmlAttributes != null)
			{
				foreach (KeyValuePair<string, object> pair in htmlAttributes)
				{
					parameters[pair.Key] = pair.Value;
				}
			}

			return parameters;
		}

		/// <summary>
		/// Creates an instance of <see cref="SitecoreFieldAttributeDictionary"/> based on an existing dictionary of image attributes.
		/// </summary>
		/// <param name="htmlAttributes">The dictionary of image attributes to base this dictionary on.</param>
		/// <returns>A new <see cref="SitecoreFieldAttributeDictionary"/>.</returns>
		/// <remarks>
		/// This method converts standard attributes like width and height to Sitecore field equivalents
		/// like mw and mh to ensure scaling is applied properly.
		/// </remarks>
		public static SitecoreFieldAttributeDictionary FromImageAttributes(IDictionary<string, object> htmlAttributes)
		{
			IDictionary<string, object> modifiedHtmlAttributes = null;

			if (htmlAttributes != null)
			{
				modifiedHtmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

				foreach (KeyValuePair<string, object> pair in htmlAttributes)
				{
					string name = pair.Key;

					object value = pair.Value;

					string text = String.Format(CultureInfo.InvariantCulture, "{0}", value);

					if (String.Equals(name, "width", StringComparison.OrdinalIgnoreCase))
					{
						name = "mw";

						value = SitecoreFieldAttributeParser.ConvertCssSizeUnitToInt32(text);
					}
					else if (String.Equals(name, "height", StringComparison.OrdinalIgnoreCase))
					{
						name = "mh";

						value = SitecoreFieldAttributeParser.ConvertCssSizeUnitToInt32(text);
					}

					modifiedHtmlAttributes[name] = value;
				}
			}

			return FromAttributes(modifiedHtmlAttributes);
		}
	}
}
