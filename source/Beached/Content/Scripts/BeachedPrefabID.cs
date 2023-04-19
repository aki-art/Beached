using KSerialization;
using System.Collections.Generic;

namespace Beached.Content.Scripts
{
    // as tags are limited, i place less important tags in here instead. mopt triggering the "out of tagbits" error
    [SerializationConfig(MemberSerialization.OptIn)]
    public class BeachedPrefabID : KMonoBehaviour
    {
        [Serialize]
        private HashSet<Tag> tags;

        public bool AddTag(Tag tag)
        {
            tags ??= new HashSet<Tag>();
            return tags.Add(tag);
        }

        public bool RemoveTag(Tag tag)
        {
            if (tags == null) return false;
            return tags.Remove(tag);
        }

        public bool HasTag(Tag tag)
        {
            if (tags == null) return false;
            return tags.Contains(tag);
        }
    }
}
