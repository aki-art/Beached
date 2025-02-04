using Beached.Content.ModDb;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts.Items
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_GeneticallyModifiableEgg : KMonoBehaviour, IGameObjectEffectDescriptor
	{
		[Serialize]
		public List<string> traitIds;

		[MyCmpReq] KSelectable kSelectable;

		public override void OnSpawn()
		{
			base.OnSpawn();
		}

		public void ApplyTrait(string traitId)
		{
			traitIds ??= [];
			if (!traitIds.Contains(traitId))
			{
				traitIds.Add(traitId);
			}

			if (!kSelectable.HasStatusItem(BStatusItems.geneticallyMofidied))
			{
				kSelectable.AddStatusItem(BStatusItems.geneticallyMofidied, this);
			}
		}

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			var descriptors = new List<Descriptor>();

			if (!Game.Instance.GameStarted())
				return descriptors;

			if (traitIds == null)
				return descriptors;

			var traits = Db.Get().traits;

			if (traits != null)
			{
				foreach (var traitId in traitIds)
				{
					var trait = traits.TryGet(traitId);
					if (trait != null)
						descriptors.Add(new Descriptor($"•   {trait.Name}", "desc"));
				}
			}

			return descriptors;
		}

		public string GetStatusItemString(string str)
		{
			var dbTraits = Db.Get().traits;
			var traits = "";

			foreach (var traitId in traitIds)
			{
				var trait = dbTraits.TryGet(traitId);
				if (trait != null)
				{
					traits += $"•   {trait.Name}\n";
				}
			}

			return string.Format(str, traits);
		}
	}
}
