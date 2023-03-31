using FUtility.FUI;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Content.Scripts.UI
{
    public class BIconToggle : FToggle
    {
        private Image image;
        private LocText label;

        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            image = transform.Find("Image").GetComponent<Image>();
            label = transform.Find("Label").GetComponent<LocText>();
        }

        public void SetIcon(Sprite sprite)
        {
            if(image == null)
            {
                Log.Warning("BIconToggle image is null");
                return;
            }

            if(sprite == null)
            {
                image.sprite = Assets.GetSprite("unknown");
                return;
            }

            image.sprite = sprite;
        }

        public void SetLabel(string text)
        {
            if(label == null)
            {
                return;
            }

            label.text = text;
        }
    }
}
