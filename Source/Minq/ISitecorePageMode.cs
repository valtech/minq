using System;

namespace Minq
{
	/// <summary>
	/// Defines an object that represents the current page request mode.
	/// </summary>
	public interface ISitecorePageMode
	{
		/// <summary>
		/// Gets if the page is being requested from the content delivery site normally or not.
		/// </summary>
		bool IsNormal
		{
			get;
		}

		/// <summary>
		/// Gets if the page is being edited using the page editor on the content authoring platform or not.
		/// </summary>
		bool IsPageEditor
		{
			get;
		}

		/// <summary>
		/// Gets if the page is being previewed on the content authoring platform or not.
		/// </summary>
		bool IsPreview
		{
			get;
		}

		/// <summary>
		/// Gets if a developer is viewing the page in debug mode or not.
		/// </summary>
		bool IsDebug
		{
			get;
		}
	}
}
