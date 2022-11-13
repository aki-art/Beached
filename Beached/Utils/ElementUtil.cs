using Klei.AI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Beached.Utils
{
    public class ElementUtil
    {
        public static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        public static SimHashes RegisterSimHash(string name)
        {
            var simHash = (SimHashes)Hash.SDBMLower(name);
            SimHashNameLookup.Add(simHash, name);
            ReverseSimHashNameLookup.Add(name, simHash);

            return simHash;
        }

        public static Substance CreateSubstance(string id, bool specular, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor, string normal)
        {
            var animFile = Assets.Anims.Find(a => a.name == anim);

            if(animFile == null)
            {
                animFile = Assets.Anims.Find(a => a.name == "glass_kanim");
            }

            var newMaterial = new Material(material);

            if (state == Element.State.Solid)
            {
                SetTexture(newMaterial, id.ToLowerInvariant(), "_MainTex");

                if (specular)
                {
                    SetTexture(newMaterial, id.ToLowerInvariant() + "_spec", "_ShineMask");

                    if (specularColor.HasValue)
                    {
                        newMaterial.SetColor("_ShineColour", specularColor.Value);
                    }
                }

                if(!normal.IsNullOrWhiteSpace())
                {
                    SetTexture(newMaterial, normal, "_NormalNoise");
                }
            }

            return ModUtil.CreateSubstance(id.ToString(), state, animFile, newMaterial, color, uiColor, conduitColor);
        }

        // TODO: load from an assetbundle later
        private static void SetTexture(Material material, string texture, string property)
        {
            var path = Path.Combine(Mod.folder, "assets", "textures", texture + ".png");

            if (ModAssets.TryLoadTexture(path, out var tex))
            {
                material.SetTexture(property, tex);
            }
        }

        public static void AddModifier(Element element, float decor, float overHeat)
        {
            if (decor != 0)
            {
                element.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, decor, element.name, true));
            }

            if (overHeat != 0)
            {
                element.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, overHeat, element.name, false));
            }
        }
    }
}
