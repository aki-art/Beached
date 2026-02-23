using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Buildings
{
	public class CritterFeederStorageBlock : KMonoBehaviour
	{
		[SerializeField] public string connectorDefId;
		[SerializeField] public CellOffset cellOffset;

		private IList<Tag> dummyElement;
		private BuildingDef connectorDef;
		private AttachedCritterFeeder cachedFeeder;


		[Serialize] private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

		public override void OnSpawn()
		{
			SpawnFeeder(Grid.OffsetCell(this.NaturalBuildingCell(), cellOffset));
		}

		public override void OnPrefabInit()
		{
			dummyElement =
			[
				SimHashes.Unobtanium.CreateTag()
			];

			connectorDef = Assets.GetBuildingDef(connectorDefId);
		}

		public KPrefabID GetConnectorBuilding() => connectorRef.Get();

		public override void OnCleanUp()
		{
			DestroyOrphanedConnectorBuilding();
		}

		public void DestroyOrphanedConnectorBuilding()
		{
			var building = GetConnectorBuilding();

			if (building == null)
				return;

			connectorRef.Set(null);
			cachedFeeder = null;

			GameScheduler.Instance.ScheduleNextFrame("Destroy Connector building", o =>
			{
				if (!(building != null))
					return;

				Util.KDestroyGameObject(building.gameObject);
			});
		}

		private void SpawnFeeder(int targetCell)
		{
			var generator = GetFeeder();
			GameObject feederGo = null;

			Log.Debug($"generator {generator == null}");
			Log.Debug($"connectorDef {connectorDef == null}");

			if (generator != null)
				feederGo = generator.gameObject;

			if (feederGo == null)
			{
				feederGo = connectorDef.Build(targetCell, Orientation.R180, null, dummyElement, GetComponent<PrimaryElement>().Temperature);

				Log.Debug($"gameObject {feederGo == null}");
				var feeder = feederGo.GetComponent<AttachedCritterFeeder>();
				Log.Debug($"feeder {feeder == null}");
				feeder.parent = new Ref<CritterFeederStorageBlock>(this);
				connectorRef = new Ref<KPrefabID>(feeder.GetComponent<KPrefabID>());

				feederGo.SetActive(true);
				//gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;

				feeder.enabled = false;
			}
		}

		public AttachedCritterFeeder GetFeeder()
		{
			if (cachedFeeder == null)
			{
				var kprefabId = connectorRef.Get();

				if (kprefabId != null)
					cachedFeeder = kprefabId.GetComponent<AttachedCritterFeeder>();
			}

			return cachedFeeder;
		}
	}
}
