using Database;
using System.Collections.Generic;

namespace Beached.Content.ModDb
{
	public class BRoomTypes
	{
		public static RoomType NatureVista;

		public static RoomConstraints.Constraint MAXIMUM_SIZE_256 = new(
			null,
			room => room.cavity.numCells <= 256,
			1,
			string.Format(global::STRINGS.ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, "256"),
			string.Format(global::STRINGS.ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, "256"));

		public static RoomConstraints.Constraint NATURAL_POI = new(
			null,
			HasNaturalPOI,
			1,
			STRINGS.ROOMS.CRITERIA.NATURAL_POI.NAME,
			STRINGS.ROOMS.CRITERIA.NATURAL_POI.DESCRIPTION);

		public static readonly RoomDetails.Detail HAS_NATURAL_POI = new(_ => STRINGS.ROOMS.DETAILS.NATURAL_POI.NAME);

		private static bool HasNaturalPOI(Room room)
		{
			var pois = room.cavity.GetNaturePOIs();
			return pois != null && pois.Count > 0;
		}

		[DbEntry]
		public static void Register(RoomTypes __instance)
		{
			NatureVista = __instance.Add(new RoomType(
				"Beached_NatureVista",
				STRINGS.ROOMS.TYPES.BEACHED_NATUREVISTA.NAME,
				STRINGS.ROOMS.TYPES.BEACHED_NATUREVISTA.DESCRIPTION,
				STRINGS.ROOMS.TYPES.BEACHED_NATUREVISTA.TOOLTIP,
				STRINGS.ROOMS.TYPES.BEACHED_NATUREVISTA.EFFECT,
				Db.Get().RoomTypeCategories.Park,
				NATURAL_POI,
				[
					RoomConstraints.NO_INDUSTRIAL_MACHINERY,
					RoomConstraints.MINIMUM_SIZE_32,
					MAXIMUM_SIZE_256
				],
				[
					RoomDetails.SIZE,
					HAS_NATURAL_POI,
				],
				1,
				effects:
				[
					"RoomNatureReserve"
				],
				sortKey: 17));
		}

		public static void ModifyConstraintRules()
		{
			// make bed room constraits stomp rec rooms, this allows rec buildings to be in bedrooms
			RoomConstraints.BED_SINGLE.stomp_in_conflict ??= new List<RoomConstraints.Constraint>();
			RoomConstraints.BED_SINGLE.stomp_in_conflict.Add(RoomConstraints.REC_BUILDING);
			RoomConstraints.LUXURY_BED_SINGLE.stomp_in_conflict ??= new List<RoomConstraints.Constraint>();
			RoomConstraints.LUXURY_BED_SINGLE.stomp_in_conflict.Add(RoomConstraints.REC_BUILDING);
		}
	}
}
