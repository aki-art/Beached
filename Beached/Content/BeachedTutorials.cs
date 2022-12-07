using Beached.Content.Codex.Guides;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedTutorials : KMonoBehaviour
    {
        [MyCmpGet]
        private Notifier notifier;

        public static BeachedTutorials Instance;

        [SerializeField]
        private Dictionary<Topic, TutorialInfo> tutorials;

        public BeachedTutorials()
        {
            tutorials = new Dictionary<Topic, TutorialInfo>()
            {
                { Topic.CritterMorale, new TutorialInfo(CritterHappinessTutorial.ID) },
                { Topic.Mushroom, new TutorialInfo(MushroomTutorial.ID) }
            };
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Instance = this;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            Instance = null;
        }

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
                        ModDb.ModDb.BeachedTutorialMessage,
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
