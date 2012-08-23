using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents the processing context for request-level operations in Sitecore via LINQ.
	/// </summary>
	public class SContext
	{
		private ISitecoreContainer _container;

		/// <summary>
		///  Initializes the class for use based on a <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="container"></param>
		public SContext(ISitecoreContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the Sitecore LINQ item for the current HTTP request.
		/// </summary>
		public SItem Item
		{
			get
			{
				ISitecoreContext context = _container.Resolve<ISitecoreContext>();

				ISitecoreItemGateway itemGateway = _container.Resolve<ISitecoreItemGateway>();

				return new SItem(itemGateway.GetItem(context.ItemKey), _container);
			}
		}

		/// <summary>
		/// Gets the Sitecore LINQ database for the current HTTP request.
		/// </summary>
		public SDb Db
		{
			get
			{
				ISitecoreContext context = _container.Resolve<ISitecoreContext>();

				return new SDb(context.DatabaseName, _container);
			}
		}
	}
}
