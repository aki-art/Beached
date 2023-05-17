using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ForceField : MonoBehaviour
{
    Vector3[] vertices;
    Vector2[] uvs;
    int[] triangles;
    private Mesh mesh;
    private bool dirty;
    
    [Range(1, 100)]
    public int height = 20;
    [Range(1, 100)]
    public int falloff = 5;
    [Range(1, 100)]
    public int width = 30;

    public float offset = 0;
    [Range(0f, 1f)]
    public float multiplier = 1;
    public float multiplierb = 2;
    public float funnelStrength = 2;
    public float xOffset = 2;

    private Material material;

    private void Start()
    {
        mesh = new Mesh();
        UpdateMesh();
        GetComponent<MeshFilter>().mesh = mesh;

        material = GetComponent<MeshRenderer>().sharedMaterials[0];
    }

    private void Update()
    {
        UpdateMesh();
    }
    
    public void UpdateMesh()
    {
        var xSize = width;
        var ySize = height + falloff;

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        uvs = new Vector2[vertices.Length];
        triangles = new int[xSize * ySize * 6];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                var xOffset = this.xOffset * y;
                var xo = (float)x * y * funnelStrength - xOffset;
                var yo = y;
                var zo = -Mathf.Pow(multiplierb * y + offset, 2) * multiplier + height;
                vertices[i] = new Vector3(xo, yo, zo);
                uvs[i] = new Vector2((float)x / xSize, (float)y / ySize);
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

        if (material == null)
        {
            material = GetComponent<MeshRenderer>().sharedMaterials[0];
        }

        material.mainTextureScale = new Vector2(xSize, ySize);
    }

/*    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }*/

    public void UpdateMesh2()
    {
        var xSize = width;
        var ySize = height + falloff;
        
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }
        
        triangles = new int[xSize * ySize * 6];
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

        uvs = new Vector2[vertices.Length];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (var x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uvs[i] = new Vector2((float)x / xSize, (float)y / ySize);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }
}
