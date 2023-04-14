using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
    internal class StemPiece : KMonoBehaviour
    {
        [MyCmpGet] private KBatchedAnimController kbac;

        [Serialize] public int stemIndex;
        [SerializeField] public int leafVariationCount;
        [SerializeField] public string prefix;

        public StemPiece()
        {
            stemIndex = -1;
        }

        public override void OnSpawn()
        {
            if (stemIndex == -1)
            {
                stemIndex = Random.Range(0, leafVariationCount);
            }

            kbac.Play(prefix + stemIndex, KAnim.PlayMode.Paused);
        }
    }
}
