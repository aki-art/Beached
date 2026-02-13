using Beached.Content;
using Beached.Content.Scripts.Entities;
using HarmonyLib;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Beached.Content.Scripts.Buildings.Chime;
using static ProcGen.SubWorld;

namespace Beached
{
	[SerializationConfig(MemberSerialization.OptIn)]
	[DefaultExecutionOrder(1)]
	[DisallowMultipleComponent]
	public class Beached_Grid : KMonoBehaviour, ISim200ms, IRender200ms
	{
		public const int INVALID_FORCEFIELD_OFFSET = -1;

		[Serialize] private Dictionary<int, NaturalTileInfo> naturalTiles = [];
		[Serialize] public Dictionary<int, ZoneType> zoneTypeOverrides = [];
		[Serialize] private bool initialized;

		public float[] electricity;
		public Color[] lightColors;
		public static float[] flowSquaredCache;
		public static bool[] hasClimbable;

		public static Dictionary<Vector2I, ZoneType> worldgenZoneTypes;
		public static Dictionary<int, int> forceFieldLevelPerWorld = [];
		public static Dictionary<int, float> flowOverrides = [];

		public static Beached_Grid Instance;

		public delegate void OnElectricChargeAddedEventHandler(int cell, float power);
		public OnElectricChargeAddedEventHandler OnElectricChargeAdded;
		public bool electricityDirty;
		private static TextureBuffer electricityBuffer;
		public static Texture electricityTexture;

		[HarmonyPatch(typeof(PropertyTextures), "UpdateProperty")]
		public class PropertyTextures_UpdateProperty_Patch
		{
			public static void Postfix()
			{

			}
		}

		public void Render200ms(float dt)
		{
			UpdateTextures();
		}



		private static void UpdateTextures()
		{
			if (!Grid.IsInitialized())
				return;

			if (Game.Instance == null || Game.Instance.IsLoading())
				return;

			if (Game.Instance.IsPaused)
				return;

			// TODO: transpile in so this isnt double called
			PropertyTextures.instance.GetVisibleCellRange(out var x0, out var y0, out var x1, out var y1);

			var texture_region = electricityBuffer.Lock(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
			UpdateElectricTexture(texture_region, x0, y0, x1, y1);
			texture_region.Unlock();

			for (var i = 0; i < Grid.CellCount; i++)
			{
				flowSquaredCache[i] = GetFlowSqDirectly(i);
			}
		}

		[HarmonyPatch(typeof(PropertyTextures), "OnReset")]
		public class PropertyTextures_OnReset_Patch
		{
			public static void Postfix(PropertyTextures __instance)
			{
				//var grid = Beached_Grid.Instance;

				electricityBuffer = new TextureBuffer(
					"Beached_Electricity",
					Grid.WidthInCells,
					Grid.HeightInCells,
					TextureFormat.RGB24,
					FilterMode.Bilinear,
					__instance.texturePagePool);

				electricityTexture = electricityBuffer.texture;

				if (Beached_ElectricityRenderer.Instance != null && Beached_ElectricityRenderer.Instance.material != null)
					Beached_ElectricityRenderer.Instance.material.SetTexture("_Electricity", electricityTexture);
			}
		}


		[HarmonyPatch(typeof(PropertyTextures), "OnShadersReloaded")]
		public class PropertyTextures_OnShadersReloaded_Patch
		{
			public static void Postfix()
			{
				Beached_ElectricityRenderer.Instance.material.SetTexture("_Electricity", electricityTexture);
			}
		}

		private static void UpdateElectricTexture(TextureRegion region, int x0, int y0, int x1, int y1)
		{
			for (var y3 = y0; y3 <= y1; ++y3)
			{
				for (var x = x0; x <= x1; ++x)
				{
					var cell = Grid.XYToCell(x, y3);
					if (Grid.IsValidCell(cell) && Grid.IsActiveWorld(cell))
					{
						var test = Instance.electricity[cell];
						region.SetBytes(x, y3, (byte)(test * byte.MaxValue));
					}
				}
			}
		}

		public override void OnPrefabInit()
		{
			Instance = this;
		}

		public void AddElectricCharge(int cell, float power)
		{
			electricity[cell] += power;
			OnElectricChargeAdded?.Invoke(cell, power);
		}

		unsafe static public float GetFlowSqDirectly(int cell)
		{
			var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
			var flowTexVec = vecPtr[cell];
			var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

			return flowVec.sqrMagnitude;
		}

		/*		unsafe static public Vector2f GetFlowVector(int cell)
				{
					var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
					var flowTexVec = vecPtr[cell];
					var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

					var baseValue = flowVec;

					if (flowOverrides.TryGetValue(cell, out var modifier))
					{
						baseValue.X += modifier;
						baseValue.Y += modifier;
					}

					return baseValue;
				}*/


		unsafe static public float GetFlow(int cell)
		{
			/*			var vecPtr = (FlowTexVec2*)PropertyTextures.externalFlowTex;
						var flowTexVec = vecPtr[cell];
						var flowVec = new Vector2f(flowTexVec.X, flowTexVec.Y);

						var baseValue = flowVec.magnitude;*/

			if (flowSquaredCache == null)
				return 0;

			var value = flowSquaredCache[cell];
			var baseValue = Mathf.Sqrt(value);

			if (flowOverrides.TryGetValue(cell, out var modifier))
				baseValue += modifier;

			return baseValue;
		}

		public override void OnCleanUp() => Instance = null;

		public override void OnSpawn()
		{
			electricity = new float[Grid.CellCount];
			flowSquaredCache = new float[Grid.CellCount];

			flowOverrides = [];

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
				for (var cell = 0; cell < Grid.CellCount; cell++)
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

		public void Sim200ms(float dt)
		{
			// TODO: inefficient AH
			for (var i = 0; i < Grid.CellCount; i++)
				electricity[i] *= 0.95f;
		}

		[Serializable]
		public class NaturalTileInfo
		{
			public SimHashes id;
			public float mass;
		}
	}
}
