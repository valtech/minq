﻿using System;
using System.Collections.Generic;

namespace Minq.Mocks
{
	/// <summary>
	/// Provides a unit testable version of the <see ref="ISitecoreItem" /> interface.
	/// </summary>
	public class MockSitecoreItem : ISitecoreItem
	{
		private SitecoreUrl _url;
		private SitecoreItemKey _key;
		private IDictionary<string, ISitecoreField> _fields = new Dictionary<string, ISitecoreField>(StringComparer.OrdinalIgnoreCase);
		private IList<ISitecoreItem> _children = new List<ISitecoreItem>();
		private ISitecoreItem _parent;
		private int[] _versions;
		private string[] _languages;

		/// <summary>
		/// Initializes the class based on a <see cref="SitecoreItemKey"/>.
		/// </summary>
		/// <param name="key">The <see cref="SitecoreItemKey" /> used to uniquely identify the item</param>
		public MockSitecoreItem(SitecoreItemKey key)
		{
			_key = key;

			_versions = new int[] { 1 };
        }

		public SitecoreUrl Url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
			}
		}

		/// <summary>
		/// Gets the <see cref="SitecoreItemKey" /> used to uniquely identify the item.
		/// </summary>
		public SitecoreItemKey Key
		{
			get
			{
				return _key;
			}
		}

		/// <summary>
		/// Adds a field to this mock Sitecore item.
		/// </summary>
		/// <param name="field">The field to add.</param>
		public void AddField(ISitecoreField field)
		{
			_fields[field.Name] = field;
		}

		/// <summary>
		/// Adds a child item to this mock Sitecore item.
		/// </summary>
		/// <param name="child">The child item to add.</param>
		public void AddChild(MockSitecoreItem child)
		{
			child._parent = this;

			_children.Add(child);
		}

		/// <summary>
		/// Gets all the Sitecore fields defined for this item
		/// based on its template definition.
		/// </summary>
		public IDictionary<string, ISitecoreField> FieldDictionary
		{
			get
			{
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
				return _children;
			}
		}

		public ISitecoreItem Parent
		{
			get
			{
				return _parent;
			}
		}

		public int[] Versions
		{
			get
			{
				return _versions ?? new int[0];
			}
			set
			{
				_versions = value;
			}
		}

		public string[] Languages
		{
			get
			{
				return _languages ?? new string[0];
			}
			set
			{
				_languages = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of this item.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		SitecoreTemplateKey ISitecoreItem.TemplateKey
		{
			get
			{
				return TemplateKey;
			}
		}

		public SitecoreTemplateKey TemplateKey
		{
			get;
			set;
		}
	}
}
