using Database;
using Klei.AI;

namespace Beached.Content.ModDb
{
	public class BEmotes
	{
		public static Emote scared;
		public static Emote mucusSlip;

		[DbEntry]
		public static void Register(Emotes __instance)
		{
			scared = new Emote(
				__instance,
				"Beached_Emote_Scared",
				[
					new EmoteStep() { anim = "floor_floor_1_0_pre" },
					new EmoteStep() { anim = "floor_floor_1_0_loop" },
					new EmoteStep() { anim = "floor_floor_1_0_pst" },
				],
				"anim_loco_run_insane_kanim");

			mucusSlip = new Emote(
				__instance,
				"Beached_Emote_MucusSlip",
				[
					new EmoteStep() { anim = "portalbirth" }
				],
				"anim_interacts_portal_kanim");
		}
	}
}
