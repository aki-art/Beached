using System;
using UnityEngine;

namespace Beached.Utils
{
    public class MockStructs
    {
        // GroundRenderer.Materials
        public struct Materials
        {
            public Material opaque;
            public Material alpha;

            public Materials(Material opaque, Material alpha)
            {
                this.opaque = opaque;
                this.alpha = alpha;
            }
        }

		// Game
		public enum SpawnRotationConfig
		{
			Normal,
			StringName
		}

		// Game
		[Serializable]
		public struct SpawnRotationData
		{
			public string animName;
			public bool flip;
		}

		// Game
		[Serializable]
		public struct SpawnPoolData
		{
			[HashedEnum]
			public SpawnFXHashes id;
			public int initialCount;
			public Color32 colour;
			public GameObject fxPrefab;
			public string initialAnim;
			public Vector3 spawnOffset;
			public Vector2 spawnRandomOffset;
			public SpawnRotationConfig rotationConfig;
			public SpawnRotationData[] rotationData;
		}
	}
}
