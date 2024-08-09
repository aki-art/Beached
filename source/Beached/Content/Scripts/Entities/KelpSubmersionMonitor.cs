using Beached.Content.Defs.Entities.Corals;
using System.Linq;

namespace Beached.Content.Scripts.Entities
{
	public class KelpSubmersionMonitor : KMonoBehaviour, ISlicedSim1000ms
	{
		[MyCmpReq] private SegmentedKelp kelp;

		public const float MINIMUM_WATER_MASS = 0.3f;

		public string WiltStateString { get; }

		public WiltCondition.Condition[] Conditions { get; }

		public override void OnSpawn()
		{
			base.OnSpawn();
			SlicedUpdaterSim1000ms<KelpSubmersionMonitor>.instance.RegisterUpdate1000ms(this);
		}

		public void SlicedSim1000ms(float dt)
		{
			if (!IsCellSubmerged(kelp.topCell))
			{
				// check the rest below
				int y = kelp.length - 1;
				while (y-- >= 0)
				{
					var cell = Grid.OffsetCell(Grid.PosToCell(this), 0, y);
					if (IsCellSubmerged(cell))
					{
						break;
					}
				}

				kelp.SetLength(y); // TODO: it should just wilt, and then die a while later
			}
		}

		public override void OnCleanUp()
		{
			SlicedUpdaterSim1000ms<KelpSubmersionMonitor>.instance.UnregisterUpdate1000ms(this);
			base.OnCleanUp();
		}

		public static bool IsCellSubmerged(int cell)
		{
			return Grid.IsValidCell(cell)
				&& CoralTemplate.ALL_WATERS.Contains(Grid.Element[cell].id)
				&& (Grid.Mass[cell] >= MINIMUM_WATER_MASS);
		}
	}
}
