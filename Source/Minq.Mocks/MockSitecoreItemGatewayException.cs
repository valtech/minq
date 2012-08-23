using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minq.Mocks
{
	/// <summary>
	/// The exception that is thrown when the is a problem with the <see cref="MockSitecoreItemGateway" />.
	/// </summary>
	public class MockSitecoreItemGatewayException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MockSitecoreItemGatewayException" /> class with a specified error message.
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public MockSitecoreItemGatewayException(string message)
			: base(message)
		{

		}
	}
}
