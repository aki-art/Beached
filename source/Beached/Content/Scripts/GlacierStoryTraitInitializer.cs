using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class GlacierStoryTraitInitializer : StoryTraitStateMachine<GlacierStoryTraitInitializer, GlacierStoryTraitInitializer.Instance, GlacierStoryTraitInitializer.Def>
	{
		public BoolParameter HasAnyBioBotBeenReleased;

		public override void InitializeStates(out BaseState default_state)
		{
			serializable = SerializeType.ParamsOnly;
			default_state = root;
		}

		public class Def : TraitDef
		{
			public override void Configure(GameObject prefab)
			{
				this.Story = BStories.Glaciers;
				this.CompletionData = new StoryCompleteData()
				{
					KeepSakeSpawnOffset = new CellOffset(0, 2),
					CameraTargetOffset = new CellOffset(0, 3)
				};
			}

		}

		public new class Instance : TraitInstance
		{
			private StoryInstance storyInstance;

			public Instance(StateMachineController master, Def def) : base(master, def)
			{
			}

			public override void StartSM()
			{
				base.StartSM();
				//this.machine = this.gameObject.GetSMI<MorbRoverMaker.Instance>();
				storyInstance = StoryManager.Instance.GetStoryInstance(BStories.Glaciers.HashId);
				CompleteEvent();
			}
		}
	}
}
