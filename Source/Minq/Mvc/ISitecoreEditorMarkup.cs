using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mvc
{
	public interface ISitecoreEditorMarkup
	{
		string GetHtml(string childContent);
	}
}
