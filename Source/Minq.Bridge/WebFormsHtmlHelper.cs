using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Minq.Bridge
{
    public class WebFormsHtmlHelper<TModel>
    {
        private TModel _model;

        public WebFormsHtmlHelper(TModel model)
        {
            _model = model;
        }

        public ViewDataDictionary<TModel> ViewData
        {
            get { return new ViewDataDictionary<TModel>(_model); }
        }
    }
}