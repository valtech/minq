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
		private ISitecoreItem _item;
		private ISitecoreContainer _container;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreItem"/> and <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="item">The low level Sitecore item that represents this LINQ item.</param>
		/// <param name="container">The Sitecore container.</param>
		public SItem(ISitecoreItem item, ISitecoreContainer container)
		{
			_item = item;
			_container = container;
		}

		/// <summary>
		/// Returns a Sitecore field from the LINQ item.
		/// </summary>
		/// <param name="name">The name of the field.</param>
		/// <returns>A LINQ field.</returns>
		public SField Field(string name)
		{
			ISitecoreField field;

			if (_item.FieldDictionary.TryGetValue(name, out field))
			{
				return new SField(field);
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
				return _item.Key.Guid;
			}
		}

		/// <summary>
		/// Returns a collection of the child items of this item or document, in order.
		/// </summary>
		/// <returns>An <see cref="IEnumerable<T>"/> of <see cref="SItem"/> containing the child items of this item, in order.</returns>
		public IEnumerable<SItem> Items()
		{
			foreach (ISitecoreItem child in _item.Children)
			{
				yield return new SItem(child, _container);
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

		/// <summary>
		/// Gets the parent <see cref="SItem"/> of this LINQ item.
		/// </summary>
		public SItem Parent
		{
			get
			{
				ISitecoreItem parent = _item.Parent;

				if (parent != null)
				{
					return new SItem(parent, _container);
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
				return new STemplate(_container.Resolve<ISitecoreTemplateGateway>().GetTemplate(_item.TemplateKey));
			}
		}

		/// <summary>
		/// Converts this LINQ item into a plain old CLR object.
		/// </summary>
		/// <typeparam name="TType">The object type to convert to.</typeparam>
		/// <returns>The converted object.</returns>
		public TType Poco<TType>()
			where TType : class, new()
		{
			TType instance = new TType();

			Type type = instance.GetType();

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
					}
				}
				else
				{
					SitecoreItemKeyAttribute itemKeyAttribute = (SitecoreItemKeyAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreItemKeyAttribute));

					if (itemKeyAttribute != null)
					{
						property.SetValue(instance, new SitecoreItemKey(Guid, _container.Resolve<ISitecoreContext>()), null);
					}

					SitecoreChildrenAttribute childrenAttribute = (SitecoreChildrenAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreChildrenAttribute));

					if (childrenAttribute != null)
					{
						Type proprtyType = property.PropertyType;

						if (proprtyType.IsGenericType)
						{
							Type genericParameter = proprtyType.GetGenericArguments()[0];

							Type genericCollectionType = typeof(PocoCollection<>);

							Type collectionType = genericCollectionType.MakeGenericType(new Type[] { genericParameter });

							object collection = Activator.CreateInstance(collectionType, new object[] { this });

							property.SetValue(instance, collection, null);
						}
					}
				}
			}

			return instance;
		}

		#region PocoCollection
		sealed class PocoCollection<TChild> : ICollection<TChild>
			where TChild : class, new()
		{
			private SItem _item;
			private IList<TChild> _children;

			public PocoCollection(SItem item)
			{
				_item = item;
			}

			private IList<TChild> Children
			{
				get
				{
					if (_children == null)
					{
						_children = new List<TChild>(_item.Items().Select(item => item.Poco<TChild>()));
					}

					return _children;
				}
			}

			public void Add(TChild item)
			{
				throw new NotImplementedException();
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains(TChild item)
			{
				throw new NotImplementedException();
			}

			public void CopyTo(TChild[] array, int arrayIndex)
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

			public bool Remove(TChild item)
			{
				throw new NotImplementedException();
			}

			public IEnumerator<TChild> GetEnumerator()
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
