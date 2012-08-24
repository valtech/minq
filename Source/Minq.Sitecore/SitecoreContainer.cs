using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minq.Mvc;
using Minq.Sitecore.Mvc;

namespace Minq.Sitecore
{
    public class SitecoreContainer : ISitecoreContainer
    {
        private SitecoreContext _context;
        private SitecoreItemGateway _itemGateway;
		private SitecoreTemplateGateway _templateGateway;
		private SitecoreRendering _rendering;

        private SitecoreContext Context
        {
            get
            {
                if (_context == null)
                {
                    _context = new SitecoreContext();
                }

                return _context;
            }
        }

		private SitecoreRendering Rendering
		{
			get
			{
				if (_rendering == null)
				{
					_rendering = new SitecoreRendering();
				}

				return _rendering;
			}
		}

        private SitecoreItemGateway ItemGateway
        {
            get
            {
                if (_itemGateway == null)
                {
                    _itemGateway = new SitecoreItemGateway();
                }

                return _itemGateway;
            }
        }

		private SitecoreTemplateGateway TemplateGateway
		{
			get
			{
				if (_templateGateway == null)
				{
					_templateGateway = new SitecoreTemplateGateway();
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
			else if (type == typeof(ISitecoreRendering))
			{
				return (TType)(object)Rendering;
			}

            throw new NotImplementedException();
        }
    }
}
