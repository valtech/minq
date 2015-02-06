using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents a Sitecore LINQ item.
	/// </summary>
	public class SItem
	{
		private ISitecoreItem _sitecoreItem;
		private SItemComposer _itemComposer;
		private SDb _db;
		private STemplate _template;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreItem"/> and <see cref="SItemComposer"/>.
		/// </summary>
		/// <param name="sitecoreItem">The low level Sitecore item that represents this LINQ item.</param>
		/// <param name="itemComposer">The Sitecore item composer.</param>
		public SItem(ISitecoreItem sitecoreItem, SItemComposer itemComposer)
		{
			_sitecoreItem = sitecoreItem;
			_itemComposer = itemComposer;
		}

		public SItemComposer Composer
		{
			get
			{
				return _itemComposer;
			}
		}

		/// <summary>
		/// Returns a Sitecore field from the LINQ item.
		/// </summary>
		/// <param name="name">The name of the field.</param>
		/// <returns>A LINQ field.</returns>
		public SField Field(string name)
		{
			ISitecoreField field;

			if (_sitecoreItem.FieldDictionary.TryGetValue(name, out field))
			{
				return new SField(field, this);
			}

			return new SField(null, this);
		}

		/// <summary>
		/// The GUID of the item.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _sitecoreItem.Key.Guid;
			}
		}

		public string LanguageName
		{
			get
			{
				return _sitecoreItem.Key.LanguageName;
			}
		}

		/// <summary>
		/// The database from which this item came (typically the web database, but could be master, core or a custom site).
		/// </summary>
		public SDb Db
		{
			get
			{
				if (_db == null)
				{
					_db = new SDb(_sitecoreItem.Key.DatabaseName, _itemComposer);
				}

				return _db;
			}
		}

		/// <summary>
		/// The URL that could be used in a browser to request this item if it represents a page (it would need presentaiton details configured).
		/// </summary>
		public SitecoreUrl Url
		{
			get
			{
				return _sitecoreItem.Url;
			}
		}

		/// <summary>
		/// Determines if the given item has at least one version and is not null.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <returns>true if the item is null or unversioned; false otherwise</returns>
		public static bool IsNullOrUnversioned(SItem item)
		{
			return item == null || !item.Versions().Any();
		}

		public IEnumerable<int> Versions()
		{
			return _sitecoreItem.Versions;
		}

		/// <summary>
		/// The name of this sitecore item as it would appear in the content tree.
		/// </summary>
		public string Name
		{
			get
			{
				return _sitecoreItem.Name;
			}
		}

		/// <summary>
		/// Returns a collection of the child items of this item or document, in order.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> containing the child items of this item, in order.</returns>
		public virtual IEnumerable<SItem> Items()
		{
			foreach (ISitecoreItem child in _sitecoreItem.Children)
			{
				if (child != null)
				{
					yield return new SItem(child, _itemComposer);
				}
			}
		}

		public IEnumerable<SItem> Items(Func<SItem, bool> filter)
		{
			return Items().Where(filter);
		}

		/// <summary>
		/// Returns a collection of the descendant items for this item, in order.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> containing the descendants of this item, in order.</returns>
		public IEnumerable<SItem> Descendants()
		{
			foreach (SItem item in Items())
			{
				yield return item;

				foreach (SItem descendant in item.Descendants())
				{
					yield return descendant;
				}
			}
		}

		/// <summary>
		/// Returns a collection of items that contain this item, and all descendant items of this item, in order.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> containing the item, and all descendant items of this item, in order.</returns>
		public IEnumerable<SItem> DescendantsAndSelf()
		{
			yield return this;

			foreach (SItem descendant in Descendants())
			{
				yield return descendant;
			}
		}

		/// <summary>
		/// Returns a collection of the ancestor items of this node.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> of the ancestor items of this node.</returns>
		public IEnumerable<SItem> Ancestors()
		{
			SItem ancestor = Parent;

			while (ancestor != null)
			{
				yield return ancestor;

				ancestor = ancestor.Parent;
			}
		}

		/// <summary>
		/// Returns a collection of elements that contain this item, and the ancestors of this item.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> of items that contain this item, and the ancestors of this item.</returns>
		public IEnumerable<SItem> AncestorsAndSelf()
		{
			yield return this;

			foreach (SItem ancestor in Ancestors())
			{
				yield return ancestor;
			}
		}

		/// <summary>
		/// The path of this item in the content tree (not suitable for use as a URL as it is not escaped).
		/// </summary>
		public string Path
		{
			get
			{
				StringBuilder builder = new StringBuilder();

				foreach (SItem item in AncestorsAndSelf())
				{
					builder.Append("/");

					builder.Append(item.Name);
				}

				return builder.ToString();
			}
		}

		/// <summary>
		/// Gets the parent <see cref="SItem"/> of this LINQ item.
		/// </summary>
		public virtual SItem Parent
		{
			get
			{
				ISitecoreItem parent = _sitecoreItem.Parent;

				if (parent != null)
				{
					return new SItem(parent, _itemComposer);
				}

				return null;
			}
		}

		/// <summary>
		/// Gets the <see cref="STemplate"/> that this item is based on.
		/// </summary>
		public STemplate Template
		{
			get
			{
				if (_template == null)
				{
					_template = _itemComposer.CreateTemplate(_sitecoreItem.TemplateKey.Guid.ToString(), _sitecoreItem.TemplateKey.DatabaseName);
				}

				return _template;
			}
		}

		/// <summary>
		/// Converts this LINQ item into a plain old CLR object.
		/// </summary>
		/// <param name="type">The object type to convert to.</param>
		/// <returns>The converted object.</returns>
		public object ToType(Type type)
		{
			object instance = Activator.CreateInstance(type);

			foreach (PropertyInfo property in type.GetProperties())
			{
				SitecoreFieldAttribute fieldAttribute = (SitecoreFieldAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreFieldAttribute));

				if (fieldAttribute != null)
				{
					SField field = Field(fieldAttribute.Name);

					if (field != null)
					{
						object value = field.Value(property.PropertyType, null);

						if (value != null)
						{
							property.SetValue(instance, value, null);
						}
						else
						{
							string s = field.Value<string>(null);

							if (SMedia.IsMediaField(s))
							{
								SMedia media = field.ToMedia();

								if (media != null)
								{
									property.SetValue(instance, media.ToType(property.PropertyType), null);
								}
							}
						}
					}
				}
				else
				{
					SitecoreItemKeyAttribute itemKeyAttribute = (SitecoreItemKeyAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreItemKeyAttribute));

					if (itemKeyAttribute != null)
					{
						property.SetValue(instance, new SitecoreItemKey(Guid, _sitecoreItem.Key.LanguageName, _sitecoreItem.Key.DatabaseName), null);
					}

					SitecoreChildrenAttribute childrenAttribute = (SitecoreChildrenAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreChildrenAttribute));

					if (childrenAttribute != null)
					{
						Type proprtyType = property.PropertyType;

						if (proprtyType.IsGenericType)
						{
							Type genericParameter = proprtyType.GetGenericArguments()[0];

							Type genericCollectionType = typeof(LazyChildrenCollection<>);

							Type collectionType = typeof(LazyChildrenCollection<>).MakeGenericType(new Type[] { genericParameter });

							object collection;

							SitecoreChildTypeAttribute childTypeAttribute = (SitecoreChildTypeAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreChildTypeAttribute));

							if (childTypeAttribute != null)
							{
								if (childTypeAttribute.ChildType != null)
								{
									collection = Activator.CreateInstance(collectionType, new object[] { this, childTypeAttribute.ChildType });
								}
								else
								{
									collection = Activator.CreateInstance(collectionType, new object[] { this, genericParameter });
								}
							}
							else
							{
								collection = Activator.CreateInstance(collectionType, new object[] { this });
							}

							property.SetValue(instance, collection, null);
						}
					}

					SitecoreItemUrlAttribute itemUrlAttribute = (SitecoreItemUrlAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreItemUrlAttribute));

					if (itemUrlAttribute != null)
					{
						property.SetValue(instance, Url, null);
					}
				}
			}

			return instance;
		}

		/// <summary>
		/// Converts this LINQ item into a plain old CLR object.
		/// </summary>
		/// <typeparam name="TType">The object type to convert to.</typeparam>
		/// <returns>The converted object.</returns>
		public TType ToType<TType>()
			where TType : class, new()
		{
			return (TType)ToType(typeof(TType));
		}
	}
}
