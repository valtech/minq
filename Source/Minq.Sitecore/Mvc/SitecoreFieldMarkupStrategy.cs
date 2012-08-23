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
	public class SitecoreFieldMarkupStrategy : ISitecoreFieldMarkupStrategy
	{
		public ISitecoreFieldMarkup GetFieldMarkup(SitecoreFieldMetadata fieldMetadata, SitecoreFieldAttributeDictionary fieldAttributes)
		{
			ScapiRenderFieldArgs args = new ScapiRenderFieldArgs
			{
				Item = SitecoreItemGateway.GetScapiItem(fieldMetadata.Key, true),
				FieldName = fieldMetadata.FieldName,
				Parameters = fieldAttributes.ToSafeDictionary()
			};

			ScapiCorePipeline.Run("renderField", args);

			return new SitecoreFieldMarkup(args.Result);
		}
	}
}
