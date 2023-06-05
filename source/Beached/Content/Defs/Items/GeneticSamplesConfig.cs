using Beached.Content.ModDb;
using Beached.Content.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Defs.Items
{
	public class GeneticSamplesConfig : IMultiEntityConfig
	{
		public static List<GameObject> items = new();

		public const string MEATY = "Beached_Item_GeneticSample_Meaty";
		public const string EVERLASTING = "Beached_Item_GeneticSample_Everlasting";
		public const string BLAND = "Beached_Item_GeneticSample_Bland";
		public const string PRODUCTIVE1 = "Beached_Item_GeneticSample_Productive1";
		public const string PRODUCTIVE2 = "Beached_Item_GeneticSample_Productive2";
		public const string PRODUCTIVE3 = "Beached_Item_GeneticSample_Productive3";
		public const string FABULOUS = "Beached_Item_GeneticSample_Fabulous";
		public const string LASTING = "Beached_Item_GeneticSample_Lasting";
		public const string HYPOALLERGENIC = "Beached_Item_GeneticSample_Hypoallergenic";

		public List<GameObject> CreatePrefabs()
		{
			items = new()
			{
				CreateItem(MEATY,"beached_dna_sample_basic_kanim",BCritterTraits.MEATY),
				CreateItem(EVERLASTING,"beached_dna_sample_basic_kanim",BCritterTraits.EVERLASTING),
				CreateItem(BLAND,"beached_dna_sample_basic_kanim",BCritterTraits.BLAND),
				CreateItem(FABULOUS,"beached_dna_sample_basic_kanim",BCritterTraits.FABULOUS),
				CreateItem(PRODUCTIVE1,"beached_dna_sample_basic_kanim",BCritterTraits.PRODUCTIVE1),
				CreateItem(PRODUCTIVE2,"beached_dna_sample_basic_kanim",BCritterTraits.PRODUCTIVE2),
				CreateItem(PRODUCTIVE3,"beached_dna_sample_basic_kanim",BCritterTraits.PRODUCTIVE3),
				CreateItem(LASTING,"beached_dna_sample_basic_kanim",BCritterTraits.PRODUCTIVE3),
				CreateItem(HYPOALLERGENIC,"beached_dna_sample_basic_kanim",BCritterTraits.HYPOALLERGENIC),
			};

			return items;
		}

		public static GameObject CreateItem(string ID, string anim, string traitId)
		{
			var trait = Db.Get().traits.TryGet(traitId);
			var name = string.Format(STRINGS.ITEMS.MISC.BEACHED_GENETIC_SAMPLE.NAME, trait == null ? "Unknown" : trait.Name);

			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				name,
				STRINGS.ITEMS.MISC.BEACHED_GENETIC_SAMPLE.DESC,
				1f,
				false,
				Assets.GetAnim(anim),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.66f,
				0.75f,
				true,
				0,
				SimHashes.Creature,
				additionalTags: new List<Tag>
				{
					GameTags.PedestalDisplayable,
					GameTags.Organics
				});

			prefab.AddOrGet<EntitySplitter>();
			prefab.AddOrGet<SimpleMassStatusItem>();
			prefab.AddOrGet<GeneticSample>().traitId = traitId;

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
