using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
	[SkipSaveFileSerialization]
	public class Beached_CritterMetabolismModifier : KMonoBehaviour, IGameObjectEffectDescriptor
	{
		public List<float> multipliers = [];
		private float multiplier = 1f;
		public AttributeInstance metabolism;
		public AttributeModifier metabolismModifier;

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return
			[
				new Descriptor($"This critter has {multiplier}X metabolism", "", Descriptor.DescriptorType.Effect)
			];
		}

		public void UpdateMetabolism(float multiplier)
		{
			multipliers.Add(multiplier);
			metabolism ??= gameObject.GetAttributes().Get(Db.Get().CritterAttributes.Metabolism); // this is an AddorGet call

			if (metabolism == null)
			{
				Log.Warning("this critter has no metabolism");
				return;
			}

			if (metabolismModifier == null)
			{
				metabolismModifier = new AttributeModifier(
					Db.Get().CritterAttributes.Metabolism.Id,
					0f,
					(string)global::STRINGS.DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME,
					true,
					is_readonly: false);

				metabolism.Add(metabolismModifier);
			}

			multiplier = 1f;
			multipliers.Do(n => multiplier *= n);

			metabolismModifier.SetValue(multiplier - 1f);
		}
	}
}
