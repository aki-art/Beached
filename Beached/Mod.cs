global using Beached.Utils;
using Beached.Content;
using Beached.Content.BWorldGen;
using Beached.Content.Defs.Entities.Plants;
using Beached.ModDevTools;
using Beached.Settings;
using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TUNING;

namespace Beached
{
    public class Mod : UserMod2
    {
        public static bool DebugMode = true;

        public static Config Settings = new();

        public static string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // path field does not seem reliable with Local installs

        public static bool isFastTrackHere;

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            BTags.OnModLoad();

            ZoneTypes.Initialize();
            BeachedDevTools.Initialize();
            BWorldGenTags.Initialize();

            CROPS.CROP_TYPES.Add(new Crop.CropVal(CellAlgaeConfig.ID, 3f * CONSTS.CYCLE_LENGTH));

            /*            var types = Assembly.GetExecutingAssembly().GetTypes();
                        foreach (var type in types)
                        {
                            foreach (var methodInfo in type.GetMethods())
                            {
                                foreach (Attribute attr in Attribute.GetCustomAttributes(methodInfo))
                                {
                                    if (attr.GetType() == typeof(OverrideAttribute))
                                    {
                                        var overrideAttr = (OverrideAttribute)attr;
                                        var parentType = overrideAttr.parentType ?? type.BaseType;

                                        var parameters = methodInfo.GetParameters();
                                        var originalParams = new List<Type>();
                                        foreach (var p in parameters)
                                        {
                                            Log.Debug(p.Name);
                                            if (p.Name != "__result" && p.Name != "__instance")
                                            {
                                                originalParams.Add(p.ParameterType);
                                            }
                                        }

                                        Log.Debug(parentType.ToString());
                                        Log.Debug(methodInfo.Name.ToString());
                                        Log.Debug(string.Join(",", originalParams));
                                        var original = AccessTools.Method(parentType, methodInfo.Name, originalParams.ToArray());

                                        harmony.Patch(original, prefix: new HarmonyMethod(methodInfo));
                                    }
                                }
                            }*/

            /*                
                        var m_AttachExtensions = AccessTools.Method(typeof(Mod), "AttachExtension");
            Log.Debug(type.Name);
                            foreach (Attribute attr in Attribute.GetCustomAttributes(type))
                            {
                                if (attr.GetType() == typeof(ExtensionClassAttribute))
                                {
                                    var extension = (ExtensionClassAttribute)attr;
                                    var originalType = extension.originalType;

                                    var constructors = originalType.GetConstructors();
                                    var m_AttachGeneric = m_AttachExtensions.MakeGenericMethod(type);

                                    foreach (var constructor in constructors)
                                    {
                                        harmony.Patch(constructor, postfix: new HarmonyMethod(m_AttachGeneric));
                                    }
                                }
                            }
        }
            */
        }


        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<KMod.Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            foreach (var mod in mods)
            {
                if (mod.IsEnabledForActiveDlc())
                {
                    switch (mod.staticID)
                    {
                        case "TrueTiles":
                            Integration.TrueTiles.OnAllModsLoaded();
                            break;
                        case "PeterHan.FastTrack":
                            isFastTrackHere = true;
                            break;
                    }
                }
            }
        }
    }
}
