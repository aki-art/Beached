using Beached.Content;
using Beached.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Beached.Patches
{
	public class MinionConfigPatch
	{

		[HarmonyPatch(typeof(MinionConfig), nameof(MinionConfig.SetupLaserEffects))]
		public class MinionConfig_SetupLaserEffects_Patch
		{
			public static void Postfix(GameObject prefab)
			{
				ModAssets.Fx.Lasers.AddLaserEffect(prefab);
			}
		}

		[HarmonyPatch(typeof(MinionConfig), nameof(MinionConfig.OnSpawn))]
		public class MinionConfig_OnSpawn_Patch
		{
			public static void Postfix(GameObject go)
			{
				var sensors = go.GetComponent<Sensors>();
				sensors.Add(new PlushPlacebleBedSensor(sensors));
			}
		}

		[HarmonyPatch(typeof(MinionConfig), nameof(MinionConfig.CreatePrefab))]
		public class MinionConfig_CreatePrefab_Patch
		{
			public static void Postfix(GameObject __result)
			{
				ConfigureSnapons(__result);

				__result.AddOrGet<Beached_MinionStorage>();
				__result.AddOrGet<Beached_LifeGoalTracker>();
				__result.AddOrGet<Beached_MinionEvents>();

				__result.AddTag(BTags.Creatures.doNotTargetMeByCarnivores);
			}

			private static void ConfigureSnapons(GameObject __result)
			{
				var snapOn = __result.GetComponent<SnapOn>();
				snapOn.snapPoints.Add(new SnapOn.SnapPoint
				{
					pointName = CONSTS.SNAPONS.CAP,
					automatic = false,
					context = "",
					buildFile = Assets.GetAnim("beached_head_shroom_kanim"),
					overrideSymbol = "snapTo_hat"
				});

				snapOn.snapPoints.Add(new SnapOn.SnapPoint
				{
					pointName = CONSTS.SNAPONS.RUBBER_BOOTS,
					automatic = false,
					context = "",
					buildFile = Assets.GetAnim("beached_rubberboots_kanim"),
					overrideSymbol = "foot"
				});

				snapOn.snapPoints.Add(new SnapOn.SnapPoint
				{
					pointName = "dig",
					automatic = false,
					context = ModAssets.CONTEXTS.HARVEST_ORANGE_SQUISH,
					buildFile = Assets.GetAnim((HashedString)"plant_harvester_gun_kanim"),
					overrideSymbol = (HashedString)"snapTo_rgtHand"
				});

				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.MAXIXE, "beached_maxixe_necklace_kanim");
				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.ZIRCON, "beached_zircon_necklace_kanim");
				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.ZEOLITE, "beached_zeolite_necklace_kanim");
				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.HEMATITE, "beached_hematite_necklace_kanim");
				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.PEARL, "beached_pearl_necklace_kanim");
				AddNecklaceSnapon(snapOn, CONSTS.SNAPONS.JEWELLERIES.STRANGE_MATTER, "beached_strange_matter_necklace_kanim");
			}

			private static void AddNecklaceSnapon(SnapOn snapOn, string snaponId, string anim)
			{
				snapOn.snapPoints.Add(new SnapOn.SnapPoint
				{
					pointName = snaponId,
					automatic = false,
					context = "",
					buildFile = Assets.GetAnim(anim),
					overrideSymbol = "necklace"
				});
			}
		}
	}
}
