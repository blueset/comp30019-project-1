using System;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeMeshable : MonoBehaviour {

    public float maxHeight = 100f;
    public float variation = 10f;
    public int detailLevel = 12;
    public Shader shader;
    public PhysicMaterial physicMaterial;
    public SunControl sun;
    public GameObject sea;

    public Color landColor = new Color(0.106f, 0.369f, 0.125f, 1.0f);
    public Color peakColor = new Color(0.878f, 0.878f, 0.878f, 1.0f);
    public Color beachColor = new Color(1f, 0.976f, 0.769f, 1.0f);

    // Use this for initialization
    void Start() {
        MeshFilter landscapeMesh = this.gameObject.AddComponent<MeshFilter>();
        landscapeMesh.mesh = BuildLandscapeMesh(BuildHeightMap());

        MeshRenderer mRenderer = this.gameObject.AddComponent<MeshRenderer>();
        // mRenderer.material.shader = Shader.Find("Unlit/VertexColorShader");
        mRenderer.material.shader = shader;

        transform.gameObject.AddComponent<MeshCollider>();
        MeshCollider mCollider = transform.GetComponent<MeshCollider>();
        mCollider.sharedMesh = landscapeMesh.mesh;
        mCollider.material = physicMaterial;

    }

    // Update is called once per frame
    void Update() {

        MeshRenderer mRenderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        mRenderer.material.SetColor("_PointLightColor", this.sun.color);
        mRenderer.material.SetVector("_PointLightPosition", this.sun.GetWorldPosition());
    }

    private Mesh BuildLandscapeMesh(float[,] heightMap) {
        Mesh mesh = new Mesh();
        int dimension = heightMap.GetLength(0);
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        float minLandscapeHeight = float.MaxValue, maxLandscapeHeight = float.MinValue;

        for (int i = 0; i < dimension; i++)
            for (int j = 0; j < dimension; j++) {
                vertices.Add(new Vector3(i - dimension / 2,
                                         heightMap[i, j],
                                         j - dimension / 2));
                minLandscapeHeight = Math.Min(heightMap[i, j], minLandscapeHeight);
                maxLandscapeHeight = Math.Max(heightMap[i, j], maxLandscapeHeight);
            }



        // triangle is in the order of
        // (i, j) - (i, j + 1) - (i + 1, j)
        // (i, j + 1) - (i + 1, j) - (i + 1, j + 1)

        for (int i = 0; i < dimension - 1; i++)
            for (int j = 0; j < dimension - 1; j++) {
                // Triangle 1
                triangles.Add(i * dimension + j);
                triangles.Add(i * dimension + j + 1);
                triangles.Add((i + 1) * dimension + j);

                // Triangle 2
                triangles.Add((i + 1) * dimension + j);
                triangles.Add(i * dimension + j + 1);
                triangles.Add((i + 1) * dimension + j + 1);
            }

        // Color
        for (int i = 0; i < vertices.Count; i++) {
            colors.Add(getColorByHeight(vertices[i].y,
                    minLandscapeHeight, maxLandscapeHeight));
            //vertices[i] = new Vector3(vertices[i].x, 0, vertices[i].z);
        }

        Vector3 seaPos = sea.transform.localPosition;
        seaPos.y = (float) ((maxLandscapeHeight - minLandscapeHeight) * 0.25 + minLandscapeHeight);
        sea.transform.localPosition = seaPos;
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    private float randomHeight() {
        return UnityEngine.Random.Range(-maxHeight, maxHeight);
    }

    private float randomVariation() {
        return UnityEngine.Random.Range(-variation, variation);
    }

    private float[,] BuildHeightMap() {
        float[,] heightMap = new float[2, 2];

        // Initialize height map
        heightMap[0, 0] = randomHeight();
        heightMap[0, 1] = randomHeight();
        heightMap[1, 0] = randomHeight();
        heightMap[1, 1] = randomHeight();

        heightMap = DiamondSquare(heightMap, detailLevel);

        return heightMap;
    }

    private float[,] DiamondSquare(float[,] heightMap, int level) {
        if (level == 0)
            return heightMap;
        int dimension = heightMap.GetLength(0);
        int nextSize = (dimension - 1) * 2 + 1;
        float[,] result = new float[nextSize, nextSize];

        // copy values to the next iteration.
        for (int i = 0; i < dimension; i++)
            for (int j = 0; j < dimension; j++)
                result[i * 2, j * 2] = heightMap[i, j];

        // Diamond step
        for (int i = 1; i < nextSize; i += 2)
            for (int j = 1; j < nextSize; j += 2)
                result[i, j] = (
                    result[i - 1, j - 1] +
                    result[i - 1, j + 1] +
                    result[i + 1, j + 1] +
                    result[i + 1, j - 1]
                ) / 4f + randomVariation() * level;

        // Square step
        for (int i = 0; i < nextSize; i++)
            for (int j = 1 - (i % 2); j < nextSize; j += 2) {
                int squareCnt = 0;
                if (i - 1 >= 0) {
                    result[i, j] += result[i - 1, j];
                    squareCnt++;
                }
                if (i + 1 < nextSize) {
                    result[i, j] += result[i + 1, j];
                    squareCnt++;
                }
                if (j - 1 >= 0) {
                    result[i, j] += result[i, j - 1];
                    squareCnt++;
                }
                if (j + 1 < nextSize) {
                    result[i, j] += result[i, j + 1];
                    squareCnt++;
                }
                result[i, j] = result[i, j] / squareCnt 
                    + randomVariation() * level;
            }

        return DiamondSquare(result, level - 1);
    }

    private Color getColorByHeight(float height, float minH, float maxH) {
        float percentage = (height - minH) / (maxH - minH);
        //return Color.HSVToRGB(0f, 0f, percentage);
        if (percentage > 0.7f) return peakColor;
        if (percentage > 0.3f) return landColor;
        return beachColor;
    }

}
