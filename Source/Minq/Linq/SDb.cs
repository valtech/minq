using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents a Sitecore LINQ database.
	/// </summary>
	public class SDb
	{
		private string _name;
		private ISitecoreContainer _container;

		/// <summary>
		/// Initializes the class for use based on the database name and a <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="name">The name of the database.</param>
		/// <param name="container">The Sitecore container.</param>
		public SDb(string name, ISitecoreContainer container)
		{
			_name = name;
			_container = container;
		}

		public SItem Item(Guid guid)
		{
			ISitecoreContext context = _container.Resolve<ISitecoreContext>();

			ISitecoreItemGateway itemGateway = _container.Resolve<ISitecoreItemGateway>();

			return new SItem(itemGateway.GetItem(new SitecoreItemKey(guid, context.LanguageName, _name)), _container);
		}

		public SItem Item(string keyOrPath)
		{
			ISitecoreContext context = _container.Resolve<ISitecoreContext>();

			ISitecoreItemGateway itemGateway = _container.Resolve<ISitecoreItemGateway>();

			return new SItem(itemGateway.GetItem(keyOrPath, context.LanguageName, _name), _container);
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
