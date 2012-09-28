using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mvc
{
	public class SitecoreEditorMetadata
	{
		/// <summary>
		/// Gets the Sitecore item key associated with the Sitecore field associated with the metadata.
		/// </summary>
		public SitecoreItemKey Key
		{
			get;
			set;
		}

		public static SitecoreEditorMetadata FromModel<TModel>(TModel model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}

			SitecoreEditorMetadata metadata = new SitecoreEditorMetadata();

			SitecoreItemKey key = SitecoreItemKeyAttribute.FindKey(model);

			if (key != null)
			{
				metadata.Key = key;
			}
			else
			{
				throw new Exception(String.Format("Item of type {0} has a null key.", typeof(TModel)));
			}

			return metadata;
		}
	}
}
