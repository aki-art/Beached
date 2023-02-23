using ImGuiNET;
using UnityEngine;

namespace Beached.Content.Scripts
{
    public class ForceFieldVisualizer : KMonoBehaviour
    {
        [MyCmpAdd] private MeshFilter meshFilter;
        [MyCmpAdd] public MeshRenderer meshRenderer;

        private Vector3[] vertices;
        private Vector2[] uvs;
        private int[] triangles;
        private Mesh mesh;

        public int ySize = 53;
        public int xSize = 10;

        public static float offset = -42.36f;
        public static float multiplier = 0.029f;
        public static float funnelStrength = 0.1f;
        public static float xOffset = 0.5f;
        private static float multiplierb = 1.01f;

        private static float forceFieldBlur = 0.1f;
        private static float forceFieldAlphaMultiplier = 28f;

        public void CreateMesh()
        {
            // x offset. 0 for the generator being on the left, 1 for right
            var myWorld = this.GetMyWorld();
            Grid.PosToXY(transform.position, out var x, out _);
            var localX = x - myWorld.PosMin().x;
            var width = myWorld.Width;

            xOffset = localX / width;
            xOffset = Mathf.Clamp01(xOffset);

            mesh = new();
            UpdateMesh();
            meshFilter.mesh = mesh;

            meshRenderer.material = new(ModAssets.Materials.forceField);
            meshRenderer.material.SetTexture("_MainTex", ModAssets.Textures.forceFieldGrid);
            meshRenderer.material.SetTexture("_BlurTex", ModAssets.Textures.forceFieldBlurMap);
            meshRenderer.material.SetFloat("_ScrollSpeed", -6f);
            meshRenderer.material.SetFloat("_BlurSize", 0.1f);

            meshRenderer.material.mainTextureScale = new(xSize, ySize);

            transform.Rotate(-90, 0, 0);
            const float scale = 5f;
            transform.localScale = new(scale, 0.08f * scale, 0.34f * scale);
        }

        public void UpdateMesh()
        {
            vertices = new Vector3[(xSize + 1) * (ySize + 1)];
            uvs = new Vector2[vertices.Length];
            triangles = new int[xSize * ySize * 6];
            var highestY = -1;

            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (var x = 0; x <= xSize; x++, i++)
                {
                    var xo = (float)x * y * funnelStrength - xOffset * y;
                    var zo = -Mathf.Pow(multiplierb * y + offset, 2) * multiplier + ySize;
                    vertices[i] = new(xo, y, zo);
                    uvs[i] = new((float)x / xSize, (float)y / ySize);
                    highestY = Mathf.Max(highestY, (int)vertices[i].y);
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

            var myWorldId = this.GetMyWorldId();
            BeachedGrid.forceFieldLevelPerWorld[myWorldId] = highestY;
        }

        public override void OnCleanUp()
        {
            BeachedGrid.forceFieldLevelPerWorld[this.GetMyWorldId()] = BeachedGrid.INVALID_FORCEFIELD_OFFSET;
        }

        public void OnDebugSelected()
        {
            ImGui.TextColored(Color.yellow, "Rebuilding mesh every frame");

            ImGui.DragFloat("Blur amount", ref forceFieldBlur);
            ImGui.DragFloat("Alpha Multiplier", ref forceFieldAlphaMultiplier);
            meshRenderer.material.SetFloat("_BlurSize", forceFieldBlur);
            meshRenderer.material.SetFloat("_AlphaMultiplier", forceFieldAlphaMultiplier);


            ImGui.DragFloat("offset", ref offset, 1, -100, 100);
            ImGui.DragFloat("multiplier", ref multiplier, 0.01f, 0, 0.1f);
            ImGui.DragFloat("funnelStrength", ref funnelStrength,0.01f, 0, 1);
            ImGui.DragFloat("xOffset", ref xOffset, 0.01f, 0, 1);
            ImGui.DragFloat("multiplierb", ref multiplierb, 0.01f, 0.5f, 2);

            UpdateMesh();
        }
    }
}
