using Beached.Utils.GlobalEvents;
using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Beached.Content.BWorldGen
{
	public class ZoneTypes
	{
		//public static ZoneTypes2 zones;
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
			depths = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Depths");
			bamboo = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Bamboo");
			sea = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Sea");
			coralReef = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_CoralReef");
			icy = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Icy");
			beach = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Beach");
			sulfur = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Sulfur");
			bone = (ZoneType)Enum.Parse(typeof(ZoneType), "Beached_Bone");
		}
	}
}
