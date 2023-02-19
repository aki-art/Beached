using UnityEngine;

namespace Beached.Content.Scripts
{
    internal class ForceFieldVisualizer : KMonoBehaviour
    {
        [MyCmpAdd] private MeshFilter meshFilter;
        [MyCmpAdd] private MeshRenderer meshRenderer;

        private Vector3[] vertices;
        private Vector2[] uvs;
        private int[] triangles;
        private Mesh mesh;
        
        public int ySize = 53;
        public int xSize = 20;

        public float offset = -32.01f;
        public float multiplier = 0.05f;
        public float multiplierb = 0.98f;
        public float funnelStrength = 0.05f;
        public float xOffset = 0.54f;
        
        public override void OnSpawn()
        {
            mesh = new();
            UpdateMesh();
            meshFilter.mesh = mesh;
            
            meshRenderer.material = new(ModAssets.Materials.forceField);
            meshRenderer.material.SetTexture("_MainTex", ModAssets.Textures.forceFieldGrid);
            meshRenderer.material.SetFloat("_ScrollSpeed", -6f);

            meshRenderer.material.mainTextureScale = new(xSize, ySize);

            transform.Rotate(-90, 0, 0);
            transform.localScale = new(10, 0.8f * 0.5f, 4.1f * 0.5f);
        }
        
        public void UpdateMesh()
        {
            vertices = new Vector3[(xSize + 1) * (ySize + 1)];
            uvs = new Vector2[vertices.Length];
            triangles = new int[xSize * ySize * 6];

            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (var x = 0; x <= xSize; x++, i++)
                {
                    var xo = (float)x * y * funnelStrength - xOffset * y;
                    var zo = -Mathf.Pow(multiplierb * y + offset, 2) * multiplier + ySize;
                    vertices[i] = new(xo, y, zo);
                    uvs[i] = new((float)x / xSize, (float)y / ySize);
                }
            }

            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (var x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}
