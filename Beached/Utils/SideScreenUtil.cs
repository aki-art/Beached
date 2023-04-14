using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beached.Utils
{
    public class SideScreenUtil
    {

        public static void ListChildren(Transform parent, int level = 0, int maxDepth = 10)
        {
            if (level >= maxDepth) return;

            foreach (Transform child in parent)
            {
                Log.Debug(string.Concat(Enumerable.Repeat('-', level)) + child.name);
                ListChildren(child, level + 1);
            }
        }

        public static void AddClonedSideScreen<T>(string name, Type originalType)
        {
            if(DetailsScreen.Instance == null) return;

            var screens = DetailsScreen.Instance.sideScreens;
            var contentBody = DetailsScreen.Instance.sideScreenContentBody;

            if(screens == null || contentBody == null ) return;

            var oldPrefab = FindOriginal(originalType, screens);
            var newPrefab = Copy<T>(oldPrefab, contentBody, name, originalType);

            screens.Add(CreateNewSideScreen(name, newPrefab));
        }

        public static void AddCustomSideScreen<T>(string name, GameObject prefab)
        {
            if (DetailsScreen.Instance == null || DetailsScreen.Instance.sideScreens == null) return;

            var newScreen = prefab.AddComponent(typeof(T)) as SideScreenContent;
            DetailsScreen.Instance.sideScreens.Add(CreateNewSideScreen(name, newScreen));
        }

        private static SideScreenContent FindOriginal(Type type, List<DetailsScreen.SideScreenRef> screens)
        {
            var result = screens.Find(s => s?.screenPrefab.GetType() == type)?.screenPrefab;

            if (result == null)
                Debug.LogWarning("Could not find a sidescreen with the type " + type);

            return result;
        }

        private static SideScreenContent Copy<T>(SideScreenContent original, GameObject contentBody, string name, Type originalType)
        {
            var screen = Util.KInstantiateUI<SideScreenContent>(original.gameObject, contentBody).gameObject;
            UnityEngine.Object.Destroy(screen.GetComponent(originalType));

            var prefab = screen.AddComponent(typeof(T)) as SideScreenContent;
            prefab.name = name.Trim();

            screen.SetActive(false);
            return prefab;
        }

        private static DetailsScreen.SideScreenRef CreateNewSideScreen(string name, SideScreenContent prefab)
        {
            return new DetailsScreen.SideScreenRef
            {
                name = name,
                offset = Vector2.zero,
                screenPrefab = prefab
            };
        }
    }
}