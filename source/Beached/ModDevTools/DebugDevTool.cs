#if DEVTOOLS
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.StarmapEntities;
using Beached.Content.ModDb;
using Beached.Content.Overlays;
using Beached.Content.Scripts;
using FMOD.Studio;
using FMODUnity;
using ImGuiNET;
using Klei;
using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Beached.Content.ModDb.LootTables;

namespace Beached.ModDevTools
{
	public class DebugDevTool : DevTool
	{
		private static Vector4 zoneTypeColor;
		private static Vector4 previousZoneTypeColor;
		private static int selectedZoneType;
		private static string audioFile = "";
		private static Dictionary<string, ShaderPropertyInfo> liquidShaderProperties;
		private static Material mat;
		public static bool renderLiquidTexture;
		private static float uvScale = 10f;

		private string[] zoneTypes;
		private EventInstance instance;

		public DebugDevTool()
		{
			RequiresGameRunning = true;
			clip = Lighting.Instance.Settings.BackgroundClip;
			bgscale = Lighting.Instance.Settings.BackgroundUVScale;
		}

		private static string rewardTestResult;
		private static bool accelerateLifeCycles;


		// TODO this is copy paste of FindAvailablePOISpawnLocations
		private static List<AxialI> FindAvailablePOISpawnLocations(AxialI location)
		{
			var available = new List<AxialI>();
			var flag = IsSuitablePOISpawnLocation(location);
			if (flag)
			{
				available.Add(location);
			}
			for (var dist = 1; dist <= 2; dist++)
			{
				foreach (var direction in AxialI.DIRECTIONS)
				{
					var destination = location + direction * dist;
					var flag2 = IsSuitablePOISpawnLocation(destination);
					if (flag2)
					{
						available.Add(destination);
					}
				}
			}
			return available;
		}
		private static bool IsSuitablePOISpawnLocation(AxialI location)
		{
			var flag = !ClusterGrid.Instance.IsValidCell(location);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				var entities = ClusterGrid.Instance.GetEntitiesOnCell(location);
				foreach (var entity in entities)
				{
					var flag2 = entity.Layer == EntityLayer.Asteroid || entity.Layer == EntityLayer.POI;
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public override void RenderTo(DevPanel panel)
		{
			ImGui.DragFloat("Flow Multipler", ref ElementInteractionsOverlayMode.flowMult, 1f, 1f, 2000f);

			if (ImGui.CollapsingHeader("Mirrors"))
			{
				foreach (var info in Game.Instance.roomProber.cavityInfos)
				{
					var mirrorCount = info.GetMirrorCount();
					if (mirrorCount > 0)
					{
						var roomType = info.room == null ? "N/A" : info.room.roomType.ToString();

						ImGui.Text($"{mirrorCount} Mirrors in {roomType}");
					}
				}

			}

			if (ImGui.CollapsingHeader("Depths Veil"))
			{
				DepthsVeil.OnImguiDebug();
			}

			if (ImGui.Button("Spawn Drale"))
			{
				var isPaused = SpeedControlScreen.Instance.IsPaused;
				if (isPaused)
				{
					SpeedControlScreen.Instance.Unpause(false);
					SpeedControlScreen.Instance.SetSpeed(0);
				}

				if (DlcManager.IsExpansion1Active())
				{
					var locations = FindAvailablePOISpawnLocations(GameUtil.GetActiveTelepad().GetMyWorldLocation());

					if (locations.Count > 0)
					{
						var prefab = Assets.GetPrefab(Drale_SpacedOutConfig.ID);
						var POIgo = Util.KInstantiate(prefab, null, null);
						var entity = POIgo.GetComponent<ClusterGridEntity>();
						entity.Location = locations.GetRandom();
						POIgo.SetActive(true);
					}
				}

				if (isPaused)
					SpeedControlScreen.Instance.Pause();
			}

			if (ImGui.Button("Spawn Oxygen"))
				ElementLoader.FindElementByHash(SimHashes.Oxygen).substance.SpawnResource(GameUtil.GetActiveTelepad().transform.position, 1f, 315, 255, 0);

			if (ImGui.Button("Spawn Chemical Ammonia"))
				ElementLoader.FindElementByHash(Elements.ChemicalProcessing.ammoniaGas)?.substance.SpawnResource(GameUtil.GetActiveTelepad().transform.position, 1f, 315, 255, 0);

			if (ImGui.Button("Spawn Ammonia"))
			{
				try
				{

					ElementLoader.FindElementByHash(Elements.ammonia).substance.SpawnResource(GameUtil.GetActiveTelepad().transform.position, 1f, 315, 255, 0);
				}
				catch (Exception e)
				{
					Log.Debug($"failed to spawn ammonia: {e.GetType().FullName} {e.Message} {e.StackTrace}");
				}
			}

			if (ImGui.Button("Spawn Salty Oxygen"))
				ElementLoader.FindElementByHash(Elements.saltyOxygen).substance.SpawnResource(GameUtil.GetActiveTelepad().transform.position, 1f, 315, 255, 0);

			if (ImGui.Checkbox("Accelerated Lifecycles", ref accelerateLifeCycles))
				GenericGameSettings.instance.acceleratedLifecycle = accelerateLifeCycles;

			if (ImGui.Button("Debug Data trigger"))
				Beached_Mod.Instance.Trigger(ModHashes.debugDataChange);

			if (ImGui.Button("test roll metal reward 1"))
			{
				BDb.lootTables.slagmiteShellDrops.TryGetItem(out var item);
				rewardTestResult = item.tag.name;
			}

			if (ImGui.Button("test roll 2"))
			{
				BDb.lootTables.TryGetLoot<MaterialReward>(out var item2, BDb.lootTables.slagmiteShellDrops.Id);
				rewardTestResult = item2.tag.name;
			}

			rewardTestResult ??= "N/A";

			ImGui.Text(rewardTestResult);
			if (ImGui.DragFloat("Permafrost UV Scale", ref uvScale))
			{
				//ElementLoader.GetElement(Elements.permaFrost.Tag).substance.material.SetFloat("_WorldUVScale", uvScale);
				var materials = World.Instance.groundRenderer.elementMaterials[Elements.permaFrost];
				materials.opaque.SetFloat("_WorldUVScale", uvScale);
				materials.alpha.SetFloat("_WorldUVScale", uvScale);
			}

			if (ImGui.CollapsingHeader("Asteroid Field Settings"))
			{
				//AsteroidBeltVisualizer.TestingInstance.OnImguiDebug();
			}

			if (ImGui.CollapsingHeader("FMOD Tests"))
			{
				if (ImGui.Button("Control test"))
					KFMOD.PlayUISound("event:/UI/Mouse/HUD_Click");

				if (ImGui.Button("Jelly Bounce"))
					KFMOD.PlayUISound("event:/jellybounce1");

				if (ImGui.Button("Play Stinger"))
					MusicManager.instance.PlaySong(Assets.GetSimpleSoundEventName("event:/beached/Music/ocean_palace"));
			}

			HandleSelectedObject(); ;
			Seasons();
			Notifications();
			Audio();
			ZoneColors();
			Shaders();
		}

		private static void HandleSelectedObject()
		{
			var selectedObject = SelectTool.Instance?.selected;

			if (selectedObject == null)
				return;

			if (selectedObject.TryGetComponent(out Navigator navigator))
				ImGui.Text($"Navigator current type: {navigator.CurrentNavType}");
			var debugs = selectedObject.GetComponents<IImguiDebug>();
			if (debugs != null)
			{
				ImGui.Separator();
				ImGui.Text($"Selected object: {selectedObject.GetProperName()}");
				ImGui.Separator();

				if (selectedObject.TryGetComponent(out BuildingHP buildingHP))
				{
					if (ImGui.Button("Break Building"))
						buildingHP.DoDamage(100);
				}

				foreach (var imguiDebug in debugs)
				{
					ImGui.Separator();
					ImGui.Text($"{imguiDebug.GetType()}");
					imguiDebug.OnImguiDraw();
				}
			}

			var joyBehavior = selectedObject.GetSMI<JoyBehaviourMonitor.Instance>();
			if (joyBehavior != null && ImGui.Button("Overjoy"))
				joyBehavior.GoToOverjoyed();

			var stress = Db.Get().Amounts.Stress.Lookup(selectedObject);
			if (stress != null)
			{
				if (joyBehavior != null)
					ImGui.SameLine();

				if (ImGui.Button("Stress"))
					stress.SetValue(100f);
			}

			if (selectedObject.TryGetComponent(out CreatureBrain brain) && brain.TryGetComponent(out Traits traits))
			{
				ImGui.Separator();
				ImGui.Text("GMO Traits");
				var allTraits = Db.Get().traitGroups.Get(BCritterTraits.GMO_GROUP).modifiers;
				foreach (var trait in allTraits)
				{
					if (!traits.HasTrait(trait))
					{
						if (ImGui.Button(trait.Name))
							traits.Add(trait);
					}
					else
					{
						ImGui.Text(trait.Name);
						ImGui.SameLine();

						if (ImGui.SmallButton("X"))
							traits.Remove(trait);
					}
				}
			}
		}

		private float siltStoneUV = 1f;
		private static float clip;
		private float bgscale = 1f;

		private void Shaders()
		{
			if (ImGui.CollapsingHeader("Shader"))
			{
				if (ImGui.DragFloat("Lighting.Instance.Settings.BackgroundClip", ref clip))
				{
					Lighting.Instance.Settings.BackgroundClip = clip;
				}
				if (ImGui.DragFloat("Settings.BackgrounSCale", ref bgscale))
				{
					Lighting.Instance.Settings.BackgroundUVScale = bgscale;
				}
				// BackgroundUVScale
				ImGui.DragFloat("_Beached_TimeOfDayColor", ref Beached_Mod.tempDayTimeProgress);


				if (ImGui.DragFloat("Rot UV Scale", ref siltStoneUV))
				{
					var materials = World.Instance.groundRenderer.elementMaterials;
					materials[Elements.rot].opaque.SetFloat("_WorldUVScale", siltStoneUV);
					materials[Elements.rot].alpha.SetFloat("_WorldUVScale", siltStoneUV);
				}

				var rendererGo = WaterCubes.Instance.cubes.transform.Find("WaterCubesMesh");
				if (rendererGo != null)
				{
					if (rendererGo.TryGetComponent(out MeshRenderer renderer))
					{
						if (renderer.sharedMaterial == null)
							ImGui.Text("shared material is null");
						else
						{
							if (renderer.sharedMaterial.shader == null)
								ImGui.Text("shared material shader is null");
							else
							{
								ImGui.Text(renderer.sharedMaterial.shader.name);
							}
						}
					}
					else
					{
						ImGui.Text("renderer is null");
					}
				}
				else
				{
					ImGui.Text("rendererGo is null");
				}

				if (liquidShaderProperties == null)
				{
					InitializeLiquidShaderProperties();
				}

				foreach (var prop in liquidShaderProperties.Values)
				{
					if (prop is ShaderPropertyInfo<float> floatProperty)
					{
						ImGui.DragFloat("L:" + floatProperty.propertyKey, ref floatProperty.newValue, (floatProperty.maxValue - floatProperty.minValue) / 1000f, floatProperty.minValue, floatProperty.maxValue);
						floatProperty.RefreshValue();
					}
				}

				if (mat != null && ImGui.Button("toggle heat haze"))
					mat.EnableKeyword("ENABLE_HEAT_HAZE");

				ImGui.Spacing();

			}
		}

		private void ZoneColors()
		{
			if (ImGui.CollapsingHeader("Zone colors"))
			{
				if (zoneTypes == null || zoneTypes.Length == 0)
				{
					zoneTypes = ZoneTypes.values.Select(z => z.ToString()).ToArray();
				}

				ImGui.SetColorEditOptions(ImGuiColorEditFlags.DisplayHex);
				ImGui.ColorPicker4("Color", ref zoneTypeColor);
				ImGui.ListBox("ZoneType", ref selectedZoneType, zoneTypes, zoneTypes.Length);

				if (zoneTypeColor != previousZoneTypeColor)
				{
					if (zoneTypeColor != null)
					{
						var color = new Color(zoneTypeColor.x, zoneTypeColor.y, zoneTypeColor.z, zoneTypeColor.w);

						World.Instance.zoneRenderData.zoneColours[(int)ZoneTypes.values.ElementAt(selectedZoneType)] = color;
						World.Instance.zoneRenderData.OnActiveWorldChanged();

						previousZoneTypeColor = zoneTypeColor;
					}
				}
			}
		}

		private void Audio()
		{
			if (ImGui.CollapsingHeader("Audio Player"))
			{
				if (ImGui.Button("Dump audio names"))
				{
					foreach (var asset in GlobalAssets.SoundTable)
					{
						Log.Debug(asset.Key + " - " + asset.Value);
					}

					Log.Debug("&##########################################################");
					if (RuntimeManager.StudioSystem.getBankList(out var banks) == FMOD.RESULT.OK)
					{
						foreach (var bank in banks)
						{
							bank.getID(out var bankID);
							Log.Debug($"{bankID}");
							bank.getEventList(out var events);
							if (events == null) continue;
							for (var index = 0; index < events.Length; index++)
							{
								var evt = events[index];
								evt.getPath(out var path);
								evt.getID(out var ID);
								Log.Debug($"   - {index} \t\tevent {path} {ID}");

								if (path.StartsWith("event:/expansion1/expansion1_Buildings/TemporalTearOpener"))
								{
									Log.Debug("Temporal tear opener time");
									evt.getParameterDescriptionCount(out var count);
									for (var i = 0; i < count; i++)
									{
										evt.getParameterDescriptionByIndex(i, out var parameter);
										Log.Debug($"        {parameter.id} {parameter.type} {parameter.defaultvalue} {(string)parameter.name}");
									}

									evt.getInstanceList(out var instances);
									foreach (var inst in instances)
									{
										inst.getDescription(out var desc);
										desc.getPath(out var path2);
										Log.Debug("      instance: " + path2);
									}
								}
							}
						}
					}
				}

				ImGui.InputTextMultiline("Audio Name", ref audioFile, 1024, new(100, 30));
				if (!audioFile.IsNullOrWhiteSpace())
				{
					if (ImGui.Button("Play"))
					{
						var sound = GlobalAssets.GetSound(audioFile);
						if (sound != null)
						{
							var position = new Vector3(SoundListenerController.Instance.transform.position.x,
								SoundListenerController.Instance.transform.position.y,
								SoundListenerController.Instance.transform.position.z);

							instance = KFMOD.BeginOneShot(sound, position, 1f);
							instance.setParameterByName("userVolume_SFX", 1f);
							KFMOD.EndOneShot(instance);
						}
					}

					if (ImGui.Button("Play From path"))
					{
						if (audioFile != null)
						{
							var position = new Vector3(SoundListenerController.Instance.transform.position.x,
								SoundListenerController.Instance.transform.position.y,
								SoundListenerController.Instance.transform.position.z);

							instance = KFMOD.BeginOneShot(audioFile, position);
							instance.setParameterByName("userVolume_SFX", 1f);
							KFMOD.EndOneShot(instance);
						}
					}
				}
			}
		}

		private static void Notifications()
		{
			if (ImGui.CollapsingHeader("Notifications"))
			{
				if (ImGui.Button("Open test notification"))
					Tutorials.Instance.Test();
			}
		}

		private static void Seasons()
		{
			if (ImGui.CollapsingHeader("Seasons"))
			{
				if (ImGui.Button("Start meteor season"))
					ClusterManager.Instance.activeWorld.GetSMI<GameplaySeasonManager.Instance>().StartNewSeason(BGameplaySeasons.astropelagosMoonletMeteorShowers);
			}
		}

		private void InitializeLiquidShaderProperties()
		{
			mat = WaterCubes.Instance.material;
			liquidShaderProperties = [];

			RegisterWaterShaderProperty(mat, "BlendScreen##shaderBlendScreen", 0.5f, 0, 1);
			RegisterWaterShaderProperty(mat, "LiquidSelectStart##shaderLiquidSelectStart", 0.438f, 0, 1);
			RegisterWaterShaderProperty(mat, "LiquidSelectRangeNear##shaderLiquidSelectRangeNear", 0.438f, 0, 1);
			RegisterWaterShaderProperty(mat, "LiquidSelectRangeFar##shaderLiquidSelectRangeFar", 0.438f, 0, 1);
			RegisterWaterShaderProperty(mat, "LiquidSelectRangeNearOrthoSize##shaderLiquidSelectRangeNearOrthoSize", 0.438f, 0, 200);
			RegisterWaterShaderProperty(mat, "LiquidSelectRangeFarOrthoSize##shaderLiquidSelectRangeFarOrthoSize", 0.438f, 0, 200);
			RegisterWaterShaderProperty(mat, "SampleFiltering##shaderSampleFiltering", 0f);
			RegisterWaterShaderProperty(mat, "EnableCaustics##shaderEnableCaustics", 1f);
			RegisterWaterShaderProperty(mat, "CausticSpeed##shaderCausticSpeed,", 1.21f, 0f, 10);
			RegisterWaterShaderProperty(mat, "CausticFrequency##shaderCausticFrequency", 0.5f, 0, 1);
			RegisterWaterShaderProperty(mat, "CausticCount##shaderCausticCount", 5f, 2, 10);
			RegisterWaterShaderProperty(mat, "CausticScale##shaderCausticScale", 1.4f, 0, 10);
			RegisterWaterShaderProperty(mat, "HeatOctaves##shaderHeatOctaves", 5f, 1, 5);
			RegisterWaterShaderProperty(mat, "HeatSpeed##shaderHeatSpeed", 3.8f, 0, 10);
			RegisterWaterShaderProperty(mat, "HeatFrequency##shaderHeatFrequency", 0.45f, 0, 2);
			RegisterWaterShaderProperty(mat, "HeatScale##shaderHeatScale", 2f, 0, 2);
			RegisterWaterShaderProperty(mat, "HeatColour##shaderHeatColour", new Color(1, 0.913725f, 0.5529411f, 1f));
			RegisterWaterShaderProperty(mat, "EnableWaves##shaderEnableWaves", 1f);
			RegisterWaterShaderProperty(mat, "WaveSpeed##shaderWaveSpeed", 1f, 0, 10);
			RegisterWaterShaderProperty(mat, "WaveFrequency##shaderWaveFrequency", 1f, 0, 10);
			RegisterWaterShaderProperty(mat, "WaveCount##shaderWaveCount", 5f, 2, 10);
			RegisterWaterShaderProperty(mat, "WaveHeight##shaderWaveHeight", 5f, 0, 20);
		}

		private void RegisterWaterShaderProperty<T>(Material material, string propertyKey, T value, float minValue = -1, float maxValue = -1)
		{
			liquidShaderProperties.Add(propertyKey, new ShaderPropertyInfo<T>(material, propertyKey, value, minValue, maxValue));
		}
	}
}
#endif