using Beached.Content.ModDb;
using Klei.AI;
using KSerialization;

namespace Beached.Content.Scripts
{
	public class Beached_GeyserTraits : BMonoBehavior
	{
		[Serialize] public bool hasInitializedTrait;
		[Serialize] public bool hasInitializedName;
		[MyCmpReq] public Geyser geyser;
		[MyCmpReq] public Traits traits;

		public override void OnSpawn()
		{
			if (!hasInitializedTrait)
			{
				if (!this.HasTag(BTags.geyserNoTraits))
					RollTrait();

				hasInitializedTrait = true;
			}
		}

		public void ModifyName()
		{
			if (isLoadingScene || hasInitializedName)
				return;

			if (TryGetComponent(out UserNameable userNameable))
			{
				string name = userNameable.savedName;

				foreach (var traitId in traits.TraitIds)
				{
					if (Strings.TryGet($"Beached.STRINGS.CREATURES.TRAITS.GEYSERS.{traitId.ToUpperInvariant()}.PREFIX", out var prefix))
					{
						name = $"{prefix} {name}";
					}
				}

				Log.Debug("set name to " + name);
				userNameable.SetName(name);
			}
		}

		private void RollTrait()
		{
			var traitId = BDb.geyserTraits.GetRandomTrait(geyser);
			if (traitId != null)
			{
				var trait = Db.Get().traits.TryGet(traitId);
				if (trait != null)
					GetComponent<Traits>().Add(trait);
			}
		}
	}
}
