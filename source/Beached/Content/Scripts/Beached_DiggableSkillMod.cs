using KSerialization;
using System;
using UnityEngine;

namespace Beached.Content.Scripts
{
	internal class Beached_DiggableSkillMod : KMonoBehaviour
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private Diggable diggable;

		[Serialize] public string requirement;
		[Serialize] public bool isPrecisionRequested;
		[SerializeField] public string requiredSkillPerk;

		protected Guid workStatusItemHandle;
		private int skillsUpdated;
		private int minionUpdateHandle;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (requirement == null)
			{
				var filter = Beached_DigToolSkillToggle.Instance.GetActiveFilter();
				Configure(filter);
			}
			else
				Configure(requirement);

			skillsUpdated = Game.Instance.Subscribe((int)GameHashes.RolesUpdated, _ => UpdateStatusItem());
			minionUpdateHandle = Game.Instance.Subscribe((int)GameHashes.MinionMigration, _ => UpdateStatusItem());
			Subscribe((int)GameHashes.WorkableStopWork, _ => UpdateStatusItem());
			Subscribe((int)GameHashes.WorkableStartWork, _ => UpdateStatusItem());
		}

		public override void OnCleanUp()
		{
			Game.Instance.Unsubscribe(skillsUpdated);
			Game.Instance.Unsubscribe(minionUpdateHandle);
		}

		public void UpdateStatusItem()
		{
			kSelectable.RemoveStatusItem(workStatusItemHandle);

			if (isPrecisionRequested && !MinionResume.AnyMinionHasPerk(requiredSkillPerk, this.GetMyWorldId()))
			{
				var status_item = DlcManager.FeatureClusterSpaceEnabled()
					? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk
					: Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;

				workStatusItemHandle = kSelectable.AddStatusItem(status_item, requiredSkillPerk);
			}
		}

		public void Configure(string requirement)
		{
			this.requirement = requirement;
			var cell = diggable.cached_cell;

			if (!Grid.IsValidCell(cell))
				return;

			var element = Grid.Element[cell];

			var alwaysRequired = requirement == Beached_DigToolSkillToggle.PRECISION && element.HasTag(BTags.inclusionContaining);
			var preciousRequired = requirement == Beached_DigToolSkillToggle.PRECIOUS_ONLY && element.HasTag(BTags.preciousContaining);

			if (alwaysRequired || preciousRequired)
			{
				diggable.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, requiredSkillPerk);
				isPrecisionRequested = true;
			}

			UpdateStatusItem();
		}
	}
}
