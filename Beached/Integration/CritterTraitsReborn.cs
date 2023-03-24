using System;

namespace Beached.Integration
{
    public class CritterTraitsReborn
    {
        public static AddTraitToVisibleListDelegate addTraitToVisibleList;
        public delegate void AddTraitToVisibleListDelegate(string traitID);

        public static void Initialize()
        {
            var type = Type.GetType("CritterTraitsReborn.ModAPI, CritterTraitsReborn");
            if(type != null)
            {
                var methodInfo = type.GetMethod("AddTraitToVisibleList", new[] { typeof(string) });
                addTraitToVisibleList = Delegate.CreateDelegate(typeof(AddTraitToVisibleListDelegate), methodInfo) as AddTraitToVisibleListDelegate;
            }
        }
    }
}
