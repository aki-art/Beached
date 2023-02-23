using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Scripts;
using FMOD.Studio;
using FMODUnity;
using ImGuiNET;
using System.Linq;
using UnityEngine;
using static ResearchTypes;
using static STRINGS.UI.CLUSTERMAP;

namespace Beached.ModDevTools
{
    public class DebugDevTool : DevTool
    {
        private static Vector4 zoneTypeColor;
        private static Vector4 previousZoneTypeColor;
        private static int selectedZoneType;
        private static string audioFile = "";

        private string[] zoneTypes;
        EventInstance instance;

        public DebugDevTool()
        {
            RequiresGameRunning = true;
        }

        public override void RenderTo(DevPanel panel)
        {
            if (ImGui.Button("Debug Data trigger"))
            {
                BeachedMod.Instance.Trigger(ModHashes.debugDataChange);
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
                    if (BeachedMod.Instance.treasury.chances.TryGetValue(element.id, out var treasures))
                    {
                        foreach (var treasure in treasures.treasures)
                        {
                            ImGui.Text($"\t\t{treasure.tag} {treasure.weight}");
                        }
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
                            if(events == null) continue;
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
        }
    }
}
