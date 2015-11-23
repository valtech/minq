using System;
using System.Xml.Linq;

namespace Minq.Linq
{
	/// <summary>
	/// Represents a HTML link/anchor to a resource.
	/// </summary>
	public class SLink
	{
		private SField _field;
		private string _text;
		private string _url;
		private string _queryString;
		private string _title;
		private string _cssClass;
		private string _target;
		private string _linkedItemId;
		private string _linkType;
		private SItem _linkedItem;
		private bool _linkedItemResolved;
		private SMedia _linkedMedia;
		private bool _linkedMediaResolved;

		/// <summary>
		/// Creates a new HTML link/anchor.
		/// </summary>
		/// <param name="field">The Sitecore field to initialize the link from.</param>
		public SLink(SField field)
		{
			_field = field;

			XDocument document = XDocument.Parse(field.Value<string>());

			_text = GetDocumentAttribute(document, "text");
			_url = GetDocumentAttribute(document, "url");
			_queryString = GetDocumentAttribute(document, "querystring");
			_title = GetDocumentAttribute(document, "title");
			_cssClass = GetDocumentAttribute(document, "class");
			_target = GetDocumentAttribute(document, "target");
			_linkedItemId = GetDocumentAttribute(document, "id");
			_linkType = GetDocumentAttribute(document, "linktype");
		}

		/// <summary>
		/// Gets whether this link is configured to point to Sitecore media content.
		/// </summary>
		public bool IsMediaLink
		{
			get
			{
				return String.Equals(_linkType, "media", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets whether this link is configured to point to other Sitecore site content.
		/// </summary>
		public bool IsInternalLink
		{
			get
			{
				return String.Equals(_linkType, "internal", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets whether this link is configured with a hard coded external URL.
		/// </summary>
		public bool IsExternalLink
		{
			get
			{
				return String.Equals(_linkType, "external", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets whether this link normal URL or fragment based anchor.
		/// </summary>
		public bool IsAnchorLink
		{
			get
			{
				return String.Equals(_linkType, "anchor", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets whether this link is a mail-to command.
		/// </summary>
		public bool IsMailToLink
		{
			get
			{
				return String.Equals(_linkType, "mailto", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets whether this link is a JavaScript command.
		/// </summary>
		public bool IsJavaScriptLink
		{
			get
			{
				return String.Equals(_linkType, "javascript", StringComparison.InvariantCultureIgnoreCase);
			}
		}

		/// <summary>
		/// Gets the text rendered between the HTML anchor start and end tags.
		/// </summary>
		public string Text
		{
			get
			{
				return _text ?? "";
			}
		}

		/// <summary>
		/// Gets the value of the link (if it is not url).
		/// </summary>
		public string Value
		{
			get
			{
				return _url ?? "";
			}
		}

		/// <summary>
		/// Gets the URL title for the rendered HTML anchor.
		/// </summary>
		public string Title
		{
			get
			{
				return _title ?? "";
			}
		}

		/// <summary>
		/// Gets the URL target for the rendered HTML anchor.
		/// </summary>
		public string Target
		{
			get
			{
				return _target ?? "";
			}
		}

		/// <summary>
		/// Gets the URL that could be used in a browser to access the resource.
		/// </summary>
		public SitecoreUrl Url
		{
			get
			{
				if (IsMediaLink)
				{
					SMedia media = LinkedMedia;

					if (media != null)
					{
						return media.Url;
					}
				}
				else
				{
					SItem item = LinkedItem;

					if (item != null)
					{
						return item.Url;
					}
				}

				return new SitecoreUrl(_url ?? "");
			}
		}

		/// <summary>
		/// Gets the <see cref="SItem"/> linked to, or null if not an item.
		/// </summary>
		public SItem LinkedItem
		{
			get
			{
				if (!_linkedItemResolved)
				{
					if (!String.IsNullOrEmpty(_linkedItemId))
					{
						SItem item = _field.Owner.Db.Item(_linkedItemId, _field.Owner.LanguageName);

						if (!SItem.IsNullOrUnversioned(item))
						{
							_linkedItem = item;
						}
					}

					_linkedItemResolved = true;
				}

				return _linkedItem;
			}
		}

		/// <summary>
		/// Gets the <see cref="SMedia"/> linked to, or null if not an item.
		/// </summary>
		public SMedia LinkedMedia
		{
			get
			{
				if (!_linkedMediaResolved)
				{
					if (IsMediaLink)
					{
						SItem linkedItem = LinkedItem;

						if (linkedItem != null)
						{
							_linkedMedia = _field.Owner.Db.Media(_linkedItemId, _field.Owner.LanguageName);
						}
					}

					_linkedMediaResolved = true;
                }

				return _linkedMedia;
			}
		}

		/// <summary>
		/// Gets the underlying field associated with this link. 
		/// </summary>
		public SField Field
		{
			get
			{
				return _field;
			}
		}

		/// <summary>
		/// Determines if a given Sitecore raw value is a link.
		/// </summary>
		/// <param name="value">The Sitecore raw value to check.</param>
		/// <returns>true if the raw value is a link field, false otherwise.</returns>
		public static bool IsLinkField(string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				if (value.StartsWith("<link ") && value.EndsWith("/>"))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Determines if a given Sitecore field's raw value is a link.
		/// </summary>
		/// <param name="field">The Sitecore field to check.</param>
		/// <returns>true if the field is a link field, false otherwise.</returns>
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
