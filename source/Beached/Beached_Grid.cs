using Beached.Content;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Beached
{
	[SerializationConfig(MemberSerialization.OptIn)]
	[DefaultExecutionOrder(1)]
	[DisallowMultipleComponent]
	public class Beached_Grid : KMonoBehaviour
	{
		public const int INVALID_FORCEFIELD_OFFSET = -1;

		[Serialize] private Dictionary<int, NaturalTileInfo> naturalTiles = [];
		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] private bool initialized;

		public float[] electricity;

		public static Dictionary<Vector2I, ZoneType> worldgenZoneTypes;
		public static Dictionary<int, int> forceFieldLevelPerWorld = [];

		public static Beached_Grid Instance;

		public delegate void OnElectricChargeAddedEventHandler(int cell, float power);

		public OnElectricChargeAddedEventHandler OnElectricChargeAdded;

		public override void OnPrefabInit() => Instance = this;

		public void AddElectricCharge(int cell, float power)
		{
			electricity[cell] += power;
			OnElectricChargeAdded?.Invoke(cell, power);
		}

		public override void OnCleanUp() => Instance = null;

		public override void OnSpawn()
		{
			electricity = new float[Grid.CellCount];

			if (worldgenZoneTypes != null)
			{
				Log.Debug("beachedgrid has worldgen data " + worldgenZoneTypes.Count);
				foreach (var cell in worldgenZoneTypes)
				{
					zoneTypeOverrides[Grid.PosToCell(cell.Key)] = cell.Value;
				}

				//worldgenZoneTypes.Clear();
			}

			RegenerateBackwallTexture();
			World.Instance.zoneRenderData.OnActiveWorldChanged();
		}

		public static bool TryGetObject<ComponentType>(int cell, ObjectLayer layer, out ComponentType entity) where ComponentType : KMonoBehaviour
		{
			entity = null;
			return Grid.ObjectLayers[(int)layer].TryGetValue(cell, out var go) && go.TryGetComponent(out entity);
		}

		public static bool TryGetPlant<ComponentType>(int cell, out ComponentType entity) where ComponentType : KMonoBehaviour
		{
			return TryGetObject(cell, ObjectLayer.Plants, out entity);
		}

		public static int GetCellTowards(int cell, Direction direction, int distance)
		{
			return direction switch
			{
				Direction.Up => Grid.OffsetCell(cell, 0, distance),
				Direction.Down => Grid.OffsetCell(cell, 0, -distance),
				Direction.Left => Grid.OffsetCell(cell, -distance, 0),
				Direction.Right => Grid.OffsetCell(cell, distance, 0),
				Direction.None => cell,
				_ => -1,
			};
		}

		public static float GetElectricConduction(int cell)
		{
			return Grid.IsValidCell(cell)
				? Elements.electricConductivityLookup[Grid.ElementIdx[cell]]
				: 0;
		}

		public void RegenerateBackwallTexture()
		{
			if (World.Instance.zoneRenderData == null)
			{
				Debug.Log("Subworld zone render data is not yet initialized.");
				return;
			}

			var zoneRenderData = World.Instance.zoneRenderData;

			var zoneIndices = zoneRenderData.colourTex.GetRawTextureData();
			var colors = zoneRenderData.indexTex.GetRawTextureData();

			foreach (var tile in zoneTypeOverrides)
			{
				var cell = tile.Key;
				var zoneType = (byte)tile.Value;

				var color = World.Instance.zoneRenderData.zoneColours[zoneType];
				colors[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : zoneType;

				zoneIndices[cell * 3] = color.r;
				zoneIndices[cell * 3 + 1] = color.g;
				zoneIndices[cell * 3 + 2] = color.b;

				World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
			}

			zoneRenderData.colourTex.LoadRawTextureData(zoneIndices);
			zoneRenderData.indexTex.LoadRawTextureData(colors);
			zoneRenderData.colourTex.Apply();
			zoneRenderData.indexTex.Apply();

			zoneRenderData.OnShadersReloaded();
		}

		public bool TryGetNaturalTileInfo(int cell, out NaturalTileInfo info) => naturalTiles.TryGetValue(cell, out info);

		public void Initialize()
		{
			if (!initialized)
			{
				for (int cell = 0; cell < Grid.CellCount; cell++)
				{
					var element = Grid.Element[cell];
					if (element.IsSolid && element.id != SimHashes.Unobtanium)
					{
						naturalTiles[cell] = new NaturalTileInfo()
						{
							id = element.id,
							mass = Grid.Mass[cell]
						};
					}
				}

				initialized = true;
			}
		}

		[Serializable]
		public class NaturalTileInfo
		{
			public SimHashes id;
			public float mass;
		}
	}
}
