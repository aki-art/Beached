using Beached.Content.Defs.Entities.Critters.Karacoos;
using Beached.Content.Defs.Entities.Critters.Squirrels;
using Beached.Content.Scripts.Entities;
using System;
using TUNING;

namespace Beached.Content.ModDb
{
	public class BFertilityModifiers
	{
		public const string
			MERPIP_FROM_PIP = "Beached_Merpip_From_Pip",
			PIP_FROM_MERPIP = "Beached_Pip_From_MerPip",
			AGING_KARACOO = "Beached_Aging_Karacoo",
			PIP_FROM_MERPIP_LANDCHECK = "Beached_Pip_From_MerPip_LandCheck";

		public static void Register()
		{
			var standardModifier = 0.00025f;

			var creators = CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS;

			/// manually overriding based on land access: <see cref="FertilityMonitor_InstancePatch"/>
			creators.Add(() => RegisterNearbyPlantWithTagModifier(
				MERPIP_FROM_PIP,
				MerpipConfig.EGG_ID,
				BTags.coral,
				standardModifier,
				true,
				"Corals Nearby",
				(desc, tag) =>
				{
					return string.Format(desc, tag.ProperName());
				}));

			creators.Add(() => RegisterNearbyPlantWithTagModifier(
				PIP_FROM_MERPIP,
				SquirrelConfig.EGG_ID,
				BTags.coral,
				-standardModifier,
				true,
				"No Corals Nearby",
				(desc, tag) =>
				{
					return string.Format(desc, tag, tag.ProperName());
				}));

			creators.Add(() => RegisterAgingModifier(
				AGING_KARACOO,
				KaracooConfig.EGG_ID,
				15,
				1f,
				"Aging"));

			/*			creators.Add(CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(
							MossyDreckoConfig.ID,
							MossyDreckoConfig.EGG_ID,
							SpinorilaConfig.ID,
							0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE));
			*/
			/*			creators.Add(() => RegisterLandAccessModifier(
							PIP_FROM_MERPIP_LANDCHECK,
							SquirrelConfig.EGG_ID,
							CONSTS.NAV_GRID.PIP,
							NavType.Floor));*/
		}

		private static void RegisterLandAccessModifier(string id, Tag eggTag, string navGrid, NavType navType)
		{
			Db.Get().CreateFertilityModifier(id, eggTag, "Reachable Dry Land", "Requires the ability to reach dry land. <size=60%>This is not reflected in the percentages, and will take effect at the moment of egg laying.</size>", null,
				(inst, eggType) =>
				{
					var checker = inst.gameObject.AddOrGet<Beached_HatchlingCanReachLandChecker>();
					checker.checkForFallenLocation = true;
					checker.SetNavigationType(navGrid, navType);
				});

		}


		private static void RegisterAgingModifier(string id, Tag eggTag, float ageThreshold, float modifierPerSecond, string name)
		{
			var description = modifierPerSecond < 0 ? "Is younger than {0} cycles." : "Is older than {0} cycles.";

			Db.Get().CreateFertilityModifier(id, eggTag, name, description, str => string.Format(description, ageThreshold), (inst, eggType) =>
			{
				var instance = inst.gameObject.GetSMI<Beached_AgeFertilityMonitor.Instance>();
				if (instance == null)
				{
					instance = new Beached_AgeFertilityMonitor.Instance(inst.master);
					instance.StartSM();
				}

				instance.OnAged += (dt, age) =>
				{
					var isOld = age > ageThreshold;

					if (isOld)
						inst.AddBreedingChance(eggType, dt * modifierPerSecond);
				};
			});
		}

		private static void RegisterNearbyPlantWithTagModifier(string id, Tag eggTag, Tag nearbyRequiredTag, float modifierPerSecond, bool alsoInvert, string name, Func<string, Tag, string> descriptionCb = null)
		{
			var tagName = nearbyRequiredTag.ProperName();
			var description = modifierPerSecond < 0 ? "No {0} in the same room." : "{0} in the same room.";

			Db.Get().CreateFertilityModifier(id, eggTag, name, description, descriptionCb == null ? null : str => descriptionCb.Invoke(str, nearbyRequiredTag), (inst, eggType) =>
			{
				var instance = inst.gameObject.GetSMI<Beached_NearbyPlantMonitor.Instance>();
				if (instance == null)
				{
					instance = new Beached_NearbyPlantMonitor.Instance(inst.master);
					instance.StartSM();
				}

				instance.OnUpdateNearbyPlants += (dt, plants, eggs) =>
				{
					var hasRequirement = false;
					foreach (KPrefabID creature in plants)
					{
						if (creature.HasTag(nearbyRequiredTag))
						{
							hasRequirement = true;
							break;
						}
					}

					if (hasRequirement)
						inst.AddBreedingChance(eggType, dt * modifierPerSecond);
					else if (alsoInvert)
						inst.AddBreedingChance(eggType, dt * -modifierPerSecond);
				};
			});
		}
	}
}
