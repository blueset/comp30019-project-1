using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeMeshable : MonoBehaviour {

    public float maxHeight = 100f;
    public int detailLevel = 12;
    public Shader shader;

    private Color LAND_COLOR = new Color(0.106f, 0.369f, 0.125f, 1.0f);
    private Color MOUNTAIN_COLOR = new Color(0.196f, 0.256f, 0.284f, 1.0f);
    private Color PEAK_COLOR = new Color(0.878f, 0.878f, 0.878f, 1.0f);


    // Use this for initialization
    void Start() {
        MeshFilter landscapeMesh = this.gameObject.AddComponent<MeshFilter>();
        landscapeMesh.mesh = BuildLandscapeMesh(BuildHeightMap());

        MeshRenderer mRenderer = this.gameObject.AddComponent<MeshRenderer>();
        // mRenderer.material.shader = Shader.Find("Unlit/VertexColorShader");
        mRenderer.material.shader = shader;
    }

    // Update is called once per frame
    void Update() {

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
        for (int i = 0; i < vertices.Count; i++)
            colors.Add(getColorByHeight(vertices[i].y,
                    minLandscapeHeight, maxLandscapeHeight));
        
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
                ) / 4f + randomHeight();

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
                result[i, j] = result[i, j] / squareCnt + randomHeight();
            }

        return DiamondSquare(result, level - 1);
    }

    private Color getColorByHeight(float height, float minH, float maxH) {
        float percentage = (height - minH) / (maxH - minH);
        if (percentage > 0.8f) return PEAK_COLOR;
        if (percentage > 0.2f) return MOUNTAIN_COLOR;
        return LAND_COLOR;
    }

}
