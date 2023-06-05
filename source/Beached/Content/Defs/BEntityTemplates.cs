using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static EdiblesManager;

namespace Beached.Content.Defs
{
    internal class BEntityTemplates
    {
        public static GameObject CreateFood(
            string ID,
            string anim,
            float width,
            float height,
            FoodInfo foodInfo)
        {
            var name = Strings.Get($"STRINGS.ITEMS.FOOD.{ID.ToUpperInvariant()}.NAME");
            var desc = Strings.Get($"STRINGS.ITEMS.FOOD.{ID.ToUpperInvariant()}.DESC");

            var prefab = EntityTemplates.CreateLooseEntity(
                ID,
                name,
                desc,
                1f,
                false,
                Assets.GetAnim(anim),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                width,
                height,
                true);

            var gameObject = EntityTemplates.ExtendEntityToFood(prefab, foodInfo);

            return gameObject;
        }
    }
}
