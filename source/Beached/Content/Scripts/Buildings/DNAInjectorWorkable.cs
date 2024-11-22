using KSerialization;
using TUNING;

namespace Beached.Content.Scripts.Buildings
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class DNAInjectorWorkable : Workable
	{
		[MyCmpReq]
		private DNAInjector dnaInjector;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			requiredSkillPerk = null; // TODO: geneticist
			workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
			attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
			attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
			skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			overrideAnims =
			[
                //Assets.GetAnim("anim_interacts_spice_grinder_kanim")
                Assets.GetAnim("anim_interacts_vet_kanim")
			];

			SetWorkTime(5f);
			synchronizeAnims = false;
			showProgressBar = true;
			lightEfficiencyBonus = true;
		}

		public override void OnCompleteWork(WorkerBase worker)
		{
			// apply gneetic trait
			base.OnCompleteWork(worker);
			dnaInjector.ApplyTrait();
		}
	}
}
