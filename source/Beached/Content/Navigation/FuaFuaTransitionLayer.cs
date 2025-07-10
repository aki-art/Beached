using UnityEngine;

namespace Beached.Content.Navigation
{
	public class FuaFuaTransitionLayer : TransitionDriver.OverrideLayer
	{
		private KBatchedAnimController kbac;
		private float lastAngle = 0;

		public FuaFuaTransitionLayer(Navigator navigator) : base(navigator)
		{
			kbac = navigator.GetComponent<KBatchedAnimController>();
		}

		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.BeginTransition(navigator, transition);
			if (transition.start == NavType.Ladder || transition.end == NavType.Ladder)
			{
				var rotationVec = new Vector2f(transition.x, -transition.y);
				lastAngle = Vector2.Angle(Vector2.down, rotationVec);
				kbac.Rotation = lastAngle;
			}
			else if (lastAngle != 0)
			{
				kbac.Rotation = 0;
			}
		}
	}
}
