using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI.Jellyfish
{
	public class CritterGeneratorSpawner : KMonoBehaviour, IPowerGeneratingEntity
	{
		[MyCmpReq] private PrimaryElement primaryElement;
		[MyCmpReq] private Effects effects;

		[SerializeField] public string connectorDefId;

		[Serialize] private Ref<KPrefabID> connectorRef;

		private AttributeModifier wildMod;
		private BuildingDef connectorDef;
		private CritterGeneratorBuilding cachedGenerator;

		public CritterGeneratorSpawner()
		{
			connectorRef = new Ref<KPrefabID>();
		}

		public void SpawnConnectorBuilding(int targetCell)
		{
			SpawnGenerator(targetCell);
		}

		private void SpawnGenerator(int targetCell)
		{
			var existingGenerator = GetGenerator();
			GameObject gameObject = null;

			if (existingGenerator != null)
				gameObject = existingGenerator.gameObject;

			if (gameObject == null)
			{
				gameObject = connectorDef.Build(
					targetCell,
					Orientation.R180,
					null,
					[SimHashes.Unobtanium.CreateTag()],
					primaryElement.Temperature);


				var generator = gameObject.GetComponent<CritterGeneratorBuilding>();
				generator.parent = new Ref<KPrefabID>(GetComponent<KPrefabID>());

				connectorRef = new Ref<KPrefabID>(generator.GetComponent<KPrefabID>());

				gameObject.SetActive(true);
				gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;

				generator.enabled = false;
			}

			var attributes = gameObject.gameObject.GetAttributes();

			var isWild = this.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0.0;

			if (isWild)
				attributes.Add(wildMod);

			bool isUnhappy = effects.HasEffect("Unhappy");

			var calorieMonitor = this.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
			if (!(calorieMonitor.IsHungry() | isUnhappy))
				return;

			var fullness = calorieMonitor.GetCalories0to1();
			var ratio = 1f;

			if ((double)fullness <= 0.0)
				ratio = isWild ? 0.1f : 0.025f;

			else if ((double)fullness <= 0.3)
				ratio = 0.5f;

			else if ((double)fullness <= 0.5)
				ratio = 0.75f;

			if ((double)ratio >= 1.0)
				return;

			var outputModifier = !isWild ? (float)((1.0 - (double)ratio) * 100.0) : Mathf.Lerp(0.0f, 25f, 1f - ratio);

			var modifier = new AttributeModifier(
				Db.Get().Attributes.GeneratorOutput.Id,
				-outputModifier,
				global::STRINGS.BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY);

			attributes.Add(modifier);
		}

		public override void OnPrefabInit()
		{
			wildMod = new(
				Db.Get().Attributes.GeneratorOutput.Id,
				-75f,
				global::STRINGS.BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD);

			connectorDef = Assets.GetBuildingDef(connectorDefId);
		}

		private void EnableGenerator()
		{
			var generator = GetGenerator();
			generator.enabled = true;
			generator.GetComponent<BuildingCellVisualizer>().enabled = true;
		}

		public CritterGeneratorBuilding GetGenerator()
		{
			if (cachedGenerator == null)
			{
				var connector = GetConnectorBuilding();
				if (connector != null)
					cachedGenerator = connector.GetComponent<CritterGeneratorBuilding>();
			}

			return cachedGenerator;
		}

		public bool IsConnectorBuildingSpawned() => GetConnectorBuilding() != null;

		public bool IsConnected() => GetGenerator().CircuitID != ushort.MaxValue;

		public KPrefabID GetConnectorBuilding() => connectorRef.Get();

		public void DestroyOrphanedConnectorBuilding()
		{
			var building = GetConnectorBuilding();

			if (building == null)
				return;

			connectorRef.Set(null);
			cachedGenerator = null;

			GameScheduler.Instance.ScheduleNextFrame("Destroy Critter Power Generator Connector building", o =>
			{
				if (building == null)
					return;

				Util.KDestroyGameObject(building.gameObject);
			});
		}
	}
}
