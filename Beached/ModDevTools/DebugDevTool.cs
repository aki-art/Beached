using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.ModDb;
using Beached.Content.Scripts;
using FMOD.Studio;
using FMODUnity;
using ImGuiNET;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.ModDevTools
{
    public class DebugDevTool : DevTool
    {
        private static Vector4 zoneTypeColor;
        private static Vector4 previousZoneTypeColor;
        private static int selectedZoneType;
        private static float waterCube = 0.66f;
        private static float previousWaterCube = waterCube;
        private static string audioFile = "";
        private static Dictionary<string, ShaderPropertyInfo> liquidShaderProperties;
        private static Material waterCubesMaterial;

        private string[] zoneTypes;
        EventInstance instance;

        public abstract class ShaderPropertyInfo
        {
        }

        public class ShaderPropertyInfo<T> : ShaderPropertyInfo
        {
            public string propertyKey;
            public float minValue;
            public float maxValue;
            public T newValue;
            public T value;

            public ShaderPropertyInfo(string propertyKey, T defaultValue, float minValue = -1, float maxValue = -1)
            {
                this.propertyKey = propertyKey;
                this.minValue = minValue;
                this.maxValue = maxValue;
                value = newValue = defaultValue;
            }

            internal void RefreshValue()
            {
                if (!value.Equals(newValue))
                {
                    if (value is int i)
                        waterCubesMaterial.SetInt(propertyKey, i);
                    else if (value is float f)
                        waterCubesMaterial.SetFloat(propertyKey, f);
                    else if (value is Color c)
                        waterCubesMaterial.SetColor(propertyKey, c);

                    value = newValue;
                }
            }
        }

        public DebugDevTool()
        {
            RequiresGameRunning = true;
        }

        public override void RenderTo(DevPanel panel)
        {
            if (ImGui.Button("Debug Data trigger"))
            {
                Beached_Mod.Instance.Trigger(ModHashes.debugDataChange);
            }

            if (ImGui.Button("Infrared Disease"))
            {
                Infrared.Instance.SetMode(Infrared.Mode.Disease);
            }

            var selectedObject = SelectTool.Instance?.selected;

            if (selectedObject != null)
            {
                var debugs = selectedObject.GetComponents<IImguiDebug>();
                if (debugs != null)
                {
                    ImGui.Text($"Selected object: {selectedObject.GetProperName()}");
                    foreach (var imguiDebug in debugs)
                    {
                        ImGui.Spacing();
                        ImGui.Text($"{imguiDebug.GetType()}");
                        imguiDebug.OnImguiDraw();
                    }
                }

                if (selectedObject.TryGetComponent(out CreatureBrain brain) && brain.TryGetComponent(out Traits traits))
                {
                    ImGui.Spacing();
                    ImGui.Text("GMO Traits");
                    var allTraits = Db.Get().traitGroups.Get(BCritterTraits.GMO_GROUP).modifiers;
                    foreach (var trait in allTraits)
                    {
                        if (!traits.HasTrait(trait))
                        {
                            if (ImGui.Button(trait.Name))
                            {
                                traits.Add(trait);
                            }
                        }
                        else
                        {
                            ImGui.Text(trait.Name);
                        }
                    }
                }
            }

            if (ImGui.CollapsingHeader("Seasons"))
            {
                if (ImGui.Button("Start meteor season"))
                {
                    ClusterManager.Instance.activeWorld
                        .GetSMI<GameplaySeasonManager.Instance>()
                        .StartNewSeason(BGameplaySeasons.astropelagosMoonletMeteorShowers);
                }
            }

            if (ImGui.CollapsingHeader("Notifications"))
            {
                if (ImGui.Button("Open test notification"))
                {
                    Tutorials.Instance.Test();
                }
            }

            if (ImGui.TreeNode("Basic"))
            {
                if (ImGui.BeginTabBar("MyTabBar", ImGuiTabBarFlags.None))
                {
                    if (ImGui.BeginTabItem("Avocado"))
                    {
                        ImGui.Text("This is the Avocado tab!\nblah blah blah blah blah");
                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Broccoli"))
                    {
                        ImGui.Text("This is the Broccoli tab!\nblah blah blah blah blah");
                        ImGui.EndTabItem();
                    }
                    if (ImGui.BeginTabItem("Cucumber"))
                    {
                        ImGui.Text("This is the Cucumber tab!\nblah blah blah blah blah");
                        ImGui.EndTabItem();
                    }
                    ImGui.EndTabBar();
                }
                ImGui.Separator();
                ImGui.TreePop();
            }

            if (ImGui.CollapsingHeader("Diggers"))
            {
                foreach (var digger in Treasury.diggers)
                {
                    ImGui.Text($"{digger.Key}\t{digger.Value?.GetProperName()}");
                    var element = Grid.Element[digger.Key];
                    if (Beached_Mod.Instance.treasury.chances.TryGetValue(element.id, out var treasures))
                    {
                        foreach (var treasure in treasures.treasures)
                        {
                            ImGui.Text($"\t\t{treasure.tag} {treasure.weight}");
                        }
                    }
                }
            }

            if(ImGui.Button("Find monitor"))
            {
                var monitors = Object.FindObjectsOfType<DrowningMonitorUpdater>();
                if(monitors == null)
                {
                    ImGui.Text("no monitor");
                }
                else
                {
                    ImGui.Text($"{monitors.Length} monitors");
                    foreach (var monitor in monitors)
                    {
                        ImGui.Text($"{monitor.name} {monitor.transform.parent?.name}");
                    }
                }

            }
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
                                EventDescription evt = events[index];
                                evt.getPath(out var path);
                                evt.getID(out var ID);
                                Log.Debug($"   - {index} \t\tevent {path} {ID}");

                                if (path.StartsWith("event:/expansion1/expansion1_Buildings/TemporalTearOpener"))
                                {
                                    Log.Debug("Temporal tear opener time");
                                    evt.getParameterDescriptionCount(out int count);
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

            ImGui.Spacing();

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


            if (ImGui.CollapsingHeader("Shader"))
            {
                if(ImGui.Button("Save liquid plane"))
                {
                    ModAssets.SaveImage(WaterCubes.Instance.cubes.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("_MainTex2"), "watercubes");
                }

                var rendererGo = WaterCubes.Instance.cubes.transform.Find("WaterCubesMesh");
                if (rendererGo != null)
                {
                    if(rendererGo.TryGetComponent(out MeshRenderer renderer))
                    {
                        if(renderer.sharedMaterial == null)
                        {
                            ImGui.Text("shared material is null");
                        }
                        else
                        {
                            if(renderer.sharedMaterial.shader == null)
                            {
                                ImGui.Text("shared material shader is null");
                            }
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

                foreach(var prop in liquidShaderProperties.Values)
                {
                    if(prop is ShaderPropertyInfo<float> floatProperty)
                    {
                        ImGui.DragFloat(floatProperty.propertyKey, ref floatProperty.newValue, (floatProperty.maxValue - floatProperty.minValue) / 1000f, floatProperty.minValue, floatProperty.maxValue);
                        floatProperty.RefreshValue();
                    }
                }
            }
        }

        private void InitializeLiquidShaderProperties()
        {
            waterCubesMaterial = WaterCubes.Instance.material;
            liquidShaderProperties = new Dictionary<string, ShaderPropertyInfo>();

            RegisterWaterShaderProperty("_BlendScreen", 0.5f, 0, 1);
            RegisterWaterShaderProperty("_LiquidSelectStart", 0.438f, 0, 1);
            RegisterWaterShaderProperty("_LiquidSelectRangeNear", 0.438f, 0, 1);
            RegisterWaterShaderProperty("_LiquidSelectRangeFar", 0.438f, 0, 1);
            RegisterWaterShaderProperty("_LiquidSelectRangeNearOrthoSize", 0.438f, 0, 200);
            RegisterWaterShaderProperty("_LiquidSelectRangeFarOrthoSize", 0.438f, 0, 200);
            RegisterWaterShaderProperty("_SampleFiltering", 0f);
            RegisterWaterShaderProperty("_EnableCaustics", 1f);
            RegisterWaterShaderProperty("_CausticSpeed, 1.21f", 0f, 10);
            RegisterWaterShaderProperty("_CausticFrequency", 0.5f, 0, 1);
            RegisterWaterShaderProperty("_CausticCount", 5f, 2, 10);
            RegisterWaterShaderProperty("_CausticScale", 1.4f, 0, 10);
            RegisterWaterShaderProperty("_HeatOctaves", 5f, 1, 5);
            RegisterWaterShaderProperty("_HeatSpeed", 3.8f, 0, 10);
            RegisterWaterShaderProperty("_HeatFrequency", 0.45f, 0, 2);
            RegisterWaterShaderProperty("_HeatScale", 2f, 0, 2);
            RegisterWaterShaderProperty("_HeatColour", new Color(1, 0.913725f, 0.5529411f, 1f));
            RegisterWaterShaderProperty("_EnableWaves", 1f);
            RegisterWaterShaderProperty("_WaveSpeed", 1f, 0, 10);
            RegisterWaterShaderProperty("_WaveFrequency", 1f, 0, 10);
            RegisterWaterShaderProperty("_WaveCount", 5f, 2, 10);
            RegisterWaterShaderProperty("_WaveHeight", 5f, 0, 20);
        }

        private void RegisterWaterShaderProperty<T>(string propertyKey, T value, float minValue = -1, float maxValue = -1)
        {
            liquidShaderProperties.Add(propertyKey, new ShaderPropertyInfo<T>(propertyKey, value, minValue, maxValue));
        }
    }
}
