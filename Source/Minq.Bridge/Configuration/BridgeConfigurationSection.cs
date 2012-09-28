using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Minq.Bridge.Configuration
{
	internal class BridgeConfigurationSection : ConfigurationSection
	{
		private static ConfigurationProperty _controllerFactoryProperty;
		private static ConfigurationPropertyCollection _properties;

		static BridgeConfigurationSection()
		{
			_properties = new ConfigurationPropertyCollection();

			_controllerFactoryProperty = new ConfigurationProperty("controllerFactory", typeof(string));

			_properties.Add(_controllerFactoryProperty);
		}

		[ConfigurationProperty("controllerFactory")]
		public string ControllerFactory
		{
			get
			{
				return (string)base[_controllerFactoryProperty];
			}
		}
	}
}