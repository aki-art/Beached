using Database;
using HarmonyLib;
using static ModInfo;

namespace Beached.Patches
{
    public class ColonyDestinationSelectScreenPatch
    {
        [HarmonyPatch(typeof(ColonyDestinationSelectScreen), "OnAsteroidClicked")]
        public class ColonyDestinationSelectScreen_OnAsteroidClicked_Patch
        {
            public static void Postfix(ColonyDestinationSelectScreen __instance, ColonyDestinationAsteroidBeltData cluster)
            {
                // TODO: only for Astropelagos
                var stories = __instance.storyContentPanel;

                foreach (var story in Db.Get().Stories.resources)
                {
                    stories.SetStoryState(story.Id, StoryContentPanel.StoryState.Forbidden);
                }

                stories.RefreshAllStoryStates();
                stories.mainScreen.RefreshRowsAndDescriptions();
            }
        }
    }
}
