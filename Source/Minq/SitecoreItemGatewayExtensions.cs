using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;

namespace Minq
{
	/// <summary>
	/// Provides a set of static methods for querying objects that implement <see cref="ISitecoreItemGateway" />.
	/// </summary>
	public static class SitecoreItemGatewayExtensions
	{
		private static TType GetItem<TType>(this ISitecoreItemGateway source, ISitecoreContainer container, ISitecoreItem item)
			where TType : class, new()
		{
			TType instance = new TType();

			Type type = instance.GetType();

			foreach (PropertyInfo property in type.GetProperties())
			{
				SitecoreFieldAttribute fieldAttribute = (SitecoreFieldAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreFieldAttribute));

				if (fieldAttribute != null)
				{
					ISitecoreField field;

					if (item.FieldDictionary.TryGetValue(fieldAttribute.Name, out field))
					{
						object value;

						if (field.TryConvertValue(property.PropertyType, out value))
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
						property.SetValue(instance, item.Key, null);
					}
					else
					{
						SitecoreChildrenAttribute childrenAttribute = (SitecoreChildrenAttribute)Attribute.GetCustomAttribute(property, typeof(SitecoreChildrenAttribute));

						if (childrenAttribute != null)
						{
							Type proprtyType = property.PropertyType;

							if (proprtyType.IsGenericType)
							{
								Type genericParameter = proprtyType.GetGenericArguments()[0];

								Type genericCollectionType = typeof(GatewayChildren<>);

								Type collectionType = genericCollectionType.MakeGenericType(new Type[] { genericParameter });

								object collection = Activator.CreateInstance(collectionType, new object[] { container, item });

								property.SetValue(instance, collection, null);
							}
						}
					}
				}
			}

			return instance;
		}

		public static TType GetItem<TType>(this ISitecoreItemGateway source, ISitecoreContainer container, SitecoreItemKey key)
			where TType : class, new()
		{
			ISitecoreItem item = source.GetItem(key);

			if (item != null)
			{
				return source.GetItem<TType>(container, item);
			}

			throw new Exception("Could not find " + key);
		}

		sealed class GatewayChildren<TChild> : ICollection<TChild>
			where TChild : class, new()
		{
			private ISitecoreContainer _container;
			private ISitecoreItemGateway _itemGateway;
			private ISitecoreItem _item;
			private IList<TChild> _children;

			public GatewayChildren(ISitecoreContainer container, ISitecoreItem item)
			{
				_container = container;
				_itemGateway = container.Resolve<ISitecoreItemGateway>();
				_item = item;
			}

			private IList<TChild> Children
			{
				get
				{
					if (_children == null)
					{
						_children = new List<TChild>(_item.Children.Select(item => _itemGateway.GetItem<TChild>(_container, item)));
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
	}
}
