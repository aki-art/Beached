using Beached.Content;
using Beached.Utils;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Beached.Patches
{
    public class UnstableGroundManagerPatch
    {
        [HarmonyPatch(typeof(UnstableGroundManager), "OnPrefabInit")]
        public class UnstableGroundManager_OnPrefabInit_Patch
        {
            // adds new unstable elements to the UnstableGroundManager effects. 
            // the array is recreated like this because it's an array of a private structs
            public static void Prefix(ref object ___effects)
            {
                var unstableBeachedElements = new List<ElementInfo>()
                {
                    Elements.Gravel,
                    Elements.Ash
                };

                var effectList = ___effects as IList;
                var effectInfoType = effectList[0].GetType();

                var f_prefab = effectInfoType.GetField("prefab");
                var f_element = effectInfoType.GetField("element");

                var referencePrefab = FindSand(effectList, f_element, f_prefab);

                if (referencePrefab == null)
                {
                    return;
                }

                var newArray = Array.CreateInstance(effectInfoType, effectList.Count + unstableBeachedElements.Count);

                // copy previous values
                for (var i = 0; i < effectList.Count; i++)
                {
                    newArray.SetValue(effectList[i], i);
                }

                // add my new elements
                for (var i = 0; i < unstableBeachedElements.Count; i++)
                {
                    var elementInfo = unstableBeachedElements[i];
                    var newEffect = CreateNewEffect(effectInfoType, f_prefab, f_element, referencePrefab, elementInfo);

                    newArray.SetValue(newEffect, effectList.Count + i);
                }

                // set the new array to te original
                ___effects = newArray;
            }

            // Find sand so i can copy it's default values
            private static GameObject FindSand(IList effectList, FieldInfo f_element, FieldInfo f_prefab)
            {
                foreach (var effect in effectList)
                {
                    if ((SimHashes)f_element.GetValue(effect) == SimHashes.Sand)
                    {
                        return f_prefab.GetValue(effect) as GameObject;
                    }
                }

                Log.Warning("Sand not found in Unstable Ground manager.");

                return null;
            }

            private static object CreateNewEffect(Type effectInfoType, FieldInfo f_prefab, FieldInfo f_element, GameObject sandPrefab, ElementInfo elementInfo)
            {
                var newEffect = Activator.CreateInstance(effectInfoType);
                var newPrefab = UnityEngine.Object.Instantiate(sandPrefab);
                newPrefab.name = "Unstable" + elementInfo.ToString();

                f_prefab.SetValue(newEffect, newPrefab);
                f_element.SetValue(newEffect, elementInfo.SimHash);

                return newEffect;
            }
        }
    }
}
