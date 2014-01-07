using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Minq.Linq
{
	public class SLink
	{
		private SField _field;
		private string _text;
		private string _url;
		private string _anchor;
		private string _queryString;
		private string _title;
		private string _cssClass;
		private string _target;
		private string _id;
		private string _linkType;

		public SLink(SField field)
		{
			_field = field;

			//parse XML :-)

			XDocument document = XDocument.Parse(field.Value<string>());

			_text = GetDocumentAttribute(document, "text");
			_url = GetDocumentAttribute(document, "url");
			_anchor = GetDocumentAttribute(document, "url");
			_queryString = GetDocumentAttribute(document, "querystring");
			_title = GetDocumentAttribute(document, "title");
			_cssClass = GetDocumentAttribute(document, "class");
			_target = GetDocumentAttribute(document, "target");
			_id = GetDocumentAttribute(document, "id");
			_linkType = GetDocumentAttribute(document, "linktype");
		}

		public SField Field
		{
			get
			{
				return _field;
			}
		}

		public static bool IsLinkField(string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				if (value.StartsWith("<image ") && value.EndsWith("/>"))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsLinkField(SField field)
		{
			if (!field.IsEmpty)
			{
				string value = field.Value<string>();

				return IsLinkField(value);
			}

			return false;
		}

		private static string GetDocumentAttribute(XDocument document, string name)
		{
			XElement element = document.Element("link");

			if (element != null)
			{
				XAttribute attribute = element.Attribute(name);

				if (attribute != null)
				{
					return attribute.Value;
				}
			}

			return null;
		}
	}
}

/*
<link
 * text="Description"
 * linktype="internal"
 * url="/Home.aspx"
 * anchor="Anchor"
 * querystring="QS"
 * title="Alt"
 * class="Class"
 * target=""
 * id="{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"
 * />
*/