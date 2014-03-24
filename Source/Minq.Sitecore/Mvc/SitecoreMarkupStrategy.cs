using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Minq.Mvc;
using ScapiCorePipeline = global::Sitecore.Pipelines.CorePipeline;
using ScapiRenderFieldArgs = global::Sitecore.Pipelines.RenderField.RenderFieldArgs;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreMarkupStrategy : ISitecoreMarkupStrategy
	{
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
