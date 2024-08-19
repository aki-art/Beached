using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class FurSourceReactable : Reactable
	{
		public const string ID = "Beached_Reactable_FurSource";

		public FurSourceReactable(GameObject gameObject) : base(
				gameObject,
				ID,
				Db.Get().ChoreTypes.EmoteHighPriority,
				3,
				2,
				true,
				1f,
				overrideLayer: ObjectLayer.Minion)
		{
		}

		public override void InternalBegin()
		{
			KAnimControllerBase component = reactor.GetComponent<KAnimControllerBase>();
			component.AddAnimOverrides(Assets.GetAnim("anim_react_pip_kanim"));
			component.Play("hug_dupe_pre");
			component.Queue("hug_dupe_loop");
			component.Queue("hug_dupe_pst");

			component.onAnimComplete += Finish;
			gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnimSequence(
			[
			   "hug_dupe_pre",
			   "hug_dupe_loop",
			   "hug_dupe_pst"
			]);
		}

		private void Finish(HashedString anim)
		{
			if (!(anim == "hug_dupe_pst"))
				return;

			if (reactor != null)
			{
				reactor.GetComponent<KAnimControllerBase>().onAnimComplete -= Finish;
				ApplyAllergy();
			}
			else
				DebugUtil.DevLogError("HugMinionReactable finishing without adding a Hugged effect.");

			End();
		}

		private void ApplyAllergy()
		{
			var sourceStr = STRINGS.DUPLICANTS.DISEASES.BEACHED_FUR_ALLERGY.SOURCE.Replace("{Critter}", gameObject.GetProperName());
			reactor
				.GetComponent<Sicknesses>()
				.Infect(new SicknessExposureInfo(Db.Get().Sicknesses.Allergies.Id, sourceStr));
		}

		public override bool InternalCanBegin(
		  GameObject newReactor,
		  Navigator.ActiveTransition transition)
		{
			if (reactor != null)
				return false;
			return
				newReactor.HasTag(BTags.furAllergic)
				&& newReactor.TryGetComponent(out Effects effects) && !effects.HasEffect(CONSTS.DUPLICANTS.HISTAMINE_SUPPRESSION)
				&& newReactor.TryGetComponent(out Navigator navigator) && navigator.IsMoving();
		}

		public override void InternalCleanup() { }

		public override void InternalEnd() { }

		public override void Update(float dt) { }
	}
}
