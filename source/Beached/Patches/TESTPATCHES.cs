using Beached.Content.Defs.Duplicants;
using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Beached.Content.ModDb.Sicknesses;
using Beached.ModDevTools;
using HarmonyLib;
using Klei;
using Klei.AI;
using ProcGen;
using Rendering.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Beached.Patches
{

	[HarmonyPatch(typeof(ChoreConsumer), "FindNextChore")]
	public class ChoreConsumer_FindNextChore_Patch
	{
		public static void Prefix(ChoreConsumer __instance)
		{
			if (__instance.consumerState == null) Log.Warning($"{nameof(__instance.consumerState)} is null. object is: {__instance.name}");

		}
	}

	/*[HarmonyPatch(typeof(AmbienceManager), "OnSpawn")]
	public class AmbienceManager_OnSpawn_Patch
	{
		public static void Prefix(AmbienceManager __instance)
		{
			if (!RuntimeManager.IsInitialized || Elements.crystalAmbiance > 0)
				return;

			foreach (var def in __instance.quadrantDefs)
			{
				var dirtTest = def.solidSounds[(int)SolidAmbienceType.Dirt];
				var dirtDesc = RuntimeManager.GetEventDescription(dirtTest);

				// padding for the useless NumTypes
				if (def.solidSounds.Length == 19)
				{
					var dirt = def.solidSounds[(int)SolidAmbienceType.Dirt];
					def.solidSounds = def.solidSounds.AddToArray(dirt); // 19
				}

				// adding my events
				Elements.crystalAmbiance = (SolidAmbienceType)def.solidSounds.Length; // 20
				var crystal = RuntimeManager.PathToEventReference("event:/beached/Environment/crystal_ambience");
				def.solidSounds = def.solidSounds.AddToArray(crystal);

				Elements.acidAmbience = (AmbienceType)def.liquidSounds.Length;
				var acid = RuntimeManager.PathToEventReference("event:/beached/Environment/acid_sizzle_loop");
				def.liquidSounds = def.liquidSounds.AddToArray(acid);
			}

			var substanceTable = Assets.instance.substanceTable;
			substanceTable.GetSubstance(Elements.aquamarine).audioConfig.solidAmbienceType = Elements.crystalAmbiance;
			substanceTable.GetSubstance(Elements.sulfurousWater).audioConfig.ambienceType = Elements.acidAmbience;

			var midHeavy = __instance.quadrantDefs[0].liquidSounds[(int)AmbienceType.MidHeavy];
			var instance = RuntimeManager.CreateInstance(midHeavy);
			Log.Debug("mid heavy tile perc: ");

			instance.getDescription(out var description);
			description.getParameterDescriptionCount(out var parameterDescriptionCount);
			for (int i = 0; i < parameterDescriptionCount; i++)
			{
				description.getParameterDescriptionByIndex(i, out var desc);


				description.getParameterLabelByID(desc.id, 0, out var label);
				description.getMinMaxDistance(out var min, out var max);
				Log.Debug($"{label} {desc.type} {desc.defaultvalue} {min} {max}");
			}
		}
	}

	[HarmonyPatch(typeof(AmbienceManager.Quadrant), MethodType.Constructor, [typeof(AmbienceManager.QuadrantDef)])]
	public class AmbienceManager_Quadrant_Ctor_Patch
	{
		public static void Postfix(AmbienceManager.QuadrantDef def, AmbienceManager.Quadrant __instance)
		{
			if (__instance.liquidLayers.Any(layer => layer.oneShotSound.Guid == def.liquidSounds[(int)Elements.acidAmbience].Guid))
				return;

			var acidLayer = new AmbienceManager.Layer(def.liquidSounds[(int)Elements.acidAmbience]);
			__instance.liquidLayers = __instance.liquidLayers.AddToArray(acidLayer);
			__instance.allLayers.Add(acidLayer);
			__instance.loopingLayers.Add(acidLayer);

			var additionalLayers = 1;

			// padding for NumTypes, but only if no other mod has done so yet
			if (__instance.solidLayers.Length == 19)
				additionalLayers += 1;

			var originalLength = __instance.solidLayers.Length;
			Array.Resize(ref __instance.solidLayers, __instance.solidLayers.Length + additionalLayers);
			for (int index = originalLength; index < originalLength + additionalLayers; index++)
			{
				__instance.solidLayers[index] = new AmbienceManager.Layer(new EventReference(), def.solidSounds[index]);
				__instance.allLayers.Add(__instance.solidLayers[index]);
				__instance.oneShotLayers.Add(__instance.solidLayers[index]);
				__instance.solidTimers = __instance.solidTimers.AddToArray(new AmbienceManager.Quadrant.SolidTimer());
			}
		}
	}*/

	public class TestPatches
	{
		[HarmonyPatch(typeof(CustomGameSettings), nameof(CustomGameSettings.GetSettingsForMixingMetrics))]
		public class CustomGameSettings_GetSettingsForMixingMetrics_Patch
		{
			public static void Prefix(CustomGameSettings __instance)
			{
				// TODO: this is a hack fix. figure out why it breaks in the first place. existing VANILLA mixing subworlds get added 
				// has been reported without Moonlet or Beached on vanilla worldgen, seems the save file is getting corrupted somehow
				// under another, pathed name (CarrotQuarryMixing -> subworldMixing/CarrotQuarryMixingSettings). the second should not exist, and does ont without Beached.
				// then the game crashes when saving a world
				var keys = new List<string>(__instance.CurrentMixingLevelsBySetting.Keys);
				foreach (var key in keys)
				{
					if (!__instance.MixingSettings.ContainsKey(key))
					{
						__instance.CurrentMixingLevelsBySetting.Remove(key);
						Log.Warning($"There was a corrupted MixingLevelsSetting \"{key}\" in the CustomGameSettings data, removed it to prevent a crash.");
					}
				}
			}
		}

		public static Texture2D testMask;

		[HarmonyPatch(typeof(Accessorizer), "OnDeserialized")]
		public class Accessorizer_OnDeserialized_Patch
		{
			public static void Prefix(Accessorizer __instance)
			{
				var component = __instance.GetComponent<MinionIdentity>();
				if (component.personalityResourceId == (HashedString)MinnowConfig.ID)
				{
					var personality = Db.Get().Personalities.Get(MinnowConfig.ID);
					__instance.bodyData = MinionStartingStats.CreateBodyData(personality); //Accessorizer.UpdateAccessorySlots(component.nameStringKey, ref __instance.accessories);
					__instance.accessories.RemoveAll(x => x.Get() == null);

					__instance.AddAccessory(Db.Get().AccessorySlots.Hair.Lookup(__instance.bodyData.hair));
					__instance.AddAccessory(Db.Get().AccessorySlots.HeadShape.Lookup(__instance.bodyData.headShape));
					__instance.AddAccessory(Db.Get().AccessorySlots.Mouth.Lookup(__instance.bodyData.mouth));
					__instance.AddAccessory(Db.Get().AccessorySlots.HatHair.accessories[0]);
				}
			}
		}

		[HarmonyPatch(typeof(TileRenderer), "LateUpdate")]
		public class TileRenderer_LateUpdate_Patch
		{
			public static void Postfix(TileRenderer __instance)
			{
				if (!DebugDevTool.renderLiquidTexture)
					return;

				Log.Debug("active brush count: " + __instance.ActiveBrushes.Count);
				Log.Debug("brush count: " + __instance.Brushes.Length);
				foreach (var brush in __instance.ActiveBrushes)
				{
					Log.Debug("rendering " + brush.Id);

					int mWidth = Screen.width;
					int mHeight = Screen.height;

					Rect rect = new(0, 0, mWidth, mHeight);
					RenderTexture renderTexture = new(mWidth, mHeight, 24);
					Texture2D screenShot = new(mWidth, mHeight, TextureFormat.RGBA32, false);

					Camera.main.targetTexture = renderTexture;
					Camera.main.Render();

					screenShot.ReadPixels(rect, 0, 0);
					screenShot.Apply();

					ModAssets.SaveImage(screenShot, brush.layer.ToString() + "_" + brush.Id.ToString());

					RenderTexture.active = renderTexture;

				}

				Camera.main.targetTexture = null;
				RenderTexture.active = null;

				DebugDevTool.renderLiquidTexture = false;
			}
		}

		[HarmonyPatch(typeof(WearableAccessorizer), "ApplyEquipment")]
		public class WearableAccessorizer_ApplyEquipment_Patch
		{
			public static void Postfix(WearableAccessorizer __instance, Equippable equippable, KAnimFile animFile)
			{
				Log.Debug("Apply equipment");

				if (equippable.def.Slot == BAssignableSlots.JEWELLERY_ID)
				{
					if (__instance.wearables.TryGetValue(BDb.WearableTypes.jewellery, out var wearable))
					{
						__instance.RemoveAnimBuild(
							wearable.buildAnims[0],
							wearable.buildOverridePriority);
					}

					__instance.wearables[BDb.WearableTypes.jewellery] = new(
						animFile,
						equippable.def.BuildOverridePriority);

					__instance.ApplyWearable();
					__instance.wearables.Remove(BDb.WearableTypes.jewellery);

				}
			}
		}

		/*        [HarmonyPatch(typeof(SettingsCache), "LoadFiles", typeof(string), typeof(string), typeof(List<YamlIO.Error>))]
				public class SettingsCache_LoadFiles_Patch
				{
					public static void Postfix()
					{
						foreach (var world in SettingsCache.worlds.worldCache)
						{
							Debug.Log(world.Key);

							world.Value.worldTemplateRules.Add(new ProcGen.World.TemplateSpawnRules()
							{
								names = new() { "test/test"},
								listRule = ProcGen.World.TemplateSpawnRules.ListRule.GuaranteeOne,

								allowedCellsFilter = new List<ProcGen.World.AllowedCellsFilter>()
								{
									new ProcGen.World.AllowedCellsFilter()
									{
										tag = WorldGenTags.AtEdge.ToString(),
										command = ProcGen.World.AllowedCellsFilter.Command.Replace,
										tagcommand = ProcGen.World.AllowedCellsFilter.TagCommand.AtTag
									}
								}
							});
						}
					}
				}*/

		// modify story traits
		//[HarmonyPatch(typeof(SettingsCache), "LoadStoryTraits")]
		public class SettingsCache_LoadStoryTraits_Patch
		{
			public static void Postfix()
			{
				var storyTraits = Traverse.Create(typeof(SettingsCache)).Field<Dictionary<string, WorldTrait>>("storyTraits").Value;

				foreach (var trait in storyTraits)
				{
					if (trait.Value.additionalWorldTemplateRules == null) continue;

					foreach (var templateRule in trait.Value.additionalWorldTemplateRules)
					{
						if (templateRule.allowedCellsFilter == null) continue;

						foreach (var filter in templateRule.allowedCellsFilter)
						{
							if (filter.temperatureRanges != null &&
								filter.temperatureRanges.Contains(Temperature.Range.ExtremelyHot) &&
								!filter.temperatureRanges.Contains(Temperature.Range.Chilly))
							{
								filter.temperatureRanges.Add(Temperature.Range.Chilly);
							}
						}
					}
				}
			}
		}

		//[HarmonyPatch(typeof(SimDebugView), "UpdateData")]
		public class SimDebugView_UpdateData_Patch
		{
			public static void Postfix(Texture2D texture, HashedString viewMode, byte[] ___texBytes, SimDebugView __instance)
			{
				if (viewMode != OverlayModes.Disease.ID)
				{
					return;
				}

				Grid.GetVisibleExtents(out var x0, out var minY, out var x1, out var maxY);

				for (var y0 = minY; y0 <= maxY; y0 += 16)
				{
					var y1 = Math.Min(y0 + 16 - 1, maxY);

					UpdateTexture(___texBytes, x0, x1, y0, y1);
				}

				testMask.LoadRawTextureData(___texBytes);
				testMask.Apply();

				__instance.diseaseMaterial.SetTexture("_GermTex", testMask);
			}

			private static void UpdateTexture(byte[] texBytes, int x0, int x1, int y0, int y1)
			{
				x0 = Mathf.Clamp(x0, 0, Grid.WidthInCells - 1);
				x1 = Mathf.Clamp(x1, 0, Grid.WidthInCells - 1);
				y0 = Mathf.Clamp(y0, 0, Grid.HeightInCells - 1);
				y1 = Mathf.Clamp(y1, 0, Grid.HeightInCells - 1);

				for (var i = y0; i <= y1; i++)
				{
					var num = Grid.XYToCell(x0, i);
					var num2 = Grid.XYToCell(x1, i);

					for (var j = num; j <= num2; j++)
					{
						var num3 = j * 4;
						if (Grid.IsActiveWorld(j))
						{
							var color = GetDiseaseColor(j);
							texBytes[num3] = color.r;
							texBytes[num3 + 1] = color.g;
							texBytes[num3 + 2] = color.b;
							texBytes[num3 + 3] = color.a;
						}
						else
						{
							texBytes[num3] = 0;
							texBytes[num3 + 1] = 0;
							texBytes[num3 + 2] = 0;
							texBytes[num3 + 3] = 0;
						}
					}
				}
			}
		}

		private static Color32 GetDiseaseColor(int cell)
		{
			if (Grid.DiseaseIdx[cell] == byte.MaxValue)
			{
				return new Color32(byte.MaxValue, 0, 0, 0);
			}

			// TODO: needs some dictionary lookup
			var disease = Db.Get().Diseases[Grid.DiseaseIdx[cell]];
			switch (disease.Id)
			{
				case CapSporeGerms.ID:
					return new Color32(1, 0, 0, 0);
				case LimpetEggGerms.ID:
					return new Color32(2, 0, 0, 0);
				case PollenGerms.ID:
					return new Color32(3, 0, 0, 0);
				default:
					return new Color32(0, 0, 0, 0);
			}
		}

		//[HarmonyPatch(typeof(SimDebugView), "OnPrefabInit")]
		public class SimDebugView_OnPrefabInit_Patch
		{
			public static void Postfix(SimDebugView __instance)
			{
				testMask = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureFormat.RGBA32, false);
				testMask.filterMode = FilterMode.Point;
				Log.Debug("SETTING SIMDEBUG");
				__instance.diseaseMaterial = new Material(ModAssets.Materials.germOverlayReplacer);
				__instance.diseaseMaterial.SetTexture("_MyArr", ModAssets.Textures.germOverlays);
				__instance.diseaseMaterial.SetTexture("_GermTex", testMask);
				//__instance.diseaseMaterial.SetVector("_UVScale", new Vector4(0.1f, 0.15f, 0, 0));
				Log.Debug("GRID" + Grid.WidthInCells);
				__instance.diseaseMaterial.SetVector("_UVScale", new Vector4(testMask.width / (Grid.WidthInCells * 100f) * 16f, testMask.height / (Grid.HeightInCells * 100f) * 16f, 0, 0));
			}
		}

		//[HarmonyPatch(typeof(SimDebugView), "SetDisease")]
		public class SimDebugView_SetDisease_Patch
		{
			public static void Postfix(SimDebugView __instance, GameObject ___plane)
			{
				var component = ___plane.GetComponent<Renderer>();
				// TODO: just for test
				component.sharedMaterial.SetTexture("_GermTex", component.sharedMaterial.mainTexture);
			}
		}

		//[HarmonyPatch(typeof(SimDebugView), "GetDiseaseColour")]
		public class SimDebugView_GetDiseaseColour_Patch
		{
			public static bool Prefix(ref Color __result, int cell)
			{
				if (Grid.DiseaseIdx[cell] != 255)
				{
					var disease = Db.Get().Diseases[Grid.DiseaseIdx[cell]];
					var darkness = SimUtil.DiseaseCountToAlpha(Grid.DiseaseCount[cell]);
					__result = (Color)GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName) * darkness;

					//TODO: this is temporary
					switch (disease.Id)
					{
						case CapSporeGerms.ID:
							__result.a = 1f / 255f;
							break;
						case PollenGerms.ID:
							__result.a = 3f / 255f;
							break;
						case LimpetEggGerms.ID:
							__result.a = 2f / 255f;
							break;
						default:
							__result.a = 0f / 255f;
							break;
					}
				}
				else
				{
					__result = Color.black;
				}

				return false;
			}
		}

		[HarmonyPatch(typeof(MinionPathFinderAbilities), "Refresh")]
		public class MinionPathFinderAbilities_Refresh_Patch
		{
			public static void Postfix(Navigator navigator, int ___proxyID)
			{
				Capped.dupesWithCaps[___proxyID] = navigator.GetSicknesses().Has(BSicknesses.capped);
			}
		}

		// TODO: remove this when Romen's Kanim extensions are here
		[HarmonyPatch(typeof(MinionResume), "RemoveHat", typeof(KBatchedAnimController))]
		public class MinionResume_RemoveHat_Patch
		{
			public static void Postfix(KBatchedAnimController controller)
			{
				if (controller == null || controller.gameObject == null)
				{
					return;
				}

				var sicknesses = controller.gameObject.GetSicknesses();
				if (sicknesses != null && sicknesses.Has(BSicknesses.capped))
				{
					controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, true);
				}
			}
		}

		[HarmonyPatch(typeof(MinionPathFinderAbilities), "TraversePath")]
		public class MinionPathFinderAbilities_TraversePath_Patch
		{
			public static void Postfix(ref PathFinder.PotentialPath path, int from_cell, int ___proxyID, ref bool __result)
			{
				if (__result && Capped.dupesWithCaps.TryGetValue(___proxyID, out var isCapped) && isCapped)
				{
					Grid.SuitMarker.Flags flags = 0;
					var needsSuitCheck = path.HasFlag(PathFinder.PotentialPath.Flags.PerformSuitChecks) && Grid.TryGetSuitMarkerFlags(from_cell, out flags, out _) && (flags & Grid.SuitMarker.Flags.Operational) > 0;
					var directionNeedsSuit = SuitMarker.DoesTraversalDirectionRequireSuit(from_cell, path.cell, flags);

					if (directionNeedsSuit && needsSuitCheck)
					{
						__result = false;
					}
				}
			}
		}

		// this allows them to go by without restriction >.>
		//[HarmonyPatch] //(typeof(SuitMarker.SuitMarkerReactable), "InternalCanBegin")]
		public class SuitMarker_InternalCanBegin_Patch
		{
			public static MethodInfo TargetMethod()
			{
				//var type = Type.GetType("SuitMarker.SuitMarkerReactable");
				var type = typeof(SuitMarker).GetNestedType("SuitMarkerReactable", BindingFlags.NonPublic | BindingFlags.Instance);

				Debug.Assert(type != null, "type is null");
				return type.GetMethod("InternalCanBegin", [typeof(GameObject), typeof(Navigator.ActiveTransition)]);
			}

			public static void Postfix(GameObject new_reactor, ref bool __result)
			{
				if (__result && new_reactor != null)
				{
					var sicknesses = new_reactor.GetSicknesses();
					if (sicknesses != null)
					{
						__result = !sicknesses.Has(BSicknesses.capped);
					}
				}
			}
		}
		// todo: refactored screens
		/*#if ELEMENTS
				[HarmonyPatch(typeof(DiseaseInfoScreen), "BuildFactorsStrings")]
				public class DiseaseInfoScreen_BuildFactorsStrings_Patch
				{
					public static void Postfix(CollapsibleDetailContentPanel ___currentGermsPanel)
					{
						var changeRate = ElementInteractions.GetFungalGermAverageChangeRateInLight();
						var str = $"    • Exposed to Light: ~{changeRate}/s";
						___currentGermsPanel.SetLabel("beached_light", str, "test");
					}
				}
		#endif*/

		//[HarmonyPatch(typeof(SkillsScreen), "RefreshSkillWidgets")]
		public class SkillsScreen_RefreshSkillWidgets_Patch
		{
			public static void Postfix(Dictionary<string, int> ___skillGroupRow)
			{
				var desiredMineralogyIndex = ___skillGroupRow[Db.Get().SkillGroups.Mining.Id] + 1;
				var originalMineralogyIndex = ___skillGroupRow[BSkillGroups.PRECISION_ID];

				if (originalMineralogyIndex == desiredMineralogyIndex)
				{
					return;
				}
				var items = ___skillGroupRow.ToList();

				for (var i = 0; i < items.Count; i++)
				{
					var item = items[i];
					if (item.Value >= desiredMineralogyIndex && item.Value < originalMineralogyIndex)
					{
						___skillGroupRow[item.Key] += 1; // make space for mineralogy
					}
				}

				// assign mineralogy to its place
				___skillGroupRow[BSkills.ARCHEOLOGY_ID] = desiredMineralogyIndex;
			}
		}

		[HarmonyPatch(typeof(GroundRenderer), "ConfigureMaterialShine")]
		public class GroundRenderer_ConfigureMaterialShine_Patch
		{
			public static bool Prefix(Material material)
			{
				if (!material.HasProperty("_ShineMask"))
				{
					return false;
				}

				return true;
			}
		}
	}
}
