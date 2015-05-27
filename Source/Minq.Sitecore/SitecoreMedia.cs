using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScapiMediaItem = global::Sitecore.Data.Items.MediaItem;
using ScapiMediaManager = Sitecore.Resources.Media.MediaManager;
using ScapiMediaUrlOptions = Sitecore.Resources.Media.MediaUrlOptions;

namespace Minq.Sitecore
{
	public class SitecoreMedia : ISitecoreMedia
	{
		private ScapiMediaItem _scapiMediaItem;

		public SitecoreMedia(ScapiMediaItem scapiMediaItem)
		{
			_scapiMediaItem = scapiMediaItem;
		}

		public string Url
		{
			get
			{
				return ScapiMediaManager.GetMediaUrl(_scapiMediaItem);
			}
		}

		public SitecoreItemKey Key
		{
			get
			{
				return new SitecoreItemKey(_scapiMediaItem.ID.Guid, _scapiMediaItem.InnerItem.Language.Name, _scapiMediaItem.Database.Name);
			}
		}

		public string AlternateText
		{
			get
			{
				return _scapiMediaItem.Alt;
            }
		}
	}
}
