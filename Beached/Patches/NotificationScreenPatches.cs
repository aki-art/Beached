using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Patches
{
    public class NotificationScreenPatches
    {
        [HarmonyPatch(typeof(NotificationScreen), "GetNotificationBGColour")]
        public class NotificationScreen_GetNotificationBGColour_Patch
        {
            public static void Postfix(NotificationType type, ref Color __result)
            {
                if (type == ModDb.BeachedTutorialMessage)
                {
                    __result = Color.green;
                }
            }
        }

        [HarmonyPatch(typeof(NotificationScreen), "GetNotificationColour")]
        public class NotificationScreen_GetNotificationColour_Patch
        {
            public static void Postfix(NotificationType type, ref Color __result)
            {
                if (type == ModDb.BeachedTutorialMessage)
                {
                    __result = Color.magenta;
                }
            }
        }

        [HarmonyPatch(typeof(NotificationScreen), "InitNotificationSounds")]
        public class NotificationScreen_InitNotificationSounds_Patch
        {
            public static void Postfix(Dictionary<NotificationType, string> ___notificationSounds)
            {
                ___notificationSounds[ModDb.BeachedTutorialMessage] = "Tutorial";
            }
        }


        [HarmonyPatch(typeof(NotificationScreen), "GetNotificationIcon")]
        public class NotificationScreen_GetNotificationIcon_Patch
        {
            public static void Postfix()
            {
                // TODO
            }
        }


        [HarmonyPatch]
        public class TargetType_TargetMethod_Patch
        {
            public static MethodInfo TargetMethod()
            {
                var t_Entry = typeof(NotificationScreen).GetNestedType("Entry", BindingFlags.NonPublic | BindingFlags.Instance);
                return AccessTools.Method(t_Entry, "Add", new[] { typeof(Notification) });
            }

            public static void Postfix(GameObject ___label, Notification notification)
            {
                if (notification.Type == ModDb.BeachedTutorialMessage)
                {
                    if (___label.TryGetComponent(out HierarchyReferences hierarchyReferences))
                    {
                        var button = hierarchyReferences.GetReference<Button>("MainButton");
                        var icon = hierarchyReferences.GetReference<KImage>("Icon");
                        var text = hierarchyReferences.GetReference<LocText>("Text");

                        var colors = button.colors;
                        colors.normalColor = ModAssets.Colors.UI.beachedTutorialBG;
                        colors.selectedColor = Color.cyan;

                        button.transition = Selectable.Transition.ColorTint;
                        text.color = Color.white;
                        icon.sprite = NotificationScreen.Instance.icon_warning;

                        button.colors = colors;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
        public class NotificationScreen_AddNotification_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var p_colors = AccessTools.PropertySetter(typeof(Selectable), "colors");

                var index = codes.FindLastIndex(ci => ci.Calls(p_colors));

                if (index == -1)
                {
                    return codes;
                }

                var m_GetColor = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetColor", new[]
                {
                    typeof(ColorBlock),
                    typeof(Notification),
                });

                codes.InsertRange(index, new[]
                {
                    new CodeInstruction(OpCodes.Dup),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, m_GetColor)
                });

                var f_LabelPrefab = AccessTools.Field(typeof(NotificationScreen), "LabelPrefab");
                var f_MessagesPrefab = AccessTools.Field(typeof(NotificationScreen), "MessagesPrefab");

                var index2 = codes.FindLastIndex(ci => ci.LoadsField(f_LabelPrefab));

                if (index2 == -1)
                {
                    return codes;
                }

                var f_LabelsFolder = AccessTools.Field(typeof(NotificationScreen), "LabelsFolder");
                var f_MessagesFolder = AccessTools.Field(typeof(NotificationScreen), "MessagesFolder");

                var index3 = codes.FindIndex(index2, ci => ci.LoadsField(f_LabelsFolder));

                if (index3 == -1)
                {
                    return codes;
                }

                var m_GetPrefab = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetPrefab", new[]
                {
                    typeof(GameObject),
                    typeof(NotificationScreen),
                    typeof(Notification),
                });

                var m_GetParent = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetParent", new[]
                {
                    typeof(GameObject),
                    typeof(NotificationScreen),
                    typeof(Notification),
                });

                codes.InsertRange(index3 + 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, m_GetParent)
                });

                codes.InsertRange(index2 + 1, new[]
                 {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, m_GetPrefab)
                });

                return codes;
            }

            private static void GetColor(ColorBlock existingValue, Notification notification)
            {
                if (notification.Type == ModDb.BeachedTutorialMessage)
                {
                    existingValue.normalColor = Color.red;
                }
            }

            private static GameObject GetPrefab(GameObject existingValue, NotificationScreen screen, Notification notification)
            {
                return notification.Type == ModDb.BeachedTutorialMessage ? screen.MessagesPrefab : existingValue;
            }

            private static GameObject GetParent(GameObject existingValue, NotificationScreen screen, Notification notification)
            {
                return notification.Type == ModDb.BeachedTutorialMessage ? screen.MessagesFolder : existingValue;
            }
        }
    }
}
