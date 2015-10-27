using System;

namespace Minq
{
	/// <summary>
	/// Defines an object that uniquely identifies a Sitecore template.
	/// </summary>
	public class SitecoreTemplateKey
	{
		private Guid _guid;
		private string _databaseName;

		/*
		/// <summary>
		/// Initializes the class for use based on a Sitecore item guid and the language/database in the given <see cref="ISitecoreContext"/>.
		/// </summary>
		/// <param name="guid">The Sitecore item's guid.</param>
		/// <param name="context">A Sitecore context.</param>
		public SitecoreTemplateKey(Guid guid, ISitecoreContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			_guid = guid;
			_databaseName = context.DatabaseName;
		}
		*/

		/// <summary>
		/// Initializes the class for use based on a Sitecore item guid, language and database.
		/// </summary>
		/// <param name="guid">The Sitecore item's guid.</param>
		/// <param name="databaseName">The Sitecore item's database.</param>
		public SitecoreTemplateKey(Guid guid, string databaseName)
		{
			if (databaseName == null)
			{
				throw new ArgumentNullException("databaseName");
			}

			_guid = guid;
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
		/// Gets the Sitecore item's guid associated with this key.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _guid;
			}
		}

		/// <summary>
		/// Determines if two template keys are equal.
		/// </summary>
		/// <param name="a">The first key to compare.</param>
		/// <param name="b">THe second key to compare.</param>
		/// <returns>true if the value of a is the same as the value of b; otherwise, false.</returns>
		public static bool operator ==(SitecoreTemplateKey a, SitecoreTemplateKey b)
		{
			if (Object.ReferenceEquals(a, null))
			{
				return Object.ReferenceEquals(b, null);
			}
			else
			{
				if (Object.ReferenceEquals(b, null))
				{
					return false;
				}
				else
				{
					return a._guid == b._guid
						&& String.Equals(a._databaseName, b._databaseName, StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		/// <summary>
		/// Determines if two template keys have different values.
		/// </summary>
		/// <param name="a">The first key to compare.</param>
		/// <param name="b">THe second key to compare.</param>
		/// <returns>true if the value of a different to the value of b; otherwise, false.</returns>
		public static bool operator !=(SitecoreTemplateKey a, SitecoreTemplateKey b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Returns a value indicating whether two <see cref="SitecoreTemplateKey"/> instances have the same date and time value.
		/// </summary>
		/// <param name="o">The object to compare.</param>
		/// <returns>true if the value of o is the same; otherwise, false.</returns>
		public override bool Equals(object o)
		{
			return this == o as SitecoreTemplateKey;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return _guid.GetHashCode() ^ _databaseName.GetHashCode();
		}

		/// <summary>
		/// Returns the string representation of this <see cref="SitecoreTemplateKey"/>.
		/// </summary>
		/// <returns>A string representing this <see cref="SitecoreTemplateKey"/>.</returns>
		public override string ToString()
		{
			return String.Format("Guid = {0}, Database = {1}", _guid, _databaseName);
		}
	}
}
