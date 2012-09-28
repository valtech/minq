using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Linq
{
	/// <summary>
	/// Defines an object that represents a Sitecore LINQ database.
	/// </summary>
	public class SDb
	{
		private string _name;
		private SItemComposer _itemComposer;

		/// <summary>
		/// Initializes the class for use based on the database name and a <see cref="ISitecoreContainer"/>.
		/// </summary>
		/// <param name="name">The name of the database.</param>
		/// <param name="container">The Sitecore container.</param>
		public SDb(string name, SItemComposer itemComposer)
		{
			_name = name;
			_itemComposer = itemComposer;
		}

		public SMedia Media(Guid guid, string languageName)
		{
			return Media(guid.ToString(), languageName);
		}

		public SMedia Media(string keyOrPath, string languageName)
		{
			return _itemComposer.CreateMedia(keyOrPath, languageName, _name);
		}

		public SItem Item(Guid guid, string languageName)
		{
			return Item(guid.ToString(), languageName);
		}

		public SItem Item(string keyOrPath, string languageName)
		{
			return _itemComposer.CreateItem(keyOrPath, languageName, _name);
		}

		public string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
