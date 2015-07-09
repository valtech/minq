using System;

namespace Minq.Mocks
{
	public class MockSitecorePageMode : ISitecorePageMode
	{
		private bool _isNormal;
		private bool _isPageEditor;
		private bool _isPreview;
		private bool _isDebug;

		public MockSitecorePageMode()
		{
			EnterNormalMode();
		}

		public void EnterPageEditorMode()
		{
			_isPageEditor = true;
			_isNormal = false;
			_isPreview = false;
			_isDebug = false;
		}

		public void EnterNormalMode()
		{
			_isPageEditor = false;
			_isNormal = true;
			_isPreview = false;
			_isDebug = false;
		}

		public bool IsNormal
		{
			get
			{
				return _isNormal;
			}
		}

		public bool IsPageEditor
		{
			get
			{
				return _isPageEditor;
			}
		}

		public bool IsPreview
		{
			get
			{
				return _isPreview;
			}
		}

		public bool IsDebug
		{
			get
			{
				return _isDebug;
			}
		}
	}
}
