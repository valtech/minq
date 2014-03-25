using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Minq.Mvc;
using ScapiCorePipeline = global::Sitecore.Pipelines.CorePipeline;
using ScapiRenderFieldArgs = global::Sitecore.Pipelines.RenderField.RenderFieldArgs;
using ScapiItem = global::Sitecore.Data.Items.Item;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreMarkupStrategy : ISitecoreMarkupStrategy
	{
		public string GetFieldValue(SitecoreFieldMetadata fieldMetadata)
		{
			ScapiItem item = SitecoreItemGateway.GetScapiItem(fieldMetadata.Key, true);

			return item.Fields[fieldMetadata.FieldName].Value;
		}

		public ISitecoreFieldMarkup GetFieldMarkup(SitecoreFieldMetadata fieldMetadata, SitecoreFieldAttributeDictionary fieldAttributes)
		{
			return new SitecoreFieldMarkup(fieldMetadata, fieldAttributes);
		}

		public ISitecoreEditorMarkup GetEditorMarkup(SitecoreEditorMetadata editorMetadata, SitecoreFieldAttributeDictionary editorAttributes)
		{
			return new SitecoreEditorMarkup(editorMetadata, editorAttributes);
		}
	}
}
