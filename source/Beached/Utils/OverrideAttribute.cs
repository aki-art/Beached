using System;

namespace Beached.Utils
{
	public class OverrideAttribute : Attribute
	{
		public Type[] parameters;

		public OverrideAttribute()
		{
		}

		public OverrideAttribute(params Type[] parameters) : this()
		{
			this.parameters = parameters;
		}
	}
}
