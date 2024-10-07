using Beached.Content.ModDb;
using Beached.Integration;
using Klei.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class AcidVulnerableCreature : KMonoBehaviour, ISim1000ms//, IGameObjectEffectDescriptor//, IWiltCause
	{
		[MyCmpGet] private Health health;
		[MyCmpGet] private KBatchedAnimController kbac;

		[SerializeField] public float acidDamageOnTick;

		private int acidIdx;
		public bool isHurtByAcid;
		public bool sparksOnContact;

		public const float FLASH_DURATION = 0.33f;
		private float flashElapsedTime = 0f;
		private Gradient flashGradient;

		public static readonly float explosiveDamageMultiplier = 10f;
		public static readonly float damageBase = 5f;

		private static readonly Dictionary<string, float> acidVulnerabilities = new()
		{
			// Immunity
			{ DreckoPlasticConfig.ID, CONSTS.ACID_VULNERABILITY.IMMUNE },
			{ Integrations.IDS.VeryRealONIContentTheWikiSaysSo.HATCHGOLD, CONSTS.ACID_VULNERABILITY.IMMUNE },
			{ Integrations.IDS.VeryRealONIContentTheWikiSaysSo.HATCHGOLDBABY, CONSTS.ACID_VULNERABILITY.IMMUNE }
		};

		// mods: just add BTags.sparksOnAcid to your prefab
		private static readonly HashSet<string> metallicCritters = [
			Integrations.IDS.RollerSnakes.ROLLERSNAKE,
			Integrations.IDS.RollerSnakes.ROLLERSNAKEBABY,
			Integrations.IDS.RollerSnakes.ROLLERSNAKESTEEL,
			Integrations.IDS.RollerSnakes.ROLLERSNAKEBABY,
			HatchMetalConfig.ID,
			BabyHatchMetalConfig.ID,
			SweepBotConfig.ID,
			MorbRoverConfig.ID,
			ScoutRoverConfig.ID,
			// TODO: angular fish
			// TODO: stimbo
		];

		public static void OnAddPrefab(KPrefabID prefab)
		{
			if (prefab.TryGetComponent(out CreatureBrain brain))
			{
				var tag = prefab.PrefabID().ToString();

				if (!acidVulnerabilities.TryGetValue(tag, out float vulnerability))
					acidVulnerabilitiesBySpecies.TryGetValue(brain.species, out vulnerability);

				if (vulnerability > 0)
				{
					if (metallicCritters.Contains(tag))
						prefab.AddTag(BTags.sparksOnAcid);

					var damage = vulnerability * damageBase;
					if (prefab.HasTag(BTags.sparksOnAcid))
						damage *= explosiveDamageMultiplier;

					var acidVulnerableCreature = prefab.gameObject.AddComponent<AcidVulnerableCreature>();
					acidVulnerableCreature.acidDamageOnTick = damage;

					if (prefab.TryGetComponent(out Modifiers modifiers)
						&& prefab.TryGetComponent(out Attributes attributes))
					{
						modifiers.initialAttributes.Add(BAttributes.Critters.acidVulnerability.Id);
						var acidModifier = new AttributeModifier(
							BAttributes.Critters.acidVulnerability.Id,
							vulnerability,
							"Acid",
							false,
							false,
							true);

						attributes.Add(acidModifier);
					}
				}
			}
		}

		private static readonly Dictionary<Tag, float> acidVulnerabilitiesBySpecies = new()
		{
			{ GameTags.Creatures.Species.BeetaSpecies, CONSTS.ACID_VULNERABILITY.MILDLY_ANNOYING },
			{ GameTags.Creatures.Species.CrabSpecies, CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
			{ GameTags.Creatures.Species.DivergentSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.DreckoSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.GlomSpecies, CONSTS.ACID_VULNERABILITY.MILDLY_ANNOYING },
			{ GameTags.Creatures.Species.HatchSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.LightBugSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.MoleSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.MooSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.OilFloaterSpecies, CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
			{ GameTags.Creatures.Species.PacuSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.PuftSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ BTags.Species.snail, CONSTS.ACID_VULNERABILITY.PRETTY_BAD },
			{ GameTags.Creatures.Species.SquirrelSpecies, CONSTS.ACID_VULNERABILITY.BAD },
			{ GameTags.Creatures.Species.StaterpillarSpecies, CONSTS.ACID_VULNERABILITY.BAD },
		};

		private IEnumerator FlashGreen()
		{
			flashElapsedTime = 0;

			while (flashElapsedTime < FLASH_DURATION)
			{
				flashElapsedTime += 0.033f;
				kbac.HighlightColour = flashGradient.Evaluate(flashElapsedTime / FLASH_DURATION);
				yield return new WaitForSeconds(0.033f);
			}

			kbac.HighlightColour = Color.black;
			flashElapsedTime = 0;

			yield return null;
		}

		public override void OnPrefabInit()
		{
			acidIdx = ElementLoader.GetElementIndex(Elements.sulfurousWater);
			flashGradient = new Gradient();

			var colorKeys = new GradientColorKey[]
			{
				new(Color.black, 0f),
				new(Color.green, 0.4f),
				new(Color.white, 0.5f),
				new(Color.green, 0.6f),
				new(Color.black, 1f)
			};

			var alphaKeys = new GradientAlphaKey[]
			{
				new(1f, 0),
				new(1f, 1f),
			};

			flashGradient.SetKeys(colorKeys, alphaKeys);
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			/* if(!BeachedWorldManager.Instance.IsBeachedContentActive)
             {
                 isHurtByAcid = false;
                 return;
             }*/

			isHurtByAcid = acidDamageOnTick > 0;
			sparksOnContact = gameObject.HasTag(BTags.sparksOnAcid);

			if (TryGetComponent(out Attributes attributes) && BAttributes.Critters.acidVulnerability != null)
			{
				var acidAttribute = attributes.Get(BAttributes.Critters.acidVulnerability);
				if (acidAttribute != null)
					Log.Debug($"Spawned a {this.GetProperName()}, with acid vulnerability of " + acidAttribute.GetTotalValue());
				else
					Log.Debug($"Spawned a {this.GetProperName()}, but it has no acid vulnerable attribute.");
			}
		}

		public void Sim1000ms(float dt)
		{
			if (!isHurtByAcid)
				return;

			var cell = Grid.PosToCell(transform.position);
			if (Grid.ElementIdx[cell] == acidIdx)
			{
				var beforeAnim = kbac.CurrentAnim.name;
				var beforePlaymode = kbac.PlayMode;

				StartCoroutine(FlashGreen());

				health.Damage(acidDamageOnTick);

				kbac.Play("hit");

				if (beforeAnim != null)
				{
					kbac.Queue(beforeAnim, beforePlaymode, 1f, 0f);
				}

				if (sparksOnContact)
				{
					Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, cell, 0);
					FUtility.Utils.YeetRandomly(gameObject, true, 1, 2, false);
				}

				// TODO: sizzle
				// todo: acid resistance
			}
		}
	}
}
