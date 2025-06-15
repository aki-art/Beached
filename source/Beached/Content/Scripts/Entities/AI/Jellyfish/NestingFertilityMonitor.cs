using Klei;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{
	public class NestingFertilityMonitor : GameStateMachine<NestingFertilityMonitor, NestingFertilityMonitor.Instance, IStateMachineTarget, NestingFertilityMonitor.Def>
	{
		private FertileStates fertile;
		private State infertile;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = fertile;
			serializable = SerializeType.Both_DEPRECATED;

			root
				.EventHandler(ModHashes.builtNest, OnNestBuild)
				.DefaultState(fertile);

			fertile
				.DefaultState(fertile.tooManyStrobila)
				.ToggleEffect((smi => smi.fertileEffect))
				.Transition(infertile, Not(IsFertile), UpdateRate.SIM_1000ms);

			fertile.tooManyStrobila
				.ToggleStatusItem("Crowded", "There are enough strobilas nearby.")
				.Transition(fertile.ready, HasNoMoreThan2NestsNearby, UpdateRate.SIM_1000ms);

			fertile.ready
				.ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, (smi => smi.IsReadyToNest()));

			infertile
				.Transition(fertile, IsFertile, UpdateRate.SIM_1000ms);
		}

		private void OnNestBuild(Instance smi)
		{
			smi.fertility.value = 0;
		}

		public class FertileStates : State
		{
			public State tooManyStrobila;
			public State ready;
		}

		private bool HasNoMoreThan2NestsNearby(Instance smi)
		{
			return true;
			/*var room = Game.Instance.roomProber.GetRoomOfGameObject(telepadInstance.gameObject);
			if (room == null)
				return true;

			var count = 0;
			foreach (var plant in room.plants)
			{
				if (plant.IsPrefabID(JellyfishStrobilaConfig.ID))
					count++;
			}

			var cellCount = room.cavity.numCells;

			return (float)cellCount / count <= telepadInstance.def.minCellPerStrobila;*/
		}

		public static bool IsFertile(Instance smi)
		{
			return !smi.HasTag(GameTags.Creatures.PausedReproduction)
				&& !smi.HasTag(GameTags.Creatures.Confined)
				&& !smi.HasTag(GameTags.Creatures.Expecting);
		}

		public class Def : BaseDef
		{
			public float baseFertileCycles;
			public int minCellPerStrobila;

			public override void Configure(GameObject prefab)
			{
				prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
			}
		}

		public new class Instance : GameInstance
		{
			public AmountInstance fertility;
			public Effect fertileEffect;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				fertility = Db.Get().Amounts.Fertility.Lookup(gameObject);

				if (GenericGameSettings.instance.acceleratedLifecycle)
					fertility.deltaAttribute.Add(new AttributeModifier(fertility.deltaAttribute.Id, 33.3333321f, "Accelerated Lifecycle"));

				var fertilityRate = (float)(100.0f / (def.baseFertileCycles * 600.0f));

				fertileEffect = new Effect("Fertile",
					global::STRINGS.CREATURES.MODIFIERS.BASE_FERTILITY.NAME,
					global::STRINGS.CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP,
					0.0f,
					true,
					false,
					false);

				fertileEffect.Add(new AttributeModifier(
					Db.Get().Amounts.Fertility.deltaAttribute.Id,
					fertilityRate,
					(string)global::STRINGS.CREATURES.MODIFIERS.BASE_FERTILITY.NAME));
			}

			public bool IsReadyToNest() => smi.fertility.value >= smi.fertility.GetMax();
		}
	}
}
