using Beached.Content.ModDb;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
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
				if (type == BDb.BeachedTutorialMessage)
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
				if (type == BDb.BeachedTutorialMessage)
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
				___notificationSounds[BDb.BeachedTutorialMessage] = "Tutorial";
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

		[HarmonyPatch(typeof(NotificationScreen.Entry), "Add")]
		public class TargetType_TargetMethod_Patch
		{
			public static void Postfix(GameObject ___label, Notification notification)
			{
				if (notification.Type == BDb.BeachedTutorialMessage)
				{
					if (___label.TryGetComponent(out HierarchyReferences hierarchyReferences))
					{
						Button button = hierarchyReferences.GetReference<Button>("MainButton");
						KImage icon = hierarchyReferences.GetReference<KImage>("Icon");
						LocText text = hierarchyReferences.GetReference<LocText>("Text");

						ColorBlock colors = button.colors;
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

#if TRANSPILERS
		//[HarmonyPatch(typeof(NotificationScreen), "AddNotification")]
		public class NotificationScreen_AddNotification_Patch
		{
			public static IEnumerable<CodeInstruction> Transpiler(ILGenerator _, IEnumerable<CodeInstruction> orig)
			{
				List<CodeInstruction> codes = orig.ToList();

				System.Reflection.MethodInfo p_colors = AccessTools.PropertySetter(typeof(Selectable), "colors");

				int index = codes.FindLastIndex(ci => ci.Calls(p_colors));

				if (index == -1)
				{
					return codes;
				}

				System.Reflection.MethodInfo m_GetColor = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetColor",
				[
					typeof(ColorBlock),
					typeof(Notification),
				]);

				codes.InsertRange(index, new[]
				{
					new CodeInstruction(OpCodes.Dup),
					new CodeInstruction(OpCodes.Ldarg_1),
					new CodeInstruction(OpCodes.Call, m_GetColor)
				});

				System.Reflection.FieldInfo f_LabelPrefab = AccessTools.Field(typeof(NotificationScreen), "LabelPrefab");
				System.Reflection.FieldInfo f_MessagesPrefab = AccessTools.Field(typeof(NotificationScreen), "MessagesPrefab");

				int index2 = codes.FindLastIndex(ci => ci.LoadsField(f_LabelPrefab));

				if (index2 == -1)
				{
					return codes;
				}

				System.Reflection.FieldInfo f_LabelsFolder = AccessTools.Field(typeof(NotificationScreen), "LabelsFolder");
				System.Reflection.FieldInfo f_MessagesFolder = AccessTools.Field(typeof(NotificationScreen), "MessagesFolder");

				int index3 = codes.FindIndex(index2, ci => ci.LoadsField(f_LabelsFolder));

				if (index3 == -1)
				{
					return codes;
				}

				System.Reflection.MethodInfo m_GetPrefab = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetPrefab",
				[
					typeof(GameObject),
					typeof(NotificationScreen),
					typeof(Notification),
				]);

				System.Reflection.MethodInfo m_GetParent = AccessTools.Method(typeof(NotificationScreen_AddNotification_Patch), "GetParent",
				[
					typeof(GameObject),
					typeof(NotificationScreen),
					typeof(Notification),
				]);

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
				if (notification.Type == BDb.BeachedTutorialMessage)
				{
					existingValue.normalColor = Color.red;
				}
			}

			private static GameObject GetPrefab(GameObject existingValue, NotificationScreen screen, Notification notification)
			{
				return notification.Type == BDb.BeachedTutorialMessage ? screen.MessagesPrefab : existingValue;
			}

			private static GameObject GetParent(GameObject existingValue, NotificationScreen screen, Notification notification)
			{
				return notification.Type == BDb.BeachedTutorialMessage ? screen.MessagesFolder : existingValue;
			}
		}

#endif
	}
}
