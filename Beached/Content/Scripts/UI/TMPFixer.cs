using HarmonyLib;
using PeterHan.PLib.Core;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
    public class TMPFixer : KMonoBehaviour
    {
        [SerializeField]
        public TextAlignmentOptions textAlignment;

        public override void OnSpawn()
        {
            GetComponent<LocText>().alignment = textAlignment;
        }

        public static LocText ConvertToLocText(TextMeshProUGUI textMeshProUGUI)
        {
            var f_m_isAlignmentEnumConverted = typeof(TMP_Text)
                .GetField("m_isAlignmentEnumConverted", BindingFlags.NonPublic | BindingFlags.Instance);
            f_m_isAlignmentEnumConverted.SetValue(textMeshProUGUI, true);

            var go = Instantiate(textMeshProUGUI.gameObject);
            DestroyImmediate(go.GetComponent<TextMeshProUGUI>());
            go.SetActive(true);

            if (textMeshProUGUI.transform.parent != null)
            {
                go.transform.SetParent(textMeshProUGUI.transform.parent);
            }

            var locText = go.AddOrGet<LocText>();

            Log.Debug("copying");
            Log.Debug(locText == null);
            locText.gameObject.AddComponent<TMPFixer>().textAlignment = textMeshProUGUI.alignment;

            //CopyFields(textMeshProUGUI, locText, textMeshProUGUI.GetType());

            locText.enableWordWrapping = textMeshProUGUI.enableWordWrapping;
            locText.allowLinksInternal = true;
            locText.color = textMeshProUGUI.color;
            locText.font = textMeshProUGUI.font;
            locText.alignment = textMeshProUGUI.alignment;
            locText.alpha = textMeshProUGUI.alpha;
            locText.autoSizeTextContainer = textMeshProUGUI.autoSizeTextContainer;
            locText.colorGradient = textMeshProUGUI.colorGradient;
            locText.colorGradientPreset = textMeshProUGUI.colorGradientPreset;
            locText.fontSize = textMeshProUGUI.fontSize;
            locText.fontStyle = textMeshProUGUI.fontStyle;
            locText.fontWeight = textMeshProUGUI.fontWeight;
            locText.richText = textMeshProUGUI.richText;
            locText.enableKerning = textMeshProUGUI.enableKerning;
            locText.enableWordWrapping = textMeshProUGUI.enableWordWrapping;
            locText.maxVisibleLines = textMeshProUGUI.maxVisibleLines;

            if (textMeshProUGUI.text.StartsWith("STRINGS."))
            {
                locText.key = textMeshProUGUI.text;
            }
            else
            {
                locText.text = textMeshProUGUI.text;
            }

            locText.SetAllDirty();
            Destroy(textMeshProUGUI);

            return locText;
        }

        private static void CopyFields(TextMeshProUGUI src, LocText dst)
        {
            foreach (var field in typeof(TextMeshProUGUI).BaseType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var attr in Attribute.GetCustomAttributes(field, true))
                {
                    Log.Debug("setting field: " + field.Name);
                    if (attr is SerializeField)
                    {
                        field.SetValue(dst, field.GetValue(src));
                    }
                }
            }
        }

        private static void CopyFields(object src, object dst, Type type)
        {
            foreach (var field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                foreach (var attr in Attribute.GetCustomAttributes(field, true))
                {
                    if (attr is SerializeField)
                    {
                        field.SetValue(dst, field.GetValue(src));
                    }
                }
            }

            if (type.BaseType != null)
            {
                CopyFields(src, dst, type.BaseType);
            }
        }
    }
}
