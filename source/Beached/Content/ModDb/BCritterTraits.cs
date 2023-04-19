using Beached.Content.Defs.Foods;
using Beached.Content.Scripts.Entities;
using Klei.AI;
using System;
using UnityEngine;

namespace Beached.Content.ModDb
{
    internal class BCritterTraits
    {
        public const string GMO_GROUP = "Beached_GMOTraits";

        public const string BLAND = "Beached_GMOTraits_Bland";
        //public const string CHONKER = "Beached_GMOTraits_Chonker";
        public const string EVERLASTING = "Beached_GMOTraits_Everlasting";
        public const string FABULOUS = "Beached_GMOTraits_Fabulous";
        public const string HYPOALLERGENIC = "Beached_GMOTraits_HypoAllergenic";
        public const string LASTING = "Beached_GMOTraits_Lasting";
        public const string MEATY = "Beached_GMOTraits_Meaty";
        public const string PRODUCTIVE1 = "Beached_GMOTraits_Productive1";
        public const string PRODUCTIVE2 = "Beached_GMOTraits_Productive2";
        public const string PRODUCTIVE3 = "Beached_GMOTraits_Productive3";

        public static void Register()
        {
            var db = Db.Get();

            CreateBasicTrait(BLAND).OnAddTrait += OnBland;
            var everlasting = CreateBasicTrait(EVERLASTING);
            everlasting.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, float.PositiveInfinity));
            everlasting.OnAddTrait += OnEverLasting;
            CreateBasicTrait(FABULOUS).Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, 1f, is_multiplier: true));
            CreateBasicTrait(HYPOALLERGENIC); // TODO
            CreateBasicTrait(LASTING)
                .Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 1f, is_multiplier: true));
            CreateBasicTrait(MEATY).OnAddTrait += OnMeaty;
            CreateBasicTrait(PRODUCTIVE1).OnAddTrait += go => OnProductive(go, 2f);
            CreateBasicTrait(PRODUCTIVE2).OnAddTrait += go => OnProductive(go, 4f);
            CreateBasicTrait(PRODUCTIVE3).OnAddTrait += go => OnProductive(go, 8f);
        }

        private static void OnProductive(GameObject obj, float multiplier)
        {
            obj.AddOrGet<Beached_CritterMetabolismModifier>().UpdateMetabolism(multiplier);
        }

        private static void OnEverLasting(GameObject obj)
        {
            if (obj.TryGetComponent(out Traits traits))
            {
                RemoveFromTraits(traits, LASTING);
                RemoveFromTraits(traits, "CritterShortLived");
                RemoveFromTraits(traits, "CritterEnduring");
            }
        }

        private static void RemoveFromTraits(Traits traits, string id)
        {
            if (traits.HasTrait(id))
            {
                var trait = Db.Get().traits.TryGet(id);
                if (trait != null)
                {
                    traits.Remove(trait);
                }
            }
        }

        private static void OnBland(GameObject obj)
        {
            if (obj.TryGetComponent(out DecorProvider decorProvider))
            {
                decorProvider.decor.ClearModifiers();
                decorProvider.decorRadius.ClearModifiers();
            }
        }

        private static void OnMeaty(GameObject obj)
        {
            if (obj.TryGetComponent(out Butcherable butcherable))
            {
                for (int i = 0; i < butcherable.drops.Length; i++)
                {
                    var drop = butcherable.drops[i];
                    if (drop == MeatConfig.ID)
                    {
                        butcherable.drops[i] = HighQualityMeatConfig.ID;
                    }
                }
            }

            if (obj.TryGetComponent(out KBatchedAnimController kbac))
            {
                kbac.animScale *= 1.1f;
            }

            if (obj.TryGetComponent(out KBoxCollider2D collider))
            {
                collider.size *= 1.1f;
            }
        }

        private static Trait CreateBasicTrait(string ID)
        {
            var trait = Db.Get().CreateTrait(
                ID,
                Strings.TryGet($"Beached.STRINGS.CREATURES.TRAITS.{ID.ToUpperInvariant()}.NAME", out var name) ? name : "MISSING.",
                Strings.TryGet($"Beached.STRINGS.CREATURES.TRAITS.{ID.ToUpperInvariant()}.DESC", out var desc) ? desc : "MISSING.",
                GMO_GROUP,
                true,
                null,
                true,
                false);

            if (Mod.isCritterTraitsRebornHere)
            {
                Integration.CritterTraitsReborn.addTraitToVisibleList(ID);
            }

            return trait;
        }
    }
}
