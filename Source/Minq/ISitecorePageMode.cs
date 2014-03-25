using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq
{
	public interface ISitecorePageMode
	{
		bool IsNormal
		{
			get;
		}

		bool IsPageEditor
		{
			get;
		}

		bool IsPreview
		{
			get;
		}

		bool IsDebug
		{
			get;
		}
	}
}
