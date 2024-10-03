using TUNING;

namespace Beached.Content.Scripts.Buildings
{
	public class Spinner : ComplexFabricator
	{
		public override void OnPrefabInit()
		{
			base.OnPrefabInit();

			choreType = Db.Get().ChoreTypes.Compound;
			fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
			sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
			duplicantOperated = true;
			heatedTemperature = 400f;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			workable.workTime = 10f;
			workable.trackUses = true;
			workable.workLayer = Grid.SceneLayer.BuildingUse;
			workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
			workable.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
			workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
			workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
			workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			workable.overrideAnims =
			[
				Assets.GetAnim("beached_spinner_interact_kanim")
			];
		}
	}
}
