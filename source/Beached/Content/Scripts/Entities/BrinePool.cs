using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static MathUtil;

namespace Beached.Content.Scripts.Entities
{
	public class BrinePool : KMonoBehaviour, ISim1000ms, IGameObjectEffectDescriptor
	{
		[SerializeField] public float saltingCooldownS;
		[SerializeField] public MinMax saltingFrequencyS;
		[SerializeField] public CellOffset cellOffset;

		[Serialize] public float elapsedSinceLastSalt;
		[Serialize] public float nextSalt;

		private int targetCell;
		private float avgInterval;

		public override void OnSpawn()
		{
			base.OnSpawn();
			CacheTargetCell();
			avgInterval = saltingFrequencyS.Lerp(0.5f) + saltingCooldownS;
		}

		private void CacheTargetCell()
		{
			targetCell = Grid.OffsetCell(Grid.PosToCell(this), cellOffset);
		}

		public void Sim1000ms(float dt)
		{
			if (elapsedSinceLastSalt >= nextSalt)
			{
				if (transform.hasChanged)
					CacheTargetCell();

				Salt();

				elapsedSinceLastSalt = 0;
				nextSalt = saltingCooldownS + Random.Range(saltingFrequencyS.min, saltingFrequencyS.max);
			}

			elapsedSinceLastSalt += dt;
		}

		private void Salt()
		{
			if (!Grid.IsValidCell(targetCell))
				return;

			var occupyingElement = Grid.Element[targetCell];

			var result = GetSaltedElement(occupyingElement);

			if (result != SimHashes.Void)
				SimMessages.ReplaceElement(
					targetCell,
					result,
					CellEventLogger.Instance.DebugTool,
					Grid.Mass[targetCell],
					Grid.Temperature[targetCell],
					Grid.DiseaseIdx[targetCell],
					Grid.DiseaseCount[targetCell]);

			Game.Instance.SpawnFX(ModAssets.Fx.saltOff, Grid.CellToPosCTC(targetCell, Grid.SceneLayer.FXFront), 0f);
		}

		private static SimHashes GetSaltedElement(Element original)
		{
			return original.id switch
			{
				SimHashes.Water => SimHashes.SaltWater,
				SimHashes.SaltWater => SimHashes.Brine,
				SimHashes.DirtyWater => Random.value > 0.5f ? Elements.murkyBrine : SimHashes.Void, // the salt content jumps up, so we nudge it to be less frequent
				SimHashes.Oxygen => Elements.saltyOxygen,
				_ => SimHashes.Void,
			};
		}

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			var elem = Grid.Element[Grid.PosToCell(go)];
			var converted = GetSaltedElement(elem);

			if (converted != SimHashes.Void)
				return new List<Descriptor>()
				{
					new Descriptor(
						string.Format(STRINGS.CREATURES.SPECIES.OTHERS.BEACHED_BRINE_POOL.SALTOFF, elem.tag.ProperName(), converted.CreateTag().ProperName()),
						string.Format(STRINGS.CREATURES.SPECIES.OTHERS.BEACHED_BRINE_POOL.SALTOFF_TOOLTIP, GameUtil.GetFormattedTime(avgInterval)))
				};
			else
				return new List<Descriptor>()
				{
					new Descriptor("Idle", "")
				};
		}
	}
}
