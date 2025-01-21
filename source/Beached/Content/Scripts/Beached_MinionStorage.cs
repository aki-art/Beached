using Beached.Content.Defs.Foods;
using Beached.Content.ModDb;
using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_MinionStorage : KMonoBehaviour
	{
		[Serialize] public string hat;
		[Serialize] public Dictionary<string, float> serializedFlummoxModifiers;

		[MyCmpGet] private MinionResume resume;
		[MyCmpGet] private KBatchedAnimController kbac;
		[MyCmpGet] private KPrefabID kPrefabID;
		[MyCmpGet] private Effects effects;
		[MyCmpGet] private ConsumableConsumer consumableConsumer;

		private static HashSet<string> flummoxableStats;

		public static float flummoxStatPenaltyMin = -4, flummoxStatPenaltyMax = +4;

		private List<AttributeModifier> activeFlummoxModifiers;

		public void RestoreHat()
		{
			if (!hat.IsNullOrWhiteSpace())
			{
				MinionResume.ApplyHat(hat, kbac);
				hat = null;
			}
		}

		public void InitFlummoxableStats()
		{
			if (flummoxableStats == null)
			{
				var attributes = Db.Get().Attributes;

				flummoxableStats =
				[
					attributes.Art.Id,
					attributes.Athletics.Id,
					attributes.Botanist.Id,
					attributes.Caring.Id,
					attributes.Construction.Id,
					attributes.Cooking.Id,
					attributes.Digging.Id,
					attributes.Learning.Id,
					attributes.Machinery.Id,
					BAttributes.PRECISION_ID,
					attributes.Ranching.Id,
					attributes.Strength.Id,
				];

				if (DlcManager.FeatureClusterSpaceEnabled())
					flummoxableStats.Add(attributes.SpaceNavigation.Id);
			}
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			// smol
			if (resume.identity.nameStringKey == "VAHANO")
				kbac.animScale *= 0.9f;

			Subscribe((int)GameHashes.EffectAdded, OnEffectAdded);
			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);

			var attributes = gameObject.GetAttributes();

			if (serializedFlummoxModifiers != null)
			{
				activeFlummoxModifiers ??= [];

				foreach (var mod in serializedFlummoxModifiers)
				{
					var attributeModifier = new AttributeModifier(mod.Key, mod.Value);
					activeFlummoxModifiers.Add(attributeModifier);
					attributes.Add(attributeModifier);
				}
			}
		}

		private void OnEffectAdded(object obj)
		{
			if (obj is Effect effect)
			{
				switch (effect.Id)
				{
					case BEffects.FLUMMOXED:
						Log.Debug($"FLUMMOXED {name}");
						OnFlummoxed(false);
						break;
				}
			}
		}

		private void OnFlummoxed(bool forceReroll)
		{
			RemoveFlummoxModifiers();

			if (serializedFlummoxModifiers != null && !forceReroll)
				return;

			InitFlummoxableStats();

			var rolls = flummoxableStats
				.Shuffle()
				.Take(3);

			var attributes = gameObject.GetAttributes();

			foreach (var roll in rolls)
			{
				var amount = Mathf.Ceil(Random.Range(flummoxStatPenaltyMin, flummoxStatPenaltyMax));
				var attributeModifier = new AttributeModifier(
					roll,
					amount);

				activeFlummoxModifiers.Add(attributeModifier);
				attributes.Add(attributeModifier);
				serializedFlummoxModifiers.Add(roll, amount);
			}
		}

		private void RemoveFlummoxModifiers()
		{
			if (activeFlummoxModifiers != null)
			{
				var attributes = gameObject.GetAttributes();
				foreach (var mod in activeFlummoxModifiers)
				{
					if (mod != null)
						attributes.Remove(mod);
				}
			}

			activeFlummoxModifiers = [];
			serializedFlummoxModifiers = [];
		}

		private void OnEffectRemoved(object obj)
		{
			if (!effects.HasEffect(BEffects.POFF_CLEANEDTASTEBUDS))
				kPrefabID.RemoveTag(BTags.palateCleansed);

			if (!effects.HasEffect(BEffects.POFF_HELIUM))
				kPrefabID.RemoveTag(BTags.heliumPoffed);

			if (!effects.HasEffect(BEffects.FLUMMOXED))
			{
				RemoveFlummoxModifiers();
			};
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
