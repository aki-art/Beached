using Beached.Utils.GlobalEvents;
using System;
using System.Collections.Generic;
using System.Reflection;
using static ProcGen.SubWorld;

namespace Beached.Content.BWorldGen
{
	public class ZoneTypes
	{
		public delegate Dictionary<string, ZoneType> GetZoneTypesDelegate();
		private static GetZoneTypesDelegate getZoneTypes;
		private static Dictionary<string, ZoneType> zoneTypes = [];

		public static HashSet<ZoneType> values = [];

		public static ZoneType
			beach,
			bamboo,
			bone,
			sulfur,
			depths,
			sea,
			coralReef,
			icy;


		[Subscribe(GlobalEvent.WORLD_RELOADED)]
		public static void OnWorldLoad()
		{
			if (getZoneTypes == null)
				InitializeMoonletAPI();

			zoneTypes = getZoneTypes?.Invoke();

			if (zoneTypes == null)
				Log.Warning("getZoneTypes is null");

			depths = zoneTypes["Beached_Depths"];
			bamboo = zoneTypes["Beached_Bamboo"];
			sea = zoneTypes["Beached_Sea"];
			coralReef = zoneTypes["Beached_CoralReef"];
			icy = zoneTypes["Beached_Icy"];
			beach = zoneTypes["Beached_Beach"];
			sulfur = zoneTypes["Beached_Sulfur"];
			bone = zoneTypes["Beached_Bone"];

			values = [.. zoneTypes.Values];
		}

		private static void InitializeMoonletAPI()
		{
			var APIType = Type.GetType("Moonlet.ModAPI, Moonlet");

			if (APIType == null) return;

			var m_GetZoneTypes = APIType.GetMethod("GetZoneTypes", BindingFlags.Static | BindingFlags.Public);
			getZoneTypes = (GetZoneTypesDelegate)Delegate.CreateDelegate(typeof(GetZoneTypesDelegate), m_GetZoneTypes);
		}
	}
}
