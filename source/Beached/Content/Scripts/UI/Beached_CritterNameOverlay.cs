using KSerialization;
using System.Collections;
using UnityEngine;

namespace Beached.Content.Scripts.UI
{
	/// <summary>
	/// Modifies CharacterOverlay so the name only appears once the player has actually renamed it, and not spam basic critter name
	/// Added at <see cref="Patches.EntityTemplatesPatch.EntityTemplates_ExtendEntityToBasicCreature_Patch.LatePostfix(GameObject)"/>
	/// null name input redirected to reset name at <see cref="Patches.DetailsScreenPatch.DetailsScreen_OnNameChanged_Patch"/>
	/// </summary>
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Beached_CritterNameOverlay : KMonoBehaviour
	{
		[MyCmpReq] private UserNameable userNameable;
		[Tooltip("if another mod added CharacterOverlay component first, then give up control of name visibility to them, and only bother with the tag.")]
		[SerializeField] public bool disableScreenControl;

		public override void OnPrefabInit()
		{
			Subscribe((int)GameHashes.NameChanged, OnNameChanged);
		}

		public override void OnSpawn()
		{
			StartCoroutine(UpdateNameCoroutine());
		}

		private bool IsDefaultName(string name)
		{
			return (name.IsNullOrWhiteSpace() || name.StartsWith("<link"));
		}

		private IEnumerator UpdateNameCoroutine()
		{
			yield return SequenceUtil.waitForEndOfFrame;
			yield return SequenceUtil.waitForEndOfFrame;

			OnNameChanged(userNameable.savedName);
		}

		private void OnNameChanged(object data)
		{
			if ((data is string userGivenName))
				ToggleName(!IsDefaultName(userGivenName));
		}

		private void ToggleName(bool enabled)
		{
			if (enabled)
				gameObject.AddTag(BTags.userNamedCritter);
			else
				gameObject.RemoveTag(BTags.userNamedCritter);

			if (!disableScreenControl)
			{
				var entry = NameDisplayScreen.Instance.GetEntry(gameObject);
				if (entry != null && entry.nameLabel != null)
				{
					entry.nameLabel.transform.parent.gameObject.SetActive(enabled);
					NameDisplayScreen.Instance.UpdateName(gameObject);
				}
			}
		}
	}
}
