using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beached.Content.Scripts
{
    public class ParallaxBg : KMonoBehaviour
    {
        public void DeserializeFromJson()
        {
            if(TryGetComponent(out Text text))
            {
                var data = JsonConvert.DeserializeObject< Dictionary<string, float>>(text.text);

                if(data == null)
                {
                    Log.Warning($"Could not deserialize {this.PrefabID()}, data is null.");
                }

                foreach (Transform child in transform)
                {
                    if(data.TryGetValue(child.name, out float distance))
                    {
                        var image = child.GetChild(0);
                        if(image != null && image.TryGetComponent(out SpriteRenderer renderer))
                        {
                            Log.Debug($"Set layer properties of {child.name} - {distance}");
                            var properties = new MaterialPropertyBlock();
                            renderer.GetPropertyBlock(properties);
                            properties.SetFloat("_Distance", distance * 0.1f);
                            properties.SetTexture("_MainTex", renderer.sprite.texture);
                            renderer.SetPropertyBlock(properties);
                        }
                    }
                }
            }
        }

        [Serializable]
        public class LayerInfo
        {
            public string name;
            public float distance;
        }
    }
}
