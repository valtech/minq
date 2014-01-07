using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using Minq.Linq;
using System.Collections.Specialized;

namespace Minq.Mvc.Linq
{
	public class SRendering
	{
		private SItemComposer _composer;
		private NameValueCollection _parameters;
		private ISitecoreRendering _rendering;
		//private SDb _db;

		public SRendering(ISitecoreRendering rendering, SItemComposer composer /*, SDb db*/)
		{
			_rendering = rendering;
			_composer = composer;
			//_db = db;
		}

		public SItem DataItem
		{
			get
			{
				SitecoreItemKey key = _rendering.DataSourceKey;

				return _composer.CreateItem(key.Guid.ToString(), key.LanguageName, key.DatabaseName);
			}
		}

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
