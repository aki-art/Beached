using Beached.Content.ModDb;
using Beached.Content.ModDb.Germs;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	// TODO: UI strings 
	public class LimpetHost : GameStateMachine<LimpetHost, LimpetHost.Instance, IStateMachineTarget, LimpetHost.Def>
	{
		public State healthy;
		public State recovering;
		public InfectedState infected;

		public Signal sheared;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = healthy;

			recovering
				.EventHandlerTransition(GameHashes.EffectRemoved, healthy, RecoveryOver);

			healthy
				.UpdateTransition(infected, IsInfected, UpdateRate.SIM_4000ms);

			infected
				.DefaultState(infected.growing)
				.Enter(OnInfected)
				.Exit(OnCured)
				.OnSignal(sheared, healthy);

			infected.growing
				.UpdateTransition(infected.grown, UpdateScales);

			infected.grown
				.ToggleTag(GameTags.Creatures.ScalesGrown)
				.Update((smi, dt) => smi.PuffGerms(dt), UpdateRate.SIM_1000ms)
				.ToggleBehaviour(GameTags.Creatures.ScalesGrown, smi => true)
				.OnSignal(sheared, recovering);
		}

		private bool RecoveryOver(Instance smi, object _) => !smi.effects.HasEffect(BEffects.LIMPETHOST_RECOVERY);

		private bool UpdateScales(Instance smi, float _)
		{
			var value = smi.limpetGrowth.value;
			var level = (int)((smi.def.maxLevel * value) / 100f);

			if (smi.currentLimpetLevel != level)
			{
				smi.DropDisease();
				smi.currentLimpetLevel = Mathf.Min(level, smi.def.maxLevel);
				smi.UpdateSymbolOverride();
			}

			return value >= smi.limpetGrowth.GetMax();
		}

		private void OnInfected(Instance smi)
		{
			smi.InitLimpets();
			smi.UpdateSymbolOverride();

			smi.effects.Add(BEffects.LIMPETHOST, true);

			smi.gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = BDiseases.limpetEggs.Id;
		}

		private void OnCured(Instance smi)
		{
			if (smi.effects.HasEffect(BEffects.LIMPETHOST))
				smi.effects.Remove(BEffects.LIMPETHOST);

			smi.effects.Add(BEffects.LIMPETHOST_RECOVERY, true);

			smi.ClearLimpets();
			smi.UpdateSymbolOverride();

			if (smi.gameObject.TryGetComponent(out DiseaseSourceVisualizer visualizer))
				Object.DestroyImmediate(visualizer);
		}

		private bool IsInfected(Instance smi, float dt) => (float)GetLimpetCount(smi) > 0; // TODO: needs a chance and cooldown

		private float GetLimpetCount(Instance smi)
		{
			var cell = Grid.PosToCell(smi);
			var diseaseIdx = Grid.DiseaseIdx[cell];

			if (diseaseIdx == byte.MaxValue)
			{
				return 0;
			}

			var disease = Db.Get().Diseases[diseaseIdx].Id;
			if (disease != BDiseases.limpetEggs.id)
			{
				return 0;
			}

			return Grid.DiseaseCount[cell];
		}

		public class InfectedState : State
		{
			public State growing;
			public State grown;
		}

		public class Def : BaseDef
		{
			public int maxLevel;
			public float defaultGrowthRate;
			public Tag itemDroppedOnShear;
			public float massDropped;
			public int diseaseCount;
			public byte diseaseIdx;
			public float germPuffCooldown = CONSTS.CYCLE_LENGTH;
			public string targetSymbol = "beached_limpetgrowth";
			public string limpetKanim = "beached_pincher_limpetgrowth_kanim";
		}

		public new class Instance : GameInstance, IShearable
		{
			public KBatchedAnimController kbac;
			public Effects effects;
			public SymbolOverrideController symbolOverrideController;
			private readonly KAnim.Build.Symbol[] symbols;
			public AttributeModifier growingLimpetsModifier;
			public AmountInstance limpetGrowth;
			private HashedString targetSymbolHash;
			public bool isInfected;

			public int currentLimpetLevel = 0;
			private float timeSinceLastGermPuff = 0;

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				effects = GetComponent<Effects>();

				kbac = master.GetComponent<KBatchedAnimController>();
				targetSymbolHash = (HashedString)def.targetSymbol;

				var anim = Assets.GetAnim(def.limpetKanim);

				symbols = new KAnim.Build.Symbol[def.maxLevel + 1];
				for (int i = 0; i < symbols.Length; i++)
				{
					HashedString symbolName = $"beached_limpetgrowth_{i}";
					symbols[i] = anim.GetData().build.GetSymbol(symbolName);
				}

				symbolOverrideController = master.GetComponent<SymbolOverrideController>();
				currentLimpetLevel = -1;

				UpdateSymbolOverride();
			}

			public void InitLimpets()
			{
				var amounts = smi.gameObject.GetAmounts();
				limpetGrowth = amounts.Add(new AmountInstance(BAmounts.LimpetGrowth, master.gameObject));
				growingLimpetsModifier = new AttributeModifier(
					limpetGrowth.amount.deltaAttribute.Id,
					def.defaultGrowthRate,
					STRINGS.CREATURES.MODIFIERS.BEACHED_LIMPET_GROWTH_RATE.NAME);
				limpetGrowth.deltaAttribute.Add(growingLimpetsModifier);
			}

			public bool IsInfected() => currentLimpetLevel != -1;

			public void UpdateSymbolOverride()
			{
				symbolOverrideController.RemoveSymbolOverride(targetSymbolHash);
				kbac.SetSymbolVisiblity(targetSymbolHash, IsInfected());

				if (!IsInfected())
					return;

				symbolOverrideController.AddSymbolOverride(targetSymbolHash, symbols[currentLimpetLevel], 100);
			}

			public void ClearLimpets()
			{
				smi.gameObject.GetAmounts().Remove(limpetGrowth);
				currentLimpetLevel = -1;
				UpdateSymbolOverride();
				limpetGrowth.deltaAttribute.Remove(growingLimpetsModifier);
			}

			public void PuffGerms(float dt)
			{
				timeSinceLastGermPuff += dt;

				if (timeSinceLastGermPuff > def.germPuffCooldown)
					DropDisease();
			}

			public void DropDisease()
			{
				if (def.diseaseCount <= 0 || def.diseaseIdx == byte.MaxValue)
					return;

				var cell = Grid.PosToCell(this);

				if (!Grid.IsValidCell(cell))
					return;

				SimMessages.ModifyDiseaseOnCell(cell, def.diseaseIdx, def.diseaseCount);
				timeSinceLastGermPuff = 0;
			}

			public void Cure() => sm.sheared.Trigger(smi);

			public void Shear()
			{
				currentLimpetLevel = 0;
				limpetGrowth.value = 0;

				SpawnItem();
				Cure();
			}

			public bool IsFullyGrown() => currentLimpetLevel >= smi.def.maxLevel;

			private void SpawnItem()
			{
				var pe = smi.GetComponent<PrimaryElement>();

				var gameObject = FUtility.Utils.Spawn(def.itemDroppedOnShear, smi.master.gameObject);
				gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));

				if (gameObject.TryGetComponent(out PrimaryElement primaryElement))
				{
					primaryElement.Temperature = pe.Temperature;
					primaryElement.Mass = def.massDropped;
					primaryElement.AddDisease(pe.DiseaseIdx, pe.DiseaseCount, "Shearing");
				}

				gameObject.SetActive(true);

				FUtility.Utils.YeetRandomly(gameObject, true, 1, 3, false);
			}
		}
	}
}
