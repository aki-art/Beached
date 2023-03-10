using UnityEngine;

namespace Beached
{
    // Collection of methods which is promised to not change or break with update, allowing integration
    public class ModAPI
    {
        public static void AddBTag(GameObject gameObject, string tag) => gameObject.AddBTag(tag);

        public static void RemoveBTag(GameObject gameObject, string tag) => gameObject.RemoveBTag(tag);

        public static bool HasBTag(GameObject gameObject, string tag) => gameObject.HasBTag(tag);
    }
}
