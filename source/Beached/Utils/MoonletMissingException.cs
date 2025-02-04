using System;

namespace Beached.Utils
{
	public class MoonletMissingException : Exception
	{
		public MoonletMissingException() : base("Moonlet is a required API to run Beached. Please install and enable it.")
		{
		}
	}
}
