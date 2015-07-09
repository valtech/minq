using System;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents access to the Sitecore media library.
	/// </summary>
	public interface ISitecoreMediaGateway
	{
		/// <summary>
		/// When overridden in a derived class, returns the Sitecore media item for the given key or path.
		/// </summary>
		/// <param name="keyOrPath">The key or path identifying the media to return.</param>
		/// <param name="languageName">The language of the media to return.</param>
		/// <param name="databaseName">Thedatabase of the media to return.</param>
		/// <returns>A <see cref="ISitecoreMedia" />.</returns>
		ISitecoreMedia GetMedia(string keyOrPath, string languageName, string databaseName);
	}
}
