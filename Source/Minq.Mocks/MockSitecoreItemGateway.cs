using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	/// <summary>
	/// Provides a unit testable version of the <see ref="ISitecoreItemGateway" /> interface.
	/// </summary>
	public class MockSitecoreItemGateway : ISitecoreItemGateway
	{
		private IDictionary<SitecoreItemKey, ISitecoreItem> _items = new Dictionary<SitecoreItemKey, ISitecoreItem>(new SitecoreItemKeyComparer());

		/// <summary>
		/// Adds an item to this mock Sitecore gateway's internal repository.
		/// </summary>
		/// <param name="item">The child item to add.</param>
		public void AddItem(ISitecoreItem item)
		{
			_items[item.Key] = item;
		}

		/// <summary>
		/// Gets an item from this mock Sitecore gateway's internal repository.
		/// </summary>
		/// <param name="key">The <see cref="SitecoreItemKey" /> unqiuely identifying the item to return.</param>
		/// <returns>A <see cref="ISitecoreItem" />.</returns>
		public ISitecoreItem GetItem(string keyOrPath, string languageName, string databaseName)
		{
			ISitecoreItem item;

			Guid guid;

			if (Guid.TryParse(keyOrPath, out guid))
			{
				if (_items.TryGetValue(new SitecoreItemKey(guid, languageName, databaseName), out item))
				{
					return item;
				}
				else
				{
					return null;
				}
				//else
				//{
				//	throw new MockSitecoreItemGatewayException(String.Format("Sitecore item {0} does not exist", keyOrPath));
				//}
			}
			else
			{
				throw new MockSitecoreItemGatewayException(String.Format("Path lookup not supported in mocking yet {0}", keyOrPath));
			}
		}
	}
}
