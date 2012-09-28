using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using ScapiContext = global::Sitecore.Context;
using ScapiEditFrame = global::Sitecore.Web.UI.WebControls.EditFrame;
using ScapiItem = global::Sitecore.Data.Items.Item;
using ScapiId = global::Sitecore.Data.ID;
using System.IO;
using System.Web.UI;

namespace Minq.Sitecore.Mvc
{
	public class SitecoreEditorMarkup : ISitecoreEditorMarkup
	{
		private SitecoreEditorMetadata _editorMetadata;
		private SitecoreFieldAttributeDictionary _attributes;

		public SitecoreEditorMarkup(SitecoreEditorMetadata editorMetadata, SitecoreFieldAttributeDictionary attributes)
		{
			_editorMetadata = editorMetadata;
			_attributes = attributes;
		}

		public string GetHtml(string childContent)
		{
			ScapiItem item = SitecoreItemGateway.GetScapiItem(_editorMetadata.Key, true);

			Editor editor = new Editor(item);

			if (editor.Visible)
			{
				object buttons;

				if (_attributes.TryGetValue("buttons", out buttons))
				{
					editor.Buttons = String.Format("{0}", buttons);
				}

				object title;

				if (_attributes.TryGetValue("title", out title))
				{
					editor.Title = String.Format("{0}", title);
				}

				editor.Controls.Add(new LiteralControl(childContent));

				using (StringWriter writer = new StringWriter())
				{
					using (HtmlTextWriter html = new HtmlTextWriter(writer))
					{
						editor.RenderFirstPart(html);

						editor.RenderLastPart(html);
					}

					return writer.ToString();
				}
			}

			return null;
		}

		class Editor : ScapiEditFrame
		{
			private ScapiItem _item;

			public Editor(ScapiItem item)
			{
				_item = item;
			}

			protected override ScapiItem GetItem()
			{
				return _item;
			}

			public override bool Visible
			{
				get
				{
					return ShouldRender();
				}
			}
		}
	}
}
