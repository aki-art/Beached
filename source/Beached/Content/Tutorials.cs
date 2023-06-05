using Beached.Content.Codex.Guides;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content
{
	[SkipSaveFileSerialization]
	public class Tutorials : KMonoBehaviour
	{
		[MyCmpGet]
		private Notifier notifier;

		[SerializeField]
		private Dictionary<Topic, TutorialInfo> tutorials;

		public static Tutorials Instance;

		public Tutorials()
		{
			tutorials = new Dictionary<Topic, TutorialInfo>()
			{
				{ Topic.CritterMorale, new TutorialInfo(CritterHappinessTutorial.ID) },
				{ Topic.Mushroom, new TutorialInfo(MushroomTutorial.ID) }
			};
		}

		public override void OnPrefabInit() => Instance = this;

		public override void OnCleanUp() => Instance = null;

		public void Test()
		{
			foreach (var topic in tutorials.Keys)
			{
				Show(topic);
			}
		}

		private void OnClick(string codexEntryID)
		{
			ManagementMenu.Instance.OpenCodexToEntry(codexEntryID);
		}

		public void Show(Topic topic, bool force = false)
		{
			if (tutorials.TryGetValue(topic, out var tutorial) && (!tutorial.playerSeen || force))
			{
				var entry = CodexCache.FindEntry(tutorial.codexEntryID);
				if (entry != null)
				{
					var notification = new Notification(
						entry.name,
						ModDb.BDb.BeachedTutorialMessage,
						custom_click_callback: obj => OnClick(tutorial.codexEntryID));
					notifier.Add(notification);
				}

				tutorial.playerSeen = true;
			}
		}

		public enum Topic
		{
			CritterMorale,
			Crystals,
			Depths,
			Genetics,
			Maki,
			Mushroom,
		}

		[Serializable]
		public struct TutorialInfo
		{
			public string codexEntryID;
			public bool playerSeen;

			public TutorialInfo(string codexEntryID)
			{
				this.codexEntryID = codexEntryID;
				playerSeen = false;
			}
		}
	}
}
