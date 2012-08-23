using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	public class MockSitecoreContainer : ISitecoreContainer
	{
		private MockSitecoreContext _context;
		private MockSitecoreItemGateway _itemGateway;
		private MockSitecoreTemplateGateway _templateGateway;

		public MockSitecoreContext Context
		{
			get
			{
				if (_context == null)
				{
					_context = new MockSitecoreContext();
				}

				return _context;
			}
		}

		public MockSitecoreItemGateway ItemGateway
		{
			get
			{
				if (_itemGateway == null)
				{
					_itemGateway = new MockSitecoreItemGateway();
				}

				return _itemGateway;
			}
		}

		public MockSitecoreTemplateGateway TemplateGateway
		{
			get
			{
				if (_templateGateway == null)
				{
					_templateGateway = new MockSitecoreTemplateGateway();
				}

				return _templateGateway;
			}
		}

		public TType Resolve<TType>()
		{
			Type type = typeof(TType);

			if (type == typeof(ISitecoreContext))
			{
				return (TType)(object)Context;
			}
			else if (type == typeof(ISitecoreItemGateway))
			{
				return (TType)(object)ItemGateway;
			}
			else if (type == typeof(ISitecoreTemplateGateway))
			{
				return (TType)(object)TemplateGateway;
			}

			throw new NotImplementedException();
		}
	}
}
