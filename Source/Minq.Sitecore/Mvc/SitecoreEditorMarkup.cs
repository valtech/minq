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
		private SitecoreFieldAttributeDictionary _attributes;

		public SitecoreEditorMarkup(SitecoreFieldAttributeDictionary attributes)
		{
			_attributes = attributes;
		}

		public string GetHtml(string childContent)
		{
			Editor editor = new Editor();

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
			protected override ScapiItem GetItem()
			{
				return null;
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
