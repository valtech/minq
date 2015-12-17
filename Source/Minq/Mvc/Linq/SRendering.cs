using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using Minq.Linq;
using System.Collections.Specialized;

namespace Minq.Mvc.Linq
{
	/// <summary>
	/// Defines an object that reperesenting an MVC controller rendering.
	/// </summary>
	public class SRendering
	{
		private SItemComposer _composer;
		private NameValueCollection _parameters;
		private ISitecoreRendering _rendering;

		/// <summary>
		/// Initializes the class for use based on the current rendering context.
		/// </summary>
		/// <param name="rendering">The Sitecore rendering providing data about the current rendering context.</param>
		/// <param name="composer">The item composer to construct new Sitecore items.</param>
		public SRendering(ISitecoreRendering rendering, SItemComposer composer)
		{
			_rendering = rendering;
			_composer = composer;
		}

		/// <summary>
		/// Gets the Sitecore item by resolving the data source for the current controller rendering.
		/// </summary>
		public SItem DataItem
		{
			get
			{
				SitecoreItemKey key = _rendering.DataSourceKey;

				if (key != null)
				{
					return _composer.CreateItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
				}
				else
				{
					return null;
				}
			}
		}

        /// <summary>
		/// Gets the Sitecore item by resolving the Item property for the rendering.
		/// </summary>
	    public SItem Item
	    {
	        get
	        {
	            SitecoreItemKey key = _rendering.ItemKey;

	            if (key != null)
	            {
	                return _composer.CreateItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
	            }
	            else
	            {
	                return null;
	            }
	        }
	    }

		/// <summary>
		/// Gets the rendering parameters for MVC controller rendering.
		/// </summary>
		public NameValueCollection Parameters
		{
			get
			{
				if (_parameters == null)
				{
					_parameters = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

					foreach (KeyValuePair<string, string> pair in _rendering.Parameters)
					{
						_parameters[pair.Key] = pair.Value;
					}
				}

				return _parameters;
			}
		}
	}
}
