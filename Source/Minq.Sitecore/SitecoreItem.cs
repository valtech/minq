using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiItem = global::Sitecore.Data.Items.Item;
using ScapiTemplateFieldItem = Sitecore.Data.Items.TemplateFieldItem;
using ScapiStandardValuesManager = Sitecore.Data.StandardValuesManager;
using ScapiVersionCollection = global::Sitecore.Collections.VersionCollection;
using ScapiItemManager = global::Sitecore.Data.Managers.ItemManager;
using ScapiVersionComparer = global::Sitecore.Data.VersionComparer;
using ScapiVersion = global::Sitecore.Data.Version;
using ScapiLinkManager = global::Sitecore.Links.LinkManager;
using ScapiUrlOptions = global::Sitecore.Links.UrlOptions;
namespace Minq.Sitecore
{
	/// <summary>
	/// Provides a Sitecore item based on the <see ref="ISitecoreItem" /> interface.
	/// </summary>
	public class SitecoreItem : ISitecoreItem
	{
		private ScapiItem _scapiItem;
		private IDictionary<string, ISitecoreField> _fields;
		private SitecoreItem _parent;

		/// <summary>
		/// Initializes the class based on a <see cref="SitecoreItemKey"/>.
		/// </summary>
		/// <param name="key">The <see cref="SitecoreItemKey" /> used to uniquely identify the item</param>
		public SitecoreItem(ScapiItem sitecoreItem)
		{
			_scapiItem = sitecoreItem;
		}

		/// <summary>
		/// Gets the <see cref="SitecoreItemKey" /> used to uniquely identify the item.
		/// </summary>
		public SitecoreItemKey Key
		{
			get
			{
				return new SitecoreItemKey(_scapiItem.ID.Guid, _scapiItem.Language.Name, _scapiItem.Database.Name);
			}
		}

		public string CustomUrl(SitecoreUrlOptions urlOptions)
		{
			return ScapiLinkManager.GetItemUrl(_scapiItem, new ScapiUrlOptions
			{
				AddAspxExtension = true,
				AlwaysIncludeServerUrl = true,
				LanguageEmbedding = ScapiLinkManager.LanguageEmbedding,
				LowercaseUrls = true,
				EncodeNames = true
			});
		}

		/// <summary>
		/// Gets the name of this item.
		/// </summary>
		public string Name
		{
			get
			{
				return _scapiItem.Name;
			}
		}

		public int[] Versions
		{
			get
			{
				ScapiVersionCollection versions = ScapiItemManager.GetVersions(_scapiItem);

				if (versions != null)
				{
					ScapiVersionComparer comparer = new ScapiVersionComparer();

					return versions
						.OrderBy<ScapiVersion, ScapiVersion>(version => version, comparer)
						.Select(version => version.Number)
						.ToArray();
				}

				return new int[0];
			}
		}

		/// <summary>
		/// Gets all the Sitecore fields defined for this item
		/// based on its template definition.
		/// </summary>
		public IDictionary<string, ISitecoreField> FieldDictionary
		{
			get
			{
				if (_fields == null)
				{
					_fields = new Dictionary<string, ISitecoreField>(StringComparer.OrdinalIgnoreCase);

					foreach (ScapiTemplateFieldItem scapiTemplateFieldItem in _scapiItem.Template.Fields)
					{
						string name = scapiTemplateFieldItem.Name;

						_fields[name] = new SitecoreField(_scapiItem.Fields[name]);
					}
				}

				return _fields;
			}
		}

		/// <summary>
		/// Gets all the Sitecore children defined for this item.
		/// </summary>
		public IEnumerable<ISitecoreItem> Children
		{
			get
			{
				foreach (ScapiItem scapiItem in _scapiItem.Children)
				{
					if (scapiItem != null)
					{
						yield return new SitecoreItem(scapiItem);
					}
				}
			}
		}

		public ISitecoreItem Parent
		{
			get
			{
				if (_scapiItem.Parent != null)
				{
					if (_parent == null)
					{
						_parent = new SitecoreItem(_scapiItem.Parent);
					}
				}

				return _parent;
			}
		}

		public SitecoreTemplateKey TemplateKey
		{
			get
			{
				return new SitecoreTemplateKey(_scapiItem.TemplateID.Guid, _scapiItem.Database.Name);
			}
		}
	}
}
