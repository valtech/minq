using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Minq
{
	public class SitecoreUrl
	{
		private Uri _sitecoreUrl;
		private HttpContextBase _context;

		public SitecoreUrl(string sitecoreUrl)
		{
			_sitecoreUrl = new Uri(sitecoreUrl);

			HttpContext context = HttpContext.Current;

			if (context != null)
			{
				_context = new HttpContextWrapper(context);
			}
		}

		public SitecoreUrl(Uri sitecoreUrl)
		{
			_sitecoreUrl = sitecoreUrl;

			HttpContext context = HttpContext.Current;

			if (context != null)
			{
				_context = new HttpContextWrapper(context);
			}
		}

		public SitecoreUrl(Uri sitecoreUrl, HttpContextBase context)
		{
			_sitecoreUrl = sitecoreUrl;
			_context = context;
		}

		public SitecoreUrl(Uri sitecoreUrl, HttpContext context)
		{
			_sitecoreUrl = sitecoreUrl;

			if (context != null)
			{
				_context = new HttpContextWrapper(context);
			}
		}

		public SitecoreUrl For(HttpContextBase context)
		{
			return new SitecoreUrl(_sitecoreUrl, context);
		}

		public SitecoreUrl For(HttpContext context)
		{
			return new SitecoreUrl(_sitecoreUrl, new HttpContextWrapper(context));
		}

		/// <summary>
		/// Get the URL relative to the current request i.e. if the current page is http://www.example.com:8080/folder1/page.aspx and the Sitecore URL is
		/// http://www.example.com:8080/folder2/page.aspx you will get ../folder2/page.aspx.
		/// </summary>
		public string Relative
		{
			get
			{
				if (_context == null)
				{
					throw new Exception("Cannot determine relative URL as there is no HTTP context associated with this URL use url.For(HttpContext).Relative");
				}

				HttpRequestBase request = _context.Request;

				string authority = _sitecoreUrl.GetLeftPart(UriPartial.Authority);

				if (String.Equals(authority, request.Url.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
				{
					return request.Url.MakeRelativeUri(_sitecoreUrl).ToString();
				}

				if (String.Equals(_sitecoreUrl.Host, request.Url.Host))
				{
					return Virtual;
				}

				return _sitecoreUrl.ToString();
			}
		}

		/// <summary>
		/// Gets the URL ensuring that any port mapping is stripped from the URL i.e. http://www.example.com:8080/ becomes http://www.example.com/
		/// </summary>
		public string Absolute
		{
			get
			{
				string authority = _sitecoreUrl.GetLeftPart(UriPartial.Authority);

				string relativeToRoot = new Uri(authority).MakeRelativeUri(_sitecoreUrl).ToString();

				return _sitecoreUrl.Scheme + "://" + _sitecoreUrl.Host + "/" + relativeToRoot;
			}
		}

		/// <summary>
		/// Gets the raw URL without modification
		/// </summary>
		public string Raw
		{
			get
			{
				return _sitecoreUrl.ToString();
			}
		}

		/// <summary>
		/// Gets the URL ensuring that the scheme, host and port are stripped from the URL i.e. http://www.example.com:8080/page.aspx becomes /page.aspx
		/// </summary>
		public string Virtual
		{
			get
			{
				string authority = _sitecoreUrl.GetLeftPart(UriPartial.Authority);

				return "/" + new Uri(authority).MakeRelativeUri(_sitecoreUrl).ToString();
			}
		}

		public override string ToString()
		{
			return Relative;
		}

		public static implicit operator string(SitecoreUrl url)
		{
			return url.ToString();
		}

		public static implicit operator SitecoreUrl(string url)
		{
			return new SitecoreUrl(url);
		}
	}
}
