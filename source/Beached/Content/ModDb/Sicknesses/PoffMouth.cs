using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.ModDb.Sicknesses
{
	// Makes a duplicant "immune" to food morale modifiers
	public class PoffMouth : Sickness.SicknessComponent
	{
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Log.Debug($"{go.GetProperName()} got infected with Poffmouth");
			if (go.TryGetComponent(out Effects effects))
			{
				var set = new HashSet<string>();
				var originalMouthAccessory = "";
				var dbEffects = Db.Get().effects;

				foreach (var effectId in Edible.qualityEffects)
				{
					var effect = dbEffects.TryGet(effectId.Value);
					if (effect != null)
					{
						set.Add(effectId.Value);
						effects.AddImmunity(effect, PoffMouthSickness.ID);
					}
				}

				if (go.TryGetComponent(out Accessorizer accessorizer))
				{
					originalMouthAccessory = Db.Get().Accessories.Get(accessorizer.bodyData.mouth).Id;
					ReplaceAccessory(BAccessories.POFFMOUTH, accessorizer);
				}
				else
				{
					Log.Debug("NO ACCESSORISER");
				}

				return new Data()
				{
					effects = set,
					mouthId = originalMouthAccessory
				};
			}

			return null;
		}

		private class Data
		{
			public HashSet<string> effects;
			public string mouthId;
		}

		public override void OnCure(GameObject go, object instance_data)
		{
			if (go.TryGetComponent(out Effects effects) && instance_data is Data data)
			{
				var dbEffects = Db.Get().effects;
				foreach (var effectId in data.effects)
				{
					var effect = dbEffects.TryGet(effectId);
					if (effect != null)
					{
						effects.RemoveImmunity(effect, PoffMouthSickness.ID);
					}
				}

				if (go.TryGetComponent(out Accessorizer accessorizer))
				{
					ReplaceAccessory(data.mouthId, accessorizer);
				}
			}
		}

		private void ReplaceAccessory(string accessory, Accessorizer accessorizer)
		{
			var mouthSlot = Db.Get().AccessorySlots.Mouth;
			var newAccessory = mouthSlot.Lookup(accessory);
			var currentAccessory = accessorizer.GetAccessory(mouthSlot);
			Log.Debug($"replacing accessory from {currentAccessory.Id} to {accessory}");

			if (newAccessory == null)
			{
				Log.Warning($"Could not add accessory {accessory}, it was not found in the database.");
				return;
			}

			accessorizer.RemoveAccessory(currentAccessory);
			accessorizer.AddAccessory(newAccessory);
			accessorizer.ApplyAccessories();
		}

		public override List<Descriptor> GetSymptoms()
		{
			var symptoms = base.GetSymptoms() ?? [];
			symptoms.Add(new Descriptor(
				"Withered Tastebuds",
				"This Duplicant is unable to enjoy any food. No morale from food will be gained.")); // this doesn't even appear in
																									 // game but whatever, it's here

			return symptoms;
		}
	}
}
