using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Initializes the class for use based on the database name and a <see cref="SItemComposer"/>.
		/// </summary>
		/// <param name="name">The name of the database.</param>
		/// <param name="itemComposer">The item composer.</param>
		public SDb(string name, SItemComposer itemComposer)
		{
			_name = name;
			_itemComposer = itemComposer;
		}

		/// <summary>
		/// Gets a media item from the database by key or path in a specific language.
		/// </summary>
		/// <param name="guid">The GUID of the media item to get.</param>
		/// <param name="languageName">The language of the media item to get (e.g. en-GB).</param>
		/// <returns>The media item.</returns>
		public SMedia Media(Guid guid, string languageName)
		{
			return Media(guid.ToString(), languageName);
		}

		/// <summary>
		/// Gets a media item from the database by key or path in a specific language.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the media item to get.</param>
		/// <param name="languageName">The language of the media item to get (e.g. en-GB).</param>
		/// <returns>The item.</returns>
		public SMedia Media(string keyOrPath, string languageName)
		{
			return _itemComposer.CreateMedia(keyOrPath, languageName, _name);
		}

		/// <summary>
		/// Gets an item from the database by GUID in a specific language.
		/// </summary>
		/// <param name="guid">The GUID of the item to get.</param>
		/// <param name="languageName">The language of the item to get (e.g. en-GB).</param>
		/// <returns>The item.</returns>
		public SItem Item(Guid guid, string languageName)
		{
			return Item(guid.ToString(), languageName);
		}

		/// <summary>
		/// Gets an item from the database by key or path in a specific language.
		/// </summary>
		/// <param name="keyOrPath">The key or path of the item to get.</param>
		/// <param name="languageName">The language of the item to get (e.g. en-GB).</param>
		/// <returns>The item.</returns>
		public SItem Item(string keyOrPath, string languageName)
		{
			return _itemComposer.CreateItem(keyOrPath, languageName, _name);
		}

		/// <summary>
		/// Gets an item from the database by searching a list of languages.
		/// </summary>
		/// <param name="guid">The GUID of the item to get.</param>
		/// <param name="languageFallback">The list of languages to search (the first language the item exists in is returned).</param>
		/// <returns>The item that was firts matched in the language fallback list.</returns>
		public SItem Item(Guid guid, IEnumerable<string> languageFallback)
		{
			foreach (string languageName in languageFallback)
			{
				SItem item = Item(guid, languageName);

				if (item != null)
				{
					return item;
				}
			}

			return Item(guid, languageFallback.First());
		}

		/// <summary>
		/// Gets the name of the database (e.g. web, master, core).
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
		}
	}
}
