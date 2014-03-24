using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using ScapiRenderFieldResult = global::Sitecore.Xml.Xsl.RenderFieldResult;
using ScapiCorePipeline = global::Sitecore.Pipelines.CorePipeline;
using ScapiRenderFieldArgs = global::Sitecore.Pipelines.RenderField.RenderFieldArgs;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreFieldMarkup : ISitecoreFieldMarkup
	{
		private SitecoreFieldMetadata _fieldMetadata;
		private SitecoreFieldAttributeDictionary _fieldAttributes;

		public SitecoreFieldMarkup(SitecoreFieldMetadata fieldMetadata, SitecoreFieldAttributeDictionary fieldAttributes)
		{
			_fieldMetadata = fieldMetadata;
			_fieldAttributes = fieldAttributes;
		}

		public string GetHtml(string childContent)
		{
			SitecoreFieldAttributeDictionary fieldAttributes = _fieldAttributes;

			if (childContent != null)
			{
				fieldAttributes = SitecoreFieldAttributeDictionary.FromAttributes(fieldAttributes);

				fieldAttributes["haschildren"] = true;
			}

			ScapiRenderFieldArgs args = new ScapiRenderFieldArgs
			{
				Item = SitecoreItemGateway.GetScapiItem(_fieldMetadata.Key, true),
				FieldName = _fieldMetadata.FieldName,
				Parameters = fieldAttributes.ToSafeDictionary()
			};

			ScapiCorePipeline.Run("renderField", args);

			ScapiRenderFieldResult scapiRenderFieldResult = args.Result;

			StringBuilder builder = new StringBuilder();

			builder.Append(scapiRenderFieldResult.FirstPart);

			if (!String.IsNullOrEmpty(childContent))
			{
				builder.Append(childContent);
			}

			builder.Append(scapiRenderFieldResult.LastPart);

			return builder.ToString();
		}

		/*
		private ScapiRenderFieldResult _scapiRenderFieldResult;

		public SitecoreFieldMarkup(ScapiRenderFieldResult scapiRenderFieldResult)
		{
			_scapiRenderFieldResult = scapiRenderFieldResult;
		}

		public string GetHtml(string childContent)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(_scapiRenderFieldResult.FirstPart);

			if (!String.IsNullOrEmpty(childContent))
			{
				builder.Append(childContent);
			}

			builder.Append(_scapiRenderFieldResult.LastPart);

			return builder.ToString();
		}*/
	}
}
