using Beached.Content.ModDb;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;
using static Beached.Content.ModDb.LootTables;

namespace Beached.Content.Scripts.Entities.AI
{
	public class ShellGrowthMonitor : GameStateMachine<ShellGrowthMonitor, ShellGrowthMonitor.Instance, IStateMachineTarget, ShellGrowthMonitor.Def>
	{
		public State growing;
		public State grown;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = growing;

			growing
				.ToggleStatusItem("test", "")
				.ToggleStatusItem("ShellGrowthMonitor: growing", "")
				.Enter(smi => smi.ApplyOverrides())
				.ToggleAttributeModifier("growing shell", smi => smi.growingGrowthModifier)
				.UpdateTransition(grown, CheckGrowth);

			grown
				.ToggleStatusItem("ShellGrowthMonitor: grown", "")
				.ToggleTag(BTags.Creatures.grownShell)
				.ToggleBehaviour(BTags.Creatures.grownShell, smi => true);
		}

		private bool CheckGrowth(Instance smi, float dt)
		{
			var value = smi.shellGrowth.value;
			var level = (int)(smi.def.levelCount * value / 100f);

			if (smi.currentScaleLevel != level)
			{
				smi.currentScaleLevel = level;
				smi.ApplyOverrides();
			}

			Log.Debug($"value  {value}, max: {smi.shellGrowth.GetMax()}");
			return value >= smi.shellGrowth.GetMax();
		}


		public class Def : BaseDef, IGameObjectEffectDescriptor
		{
			public int levelCount;
			public float defaultGrowthRate;
			public float dropMass;
			public HashedString lootTableId;
			public SimHashes targetAtmosphere;

			public List<Descriptor> GetDescriptors(GameObject go)
			{
				return new List<Descriptor>();
			}

			public override void Configure(GameObject prefab)
			{
				prefab.GetComponent<Modifiers>().initialAmounts.Add(BAmounts.ShellGrowth.Id);
			}
		}

		public new class Instance : GameInstance
		{
			public AmountInstance shellGrowth;
			public AttributeModifier growingGrowthModifier;
			public AttributeModifier stuntedGrowthModifier;
			private KBatchedAnimController kbac;
			public SymbolOverrideController symbolOverrideController;

			public int currentScaleLevel = -1;
			protected static HashedString[] symbols =
			[
				"shell0",
				"shell1",
				"shell2",
				"shell3",
				"shell4",
				"shell5",
				"shell6",
				"shell7",
				"shell8"
			];

			public bool IsFullyGrown()
			{
				return currentScaleLevel == def.levelCount;
			}

			public void ApplyOverrides()
			{
				FUtility.Log.Debug("setting symbol to " + currentScaleLevel);

				var index = Mathf.Clamp(currentScaleLevel, 0, symbols.Length - 1);
				var symbol = kbac.AnimFiles[0].GetData().build.GetSymbol(symbols[index]);
				if (symbol != null)
				{
					symbolOverrideController.AddSymbolOverride("shell8", symbol);
				}
			}

			public void Mine()
			{
				currentScaleLevel = 0;
				shellGrowth.value = 0;
				smi.GoTo(smi.sm.growing);

				SpawnProduct();
			}

			private void SpawnProduct()
			{
				if (!BDb.lootTables.TryGetLoot<MaterialReward>(out var reward, def.lootTableId))
				{
					FUtility.Log.Warning("Could not spawn mite reward.");
					return;
				}

				var result = Util.KInstantiate(Assets.GetPrefab(reward.tag), null, null);

				var othersPrimaryElement = smi.GetComponent<PrimaryElement>();

				var primaryElement = result.GetComponent<PrimaryElement>();
				primaryElement.Temperature = othersPrimaryElement.Temperature;
				primaryElement.Mass = reward.mass;
				// TODO: this doesnt seem right ˇ
				primaryElement.AddDisease(othersPrimaryElement.DiseaseIdx, othersPrimaryElement.DiseaseCount, "Mined");

				result.SetActive(true);

				result.transform.position = transform.GetPosition();

				FUtility.Utils.YeetRandomly(result, true, 0.3f, 1.5f, false);
			}

			public Instance(IStateMachineTarget master, Def def) : base(master, def)
			{
				kbac = master.GetComponent<KBatchedAnimController>();
				shellGrowth = BAmounts.ShellGrowth.Lookup(gameObject);
				shellGrowth.value = 0;
				symbolOverrideController = master.GetComponent<SymbolOverrideController>();

				growingGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate, STRINGS.CREATURES.MODIFIERS.SHELLGROWTH.NAME);
				stuntedGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, 0, STRINGS.CREATURES.MODIFIERS.SHELLGROWTH.NAME);

				ApplyOverrides();
			}
		}
	}
}
