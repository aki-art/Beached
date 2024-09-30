using System;

namespace Beached.Utils
{
	/// <summary>
	/// Forcibly overrides a non-virtual method by patching the parent class.
	/// Type checking is expected in the method!
	/// </summary>
	public class OverrideAttribute(bool postfix = false) : Attribute
	{
		public Type[] parameters;
		public bool postfix = postfix;

		public OverrideAttribute(Type[] parameters, bool postfix = false) : this(postfix)
		{
			this.parameters = parameters;
		}
	}
}
