using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Minq.Mvc;
using ScapiSafeDictionary = global::Sitecore.Collections.SafeDictionary<string>;

namespace Minq.Sitecore.Mvc
{
	public static class SitecoreFieldAttributeDictionaryExtensions
	{
		public static ScapiSafeDictionary ToSafeDictionary(this SitecoreFieldAttributeDictionary source)
		{
			ScapiSafeDictionary dictionary = new ScapiSafeDictionary();

			if (source != null)
			{
				foreach (KeyValuePair<string, object> pair in source)
				{
					dictionary[pair.Key] = String.Format(CultureInfo.InvariantCulture, "{0}", pair.Value);
				}
			}

			return dictionary;
		}
	}
}
