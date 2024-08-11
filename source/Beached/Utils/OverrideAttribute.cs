using System;

namespace Beached.Utils
{
	/// <summary>
	/// Forcibly overrides a non-virtual method by patching the parent class
	/// </summary>
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
