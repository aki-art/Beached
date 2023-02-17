using KSerialization;
using UnityEngine;

namespace Beached.Content.Scripts.Entities
{
    internal class BambooStalkPiece : KMonoBehaviour
    {
        [MyCmpGet] private KBatchedAnimController kbac;

        [Serialize] public int stemIndex;
        [SerializeField] public int leafVariationCount;

        public BambooStalkPiece()
        {
            stemIndex = -1;
        }

        public override void OnSpawn()
        {
            if (stemIndex == -1)
            {
                stemIndex = Random.Range(0, leafVariationCount + 1);
            }

            kbac.Play("idle0_" + stemIndex, KAnim.PlayMode.Paused);
        }
    }
}
