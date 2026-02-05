using Beached.Content.Scripts.Entities.AI;

namespace Beached.Content.Navigation
{
	public class SadSnailTransitionLayer(Navigator navigator) : TransitionDriver.OverrideLayer(navigator)
	{
		public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
		{
			base.BeginTransition(navigator, transition);

			var desiccationMonitor = navigator.GetSMI<DesiccationMonitor.Instance>();

			if (desiccationMonitor == null || !desiccationMonitor.IsDesiccating())
				return;

			var anim_name = HashCache.Get().Get(transition.anim.HashValue) + "_sad";

			if (navigator.animController.HasAnimation(anim_name))
				transition.anim = anim_name;
		}
	}
}
