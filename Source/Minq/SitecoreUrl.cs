using System;
using System.Web;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents a URL (typically but not necessarily to Sitecore content).
	/// </summary>
	public class SitecoreUrl
	{
		private const string JavaScriptPrefix = "javascript:";

		private Uri _sitecoreUrl;
		private HttpContextBase _context;
		private string _javaScript;

		/// <summary>
		/// Creates a new URL from the supplied path. 
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		public SitecoreUrl(string sitecoreUrl)
			: this(sitecoreUrl, TryGetContext())
		{
			
		}

		/// <summary>
		/// Creates a new URL from the supplied path using HTTP context information to determine the missing left part.
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		/// <param name="context">The HTTP context.</param>
		public SitecoreUrl(string sitecoreUrl, HttpContext context)
			: this(sitecoreUrl, TryGetContext(context))
		{
		}

		/// <summary>
		/// Creates a new URL from the supplied path. 
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		public SitecoreUrl(Uri sitecoreUrl)
			: this(sitecoreUrl, TryGetContext())
		{
			
		}

		/// <summary>
		/// Creates a new URL from the supplied path using HTTP context information to determine the missing left part.
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		/// <param name="context">The HTTP context.</param>
		public SitecoreUrl(Uri sitecoreUrl, HttpContext context)
			: this(sitecoreUrl, TryGetContext(context))
		{

		}

		/// <summary>
		/// Creates a new URL from the supplied path using HTTP context information to determine the missing left part.
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		/// <param name="context">The HTTP context.</param>
		public SitecoreUrl(string sitecoreUrl, HttpContextBase context)
		{
			if (!String.IsNullOrEmpty(sitecoreUrl))
			{
				if (sitecoreUrl.StartsWith(JavaScriptPrefix, StringComparison.OrdinalIgnoreCase))
				{
					_javaScript = sitecoreUrl.Substring(JavaScriptPrefix.Length);
				}
				else
				{
					if (sitecoreUrl == "/" || sitecoreUrl.StartsWith("/"))
					{
						if (context != null)
						{
							string authority = context.Request.Url.GetLeftPart(UriPartial.Authority);

							sitecoreUrl = authority + sitecoreUrl;
						}
					}

					_sitecoreUrl = new Uri(sitecoreUrl);

					_context = context;
				}
			}
		}

		/// <summary>
		/// Creates a new URL from the supplied path using HTTP context information to determine the missing left part.
		/// </summary>
		/// <param name="sitecoreUrl">The path to the URL.</param>
		/// <param name="context">The HTTP context.</param>
		public SitecoreUrl(Uri sitecoreUrl, HttpContextBase context)
		{
			_sitecoreUrl = sitecoreUrl;
			_context = context;
		}

		private static HttpContextBase TryGetContext()
		{
			HttpContext context = HttpContext.Current;

			if (context != null)
			{
				return new HttpContextWrapper(context);
			}

			return null;
		}

		private static HttpContextBase TryGetContext(HttpContext context)
		{
			if (context != null)
			{
				return new HttpContextWrapper(context);
			}

			return null;
		}

		/// <summary>
		/// Get the URL relative to the current request i.e. if the current page is http://www.example.com:8080/folder1/page.aspx and the Sitecore URL is
		/// http://www.example.com:8080/folder2/page.aspx you will get ../folder2/page.aspx.
		/// </summary>
		public string Relative
		{
			get
			{
				if (_sitecoreUrl == null)
				{
					if (IsJavaScript)
					{
						return JavaScriptUrl();
					}

					return "";
				}

				if (_context == null)
				{
					throw new Exception("Cannot determine relative URL as there is no HTTP context associated with this URL.");
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
				if (_sitecoreUrl == null)
				{
					if (IsJavaScript)
					{
						return JavaScriptUrl();
					}

					return "";
				}

				string authority = _sitecoreUrl.GetLeftPart(UriPartial.Authority);

				string relativeToRoot = new Uri(authority).MakeRelativeUri(_sitecoreUrl).ToString();

				return _sitecoreUrl.Scheme + "://" + _sitecoreUrl.Host + "/" + relativeToRoot;
			}
		}

		/// <summary>
		/// Gets the URL ensuring that the scheme, host and port are stripped from the URL i.e. http://www.example.com:8080/page.aspx becomes /page.aspx
		/// </summary>
		public string Virtual
		{
			get
			{
				if (_sitecoreUrl == null)
				{
					if (IsJavaScript)
					{
						return JavaScriptUrl();
					}

					return "";
				}

				string authority = _sitecoreUrl.GetLeftPart(UriPartial.Authority);

				if (_context != null)
				{
					HttpRequestBase request = _context.Request;

					bool differentAuthority = !String.Equals(authority, request.Url.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase);
					
					if (differentAuthority)
					{
						return Absolute;
					}

					return "/" + new Uri(authority).MakeRelativeUri(_sitecoreUrl).ToString();
				}
				else
				{
					return Absolute;
				}
			}
		}

		/// <summary>
		/// Returns the virtual URL as a string.
		/// </summary>
		/// <returns>The URL in virtual URL representation.</returns>
		public override string ToString()
		{
			return Virtual;
		}

		/// <summary>
		/// Converts the given Sitecore URL to a string. A virtual URL is used by default.
		/// </summary>
		/// <returns>The URL in virtual URL representation.</returns>
		public static implicit operator string(SitecoreUrl url)
		{
			if (url == null)
			{
				return null;
			}

			return url.ToString();
		}

		/// <summary>
		/// Converts the given string to a Sitecore URL.
		/// </summary>
		/// <returns>A Sitecore URL.</returns>
		public static implicit operator SitecoreUrl(string url)
		{
			return new SitecoreUrl(url);
		}

		private string JavaScriptUrl()
		{
			return JavaScriptPrefix + _javaScript;
		}

		/// <summary>
		/// true is this link is inline JavaScript; false otherwise
		/// </summary>
		public bool IsJavaScript
		{
			get
			{
				return _javaScript != null;
			}
		}
	}
}
