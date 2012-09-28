using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Minq.Bridge.Configuration
{
	internal class BridgeConfigurationManager
	{
		private static readonly WebFormsControllerFactory WebFormsControllerFactorySingleton = GetWebFormsControllerFactory();

		private static WebFormsControllerFactory GetWebFormsControllerFactory()
		{
			Type type = Type.GetType(GetSection().ControllerFactory, true);

			return (WebFormsControllerFactory)Activator.CreateInstance(type);
		}

		public static WebFormsControllerFactory ControllerFactory
		{
			get
			{
				return WebFormsControllerFactorySingleton;
			}
		}

		private static BridgeConfigurationSection GetSection()
		{
			BridgeConfigurationSection section = ConfigurationManager.GetSection("minq.bridge") as BridgeConfigurationSection;

			if (section == null)
			{
				throw new Exception("Could not find section 'trueClarity/pocohontas'.");
			}

			return section;
		}
	}
}
