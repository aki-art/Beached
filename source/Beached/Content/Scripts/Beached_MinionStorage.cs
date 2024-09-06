using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using Klei.AI;
using KSerialization;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_MinionStorage : KMonoBehaviour
	{
		[Serialize] public string hat;

		[MyCmpGet] private MinionResume resume;
		[MyCmpGet] private KBatchedAnimController kbac;
		[MyCmpGet] private KPrefabID kPrefabID;
		[MyCmpGet] private Effects effects;
		[MyCmpGet] private ConsumableConsumer consumableConsumer;

		public void RestoreHat()
		{
			if (!hat.IsNullOrWhiteSpace())
			{
				MinionResume.ApplyHat(hat, kbac);
				hat = null;
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			// smol
			if (resume.identity.nameStringKey == "VAHANO")
				kbac.animScale *= 0.9f;

			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);
		}

		private void OnEffectRemoved(object obj)
		{
			if (!effects.HasEffect(BEffects.POFF_CLEANEDTASTEBUDS))
				kPrefabID.RemoveTag(BTags.palateCleansed);

			if (!effects.HasEffect(BEffects.POFF_HELIUM))
				kPrefabID.RemoveTag(BTags.heliumPoffed);
		}

		public void OnPalateCleansed()
		{
			consumableConsumer.SetPermitted(PoffConfig.GetRawId(Elements.nitrogen), false);
			consumableConsumer.SetPermitted(PoffConfig.GetCookedId(Elements.nitrogen), false);

			kPrefabID.AddTag(BTags.palateCleansed, true);
		}

		public void OnUnsavoryMealConsumed()
		{
			effects.Add(BEffects.UNSAVORY_MEAL, true);
		}
	}
}
