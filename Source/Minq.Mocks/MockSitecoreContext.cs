using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	/// <summary>
	/// Provides a unit testable version of the <see ref="ISitecoreContext" /> interface.
	/// </summary>
	public class MockSitecoreContext : ISitecoreContext
	{
		private string _languageName;
		private string _databaseName;
		
		/// <summary>
		/// Gets or sets the NLS culture name for the mocked Sitecore context.
		/// </summary>
		public string LanguageName
		{
			get
			{
				if (String.IsNullOrEmpty(_languageName))
				{
					return "en-GB";
				}

				return _languageName;
			}
			set
			{
				_languageName = value;
			}
		}

		/// <summary>
		/// Gets or sets the Sitecore database used as the default item repository for the mocked Sitecore context.
		/// </summary>
		public string DatabaseName
		{
			get
			{
				if (String.IsNullOrEmpty(_databaseName))
				{
					return "web";
				}

				return _databaseName;
			}
			set
			{
				_databaseName = value;
			}
		}

		public SitecoreItemKey ItemKey
		{
			get;
			set;
		}
	}
}
