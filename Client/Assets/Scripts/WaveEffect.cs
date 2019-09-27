using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class WaveEffect : MonoBehaviour
{
    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        timer = 0.0f;
    }

    void Start()
    {
        mesh = new Mesh();
        mesh.name = "WaterEffect";

        int verticesCount = cellX * cellZ * 6;
        Vector3[] vertices = new Vector3[verticesCount];
        Vector3[] normals = new Vector3[verticesCount];
        Vector2[] uvs = new Vector2[verticesCount];
        int[] indices = new int[verticesCount];
        randomWeight = new float[verticesCount];

        Vector2 center = new Vector2((float)(cellX - 1) * 0.5f, (float)(cellZ - 1) * 0.5f);
        float halfCellSize = cellSize * 0.5f;
        int rowCount = cellX * 6;

        for (int i = 0; i < randomWeight.Length; ++i)
        {
            randomWeight[i] = 1.0f;
        }

        float perX = 1.0f / (float)cellX;
        float perZ = 1.0f / (float)cellZ;

        for (int z = 0; z < cellZ; ++z)
        {
            float centerZ = ((float)z - center.y) * cellSize;
            for (int x = 0; x < cellX; ++x)
            {
                float centerX = ((float)x - center.x) * cellSize;

                Vector3 v0 = new Vector3(centerX - halfCellSize, 0.0f, centerZ - halfCellSize);
                Vector3 v1 = new Vector3(centerX - halfCellSize, 0.0f, centerZ + halfCellSize);
                Vector3 v2 = new Vector3(centerX + halfCellSize, 0.0f, centerZ - halfCellSize);
                Vector3 v3 = new Vector3(centerX + halfCellSize, 0.0f, centerZ + halfCellSize);
                Vector3 v4 = new Vector3(centerX + halfCellSize, 0.0f, centerZ - halfCellSize);
                Vector3 v5 = new Vector3(centerX - halfCellSize, 0.0f, centerZ + halfCellSize);

                int i0 = z * rowCount + x * 6 + 0;
                int i1 = z * rowCount + x * 6 + 1;
                int i2 = z * rowCount + x * 6 + 2;
                int i3 = z * rowCount + x * 6 + 3;
                int i4 = z * rowCount + x * 6 + 4;
                int i5 = z * rowCount + x * 6 + 5;

                normals[i0] = Vector3.up;
                normals[i1] = Vector3.up;
                normals[i2] = Vector3.up;
                normals[i3] = Vector3.up;
                normals[i4] = Vector3.up;
                normals[i5] = Vector3.up;

                uvs[i0] = new Vector2((float)x * perX, (float)z * perZ);
                uvs[i1] = new Vector2((float)x * perX, (float)(z + 1) * perZ);
                uvs[i2] = new Vector2((float)(x + 1) * perX, (float)z * perZ);
                uvs[i3] = new Vector2((float)(x + 1) * perX, (float)(z + 1) * perZ);
                uvs[i4] = new Vector2((float)(x + 1) * perX, (float)z * perZ);
                uvs[i5] = new Vector2((float)x * perX, (float)(z + 1) * perZ);

                vertices[i0] = v0;
                vertices[i1] = v1;
                vertices[i2] = v2;
                vertices[i3] = v3;
                vertices[i4] = v4;
                vertices[i5] = v5;

                indices[i0] = i0;
                indices[i1] = i1;
                indices[i2] = i2;
                indices[i3] = i3;
                indices[i4] = i4;
                indices[i5] = i5;
            }
        }

        for (int z = 0; z < cellZ; ++z)
        {
            for (int x = 0; x < cellX; ++x)
            {
                float r = UnityEngine.Random.Range(0.0f, 1.0f);

                randomWeight[z * rowCount + x * 6] = r;

                if (x > 0)
                {
                    int nx = x - 1;
                    randomWeight[z * rowCount + nx * 6 + 2] = r;
                    randomWeight[z * rowCount + nx * 6 + 4] = r;
                }
                if (z > 0)
                {
                    int nz = z - 1;
                    randomWeight[nz * rowCount + x * 6 + 5] = r;
                    randomWeight[nz * rowCount + x * 6 + 1] = r;
                }
                if (x > 0 && z > 0)
                {
                    int nx = x - 1;
                    int nz = z - 1;
                    randomWeight[nz * rowCount + nx * 6 + 3] = r;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = indices;

        filter.mesh = mesh;

        origPositions = vertices;

        dynamicPositions = vertices.Clone() as Vector3[];
        dynamicNormals = normals.Clone() as Vector3[];
    }

    static Vector3[] tempTriangle = new Vector3[3];

    void Update()
    {
        timer += Time.deltaTime;

        Vector2 center = new Vector2((float)(cellX - 1) * 0.5f, (float)(cellZ - 1) * 0.5f);
        int rowCount = cellX * 6;
        int triangleCount = cellX * cellZ * 2;

        for (int t = 0; t < triangleCount; ++t)
        {
            for (int i = 0; i < 3; ++i)
            {
                Vector3 p = origPositions[t * 3 + i];

                float offsetX = Mathf.Cos(timer + (p.x + p.y) * 0.5f);
                float offsetY = Mathf.Sin(timer + (p.x + p.y) * 0.5f);
                float offsetZ = Mathf.Sin(timer + (p.x + p.y) * 0.5f);

                Vector3 pos = new Vector3(offsetX, offsetY, offsetZ) * randomWeight[t * 3 + i] + origPositions[t * 3 + i];
                dynamicPositions[t * 3 + i] = pos;
                tempTriangle[i] = pos;
            }
            Vector3 v1 = tempTriangle[1] - tempTriangle[0];
            Vector3 v2 = tempTriangle[2] - tempTriangle[0];

            Vector3 avgNormal = Vector3.Cross(v1, v2).normalized;

            dynamicNormals[t * 3 + 0] = avgNormal;
            dynamicNormals[t * 3 + 1] = avgNormal;
            dynamicNormals[t * 3 + 2] = avgNormal;
        }

        mesh.vertices = dynamicPositions;
        mesh.normals = dynamicNormals;
    }

    public int cellX = 64;
    public int cellZ = 64;
    public float cellSize = 5.0f;

    private Mesh mesh = null;
    new private MeshRenderer renderer = null;
    private MeshFilter filter = null;

    private float[] randomWeight = null;
    private Vector3[] origPositions = null;
    private float timer;

    private Vector3[] dynamicPositions = null;
    private Vector3[] dynamicNormals = null;
}
