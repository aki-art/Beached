using Beached.Content;
using UnityEngine;

namespace Beached.Utils
{
    public class ElementInfo
    {
        public string id;
        public Element.State state;
        public string anim;
        public Color color;
        public Color uiColor;
        public Color conduitColor;

        public SimHashes SimHash { get; private set; }

        public Tag Tag { get; private set; }

        public ElementInfo(string id, string anim, Element.State state, Color color)
        {
            this.id = id;
            this.anim = anim;
            this.state = state;
            this.color = color;

            SimHash = ElementUtil.RegisterSimHash(id);
            Elements.elements.Add(this);
            Tag = id;
        }

        public static implicit operator SimHashes(ElementInfo info) => info.SimHash;

        public Element Get()
        {
            if (ElementLoader.elementTagTable == null)
            {
                Log.Warning("Trying to fetch element too early, elements are not loaded yet.");
                return null;
            }

            return ElementLoader.GetElement(Tag);
        }

        public Substance CreateSubstance(bool specular = false, Material material = null, Color? uiColor = null, Color? conduitColor = null, Color? specularColor = null, string normal = null)
        {
            if (material == null)
            {
                material = state == Element.State.Solid ? Assets.instance.substanceTable.solidMaterial : Assets.instance.substanceTable.liquidMaterial;
            }

            return ElementUtil.CreateSubstance(id, specular, anim, state, color, material, uiColor ?? color, conduitColor ?? color, specularColor, normal);
        }

        public Substance CreateSubstance(Color uiColor, Color conduitColor)
        {
            return CreateSubstance(false, null, uiColor, conduitColor);
        }

        public static ElementInfo Solid(string id, Color color)
        {
            return new ElementInfo(id, "glass_kanim", Element.State.Solid, color);
        }

        public static ElementInfo Liquid(string id, Color color)
        {
            return new ElementInfo(id, "liquid_tank_kanim", Element.State.Liquid, color);
        }

        public static ElementInfo Gas(string id, Color color)
        {
            return new ElementInfo(id, "gas_tank_kanim", Element.State.Gas, color);
        }

        public override string ToString()
        {
            return SimHash.ToString();
        }
    }
}
