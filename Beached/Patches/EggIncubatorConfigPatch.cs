using Beached.Content.Scripts.Buildings;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Beached.Patches
{
    internal class EggIncubatorConfigPatch
    {

        [HarmonyPatch(typeof(EggIncubatorConfig), "ConfigureBuildingTemplate")]
        public class EggIncubatorConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
            }
        }
    }
}
