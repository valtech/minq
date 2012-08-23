using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Minq
{
	/// <summary>
	/// Defines an object that uniquely identifies a Sitecore item.
	/// </summary>
	public class SitecoreItemKey
	{
		private Guid _guid;
		private string _databaseName;
		private string _languageName;

		/// <summary>
		/// Initializes the class for use based on a Sitecore item guid and the language/database in the given <see cref="ISitecoreContext"/>.
		/// </summary>
		/// <param name="guid">The Sitecore item's guid.</param>
		/// <param name="context">A Sitecore context.</param>
		public SitecoreItemKey(Guid guid, ISitecoreContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			_guid = guid;
			_languageName = context.LanguageName;
			_databaseName = context.DatabaseName;
		}

		/// <summary>
		/// Initializes the class for use based on a Sitecore item guid, language and database.
		/// </summary>
		/// <param name="guid">The Sitecore item's guid.</param>
		/// <param name="languageName">The Sitecore item's language.</param>
		/// <param name="databaseName">The Sitecore item's database.</param>
		public SitecoreItemKey(Guid guid, string languageName, string databaseName)
		{
			if (languageName == null)
			{
				throw new ArgumentNullException("languageName");
			}

			if (databaseName == null)
			{
				throw new ArgumentNullException("databaseName");
			}

			_guid = guid;
			_languageName = languageName;
			_databaseName = databaseName;
		}

		/// <summary>
		/// Gets the Sitecore item's database associated with this key.
		/// </summary>
		public string DatabaseName
		{
			get
			{
				return _databaseName;
			}
		}

		/// <summary>
		/// Gets the Sitecore item's language associated with this key.
		/// </summary>
		public string LanguageName
		{
			get
			{
				return _languageName;
			}
		}

		/// <summary>
		/// Gets the Sitecore item's guid associated with this key.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _guid;
			}
		}

		public static bool operator ==(SitecoreItemKey a, SitecoreItemKey b)
		{
			return (object)a != null && (object)b != null && a._guid == b._guid
				&& String.Equals(a._databaseName, b._databaseName, StringComparison.OrdinalIgnoreCase)
				&& String.Equals(a._languageName, b._languageName, StringComparison.OrdinalIgnoreCase);
		}

		public static bool operator !=(SitecoreItemKey a, SitecoreItemKey b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="SitecoreItemKey"/> instances have the same date and time value.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public override bool Equals(object o)
		{
			return this == o as SitecoreItemKey;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _guid.GetHashCode() ^ _languageName.GetHashCode() ^ _databaseName.GetHashCode();
		}

		/// <summary>
		/// Returns the string representation of this <see cref="SitecoreItemKey"/>.
		/// </summary>
		/// <returns>A string representing this <see cref="SitecoreItemKey"/>.</returns>
		public override string ToString()
		{
			return String.Format("Guid = {0}, Language = {1}, Database = {2}", _guid, _languageName, _databaseName);
		}
	}
}
