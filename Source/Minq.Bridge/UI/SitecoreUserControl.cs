using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using Minq.Mvc;
using Minq.Bridge.Configuration;

namespace Minq.Bridge.UI
{
    public class SitecoreUserControl<TController, TModel> : UserControl where TController : Controller
    {
        private Func<TController, Func<ActionResult>> _action;
        private TModel _model;
		private SitecoreHelper<TModel> _html;

		public SitecoreUserControl(Func<TController, Func<ActionResult>> action)
        {
            _action = action;
        }

		public SitecoreHelper<TModel> Html
        {
            get { return _html; }
        }

        public TModel Model
        {
            get { return _model; }
        }

		private WebFormsControllerFactory ControllerFactory
		{
			get
			{
				return BridgeConfigurationManager.ControllerFactory;
			}
		}

        protected override void OnInit(EventArgs e)
        {
			TController controller = ControllerFactory.CreateController<TController>();

			RequestContext context = new RequestContext();

			context.HttpContext = new HttpContextWrapper(HttpContext.Current);

			controller.ControllerContext = new ControllerContext(context, controller);

			Func<ActionResult> runner = _action(controller);

			ActionResult result = runner();

			ViewResult viewResult = result as ViewResult;

			if (viewResult != null)
			{
				_model = (TModel)viewResult.Model;

				_html = new SitecoreHelper<TModel>(
					new ViewDataDictionary<TModel>(_model), ControllerFactory.CreateSitecoreMarkupStrategy());
			}

			base.OnInit(e);
        }
    }
}
