using Beached.Content.ModDb;
using Klei.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	public class AcidVulnerableCreature : KMonoBehaviour, ISim1000ms//, IGameObjectEffectDescriptor//, IWiltCause
	{
		private int acidIdx;

		[MyCmpGet]
		private Health health;

		[MyCmpGet]
		private KBatchedAnimController kbac;

		[SerializeField]
		public float baseAcidDamage;

		public bool isHurtByAcid;

		public const float FLASH_DURATION = 0.33f;
		private float flashElapsedTime = 0f;
		private Gradient flashGradient;

		private static readonly Dictionary<string, float> acidVulnerabilities = new()
		{
			{ HatchMetalConfig.ID.ToString(), CONSTS.ACID_VULNERABILITY.EXTREME },
			{ DreckoPlasticConfig.ID, CONSTS.ACID_VULNERABILITY.IMMUNE },
			{ "RollerSnakeSteel", CONSTS.ACID_VULNERABILITY.EXTREME },
			{ "RollerSnake", CONSTS.ACID_VULNERABILITY.EXTREME },
		};

		public static void OnAddPrefab(KPrefabID prefab)
		{
			if (prefab.TryGetComponent(out CreatureBrain brain))
			{
				var vulnerability = 0f;

				if (acidVulnerabilities.TryGetValue(prefab.PrefabID().ToString(), out var v1))
				{
					vulnerability = v1;
				}
				else if (acidVulnerabilitiesBySpecies.TryGetValue(brain.species, out var v2))
				{
					vulnerability = v2;
				}

				if (vulnerability > 0)
				{
					prefab.gameObject.AddComponent<AcidVulnerableCreature>().baseAcidDamage = vulnerability;

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
			base.OnPrefabInit();
#if ELEMENTS
			acidIdx = Elements.sulfurousWater.Get().idx;
#endif

			flashGradient = new Gradient();

			var colorKeys = new GradientColorKey[]
			{
				new GradientColorKey(Color.black, 0f),
				new GradientColorKey(Color.green, 0.4f),
				new GradientColorKey(Color.white, 0.5f),
				new GradientColorKey(Color.green, 0.6f),
				new GradientColorKey(Color.black, 1f)
			};

			var alphaKeys = new GradientAlphaKey[]
			{
				new GradientAlphaKey(1f, 0),
				new GradientAlphaKey(1f, 1f),
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

			isHurtByAcid = baseAcidDamage > 0;


			if (TryGetComponent(out Attributes attributes) && BAttributes.Critters.acidVulnerability != null)
			{
				var acidAttribute = attributes.Get(BAttributes.Critters.acidVulnerability);
				if (acidAttribute != null)
				{
					Log.Debug($"Spawned a {this.GetProperName()}, with acid vulnerability of " + acidAttribute.GetTotalValue());
				}
				else
				{
					Log.Debug($"Spawned a {this.GetProperName()}, but it has no acid vulnerable attribute.");
				}
			}
		}

		public void Sim1000ms(float dt)
		{
#if !ELEMENTS
            return;
#endif
			if (!isHurtByAcid)
			{
				return;
			}

			var pos = Grid.PosToCell(transform.position);
			if (Grid.ElementIdx[pos] == acidIdx)
			{
				var beforeAnim = kbac.CurrentAnim.name;
				var beforePlaymode = kbac.PlayMode;

				StartCoroutine(FlashGreen());
				health.Damage(baseAcidDamage);

				kbac.Play("hit");

				if (beforeAnim != null)
				{
					kbac.Queue(beforeAnim, beforePlaymode, 1f, 0f);
				}
				// TODO: sizzle
				// todo: acid resistance
			}
		}
	}
}
