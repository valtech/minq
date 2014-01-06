using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

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
		///  Initializes the class for use based on a <see cref="ISitecoreItem"/> and <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="sitecoreItem">The low level Sitecore item that represents this LINQ item.</param>
		/// <param name="container">The Sitecore container.</param>
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

			return null;
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

		public static bool IsNullOrUnversioned(SItem item)
		{
			return item == null || !item.Versions().Any();
		}

		public IEnumerable<int> Versions()
		{
			return _sitecoreItem.Versions;
		}

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
		public IEnumerable<SItem> Items()
		{
			foreach (ISitecoreItem child in _sitecoreItem.Children)
			{
				yield return new SItem(child, _itemComposer);
			}
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
		public SItem Parent
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
					_template = _itemComposer.CreateTemplate(_sitecoreItem.TemplateKey);
				}

				return _template;
			}
		}

		/// <summary>
		/// Converts this LINQ item into a plain old CLR object.
		/// </summary>
		/// <typeparam name="T">The object type to convert to.</typeparam>
		/// <returns>The converted object.</returns>
		public T ToType<T>()
			where T : class, new()
		{
			T instance = new T();

			Type type = typeof(T);

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
								property.SetValue(instance, _itemComposer.CreateMedia(field, LanguageName, Db.Name).ToType(property.PropertyType), null);
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

							Type genericCollectionType = typeof(TypeCollection<>);

							Type collectionType = typeof(TypeCollection<>).MakeGenericType(new Type[] { genericParameter });

							object collection = Activator.CreateInstance(collectionType, new object[] { this });

							property.SetValue(instance, collection, null);
						}
					}
				}
			}

			return instance;
		}

		#region TypeCollection
		sealed class TypeCollection<T> : ICollection<T>
			where T : class, new()
		{
			private SItem _item;
			private IList<T> _children;

			public TypeCollection(SItem item)
			{
				_item = item;
			}

			private IList<T> Children
			{
				get
				{
					if (_children == null)
					{
						_children = new List<T>(_item.Items().Select(item => item.ToType<T>()));
					}

					return _children;
				}
			}

			public void Add(T item)
			{
				throw new NotImplementedException();
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains(T item)
			{
				throw new NotImplementedException();
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			public int Count
			{
				get
				{
					return Children.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			public bool Remove(T item)
			{
				throw new NotImplementedException();
			}

			public IEnumerator<T> GetEnumerator()
			{
				return Children.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
		#endregion
	}
}
