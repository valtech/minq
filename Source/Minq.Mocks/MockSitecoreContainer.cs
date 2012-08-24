using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mocks.Mvc;
using Minq.Mvc;

namespace Minq.Mocks
{
	public class MockSitecoreContainer : ISitecoreContainer
	{
		private MockSitecoreContext _context;
		private MockSitecoreItemGateway _itemGateway;
		private MockSitecoreTemplateGateway _templateGateway;
		private MockSitecoreRendering _rendering;

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

		public MockSitecoreRendering Rendering
		{
			get
			{
				if (_rendering == null)
				{
					_rendering = new MockSitecoreRendering();
				}

				return _rendering;
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
			set
			{
				_templateGateway = value;
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
			else if (type == typeof(ISitecoreRendering))
			{
				return (TType)(object)Rendering;
			}

			throw new NotImplementedException();
		}
	}
}
