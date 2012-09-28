using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Minq.Mvc;

namespace Minq.Bridge
{
	public abstract class WebFormsControllerFactory 
	{
		public abstract TController CreateController<TController>() where TController : Controller;

		public abstract ISitecoreMarkupStrategy CreateSitecoreMarkupStrategy();
	}
}
