using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMeshable : MonoBehaviour {

    public float maxHeight = 100f;
    public int detailLevel = 12;
    public Shader shader;

    private Color LAND_COLOR = new Color(0.106f, 0.369f, 0.125f, 1.0f);
    private Color MOUNTAIN_COLOR = new Color(0.196f, 0.256f, 0.284f, 1.0f);
    private Color PEAK_COLOR = new Color(0.878f, 0.878f, 0.878f, 1.0f);


    // Use this for initialization
    void Start() {
        MeshFilter seaMesh = this.gameObject.AddComponent<MeshFilter>();
        seaMesh.mesh = BuildSeaMesh();

        MeshRenderer mRenderer = this.gameObject.AddComponent<MeshRenderer>();
        // mRenderer.material.shader = Shader.Find("Unlit/VertexColorShader");
        mRenderer.material.shader = shader;
    }

    // Update is called once per frame
    void Update() {

    }

    private Mesh BuildSeaMesh() {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();

        for (int i = -50; i < 50; i++)
            for (int j = -50; j < 50; j++) {
                float fi = (float) i, fj = (float) j;
                vertices.Add(new Vector3(fi, 0f, fj));
                vertices.Add(new Vector3(fi, 0f, fj + 1));
                vertices.Add(new Vector3(fi + 1, 0f, fj + 1));
                vertices.Add(new Vector3(fi, 0f, fj));
                vertices.Add(new Vector3(fi + 1, 0f, fj + 1));
                vertices.Add(new Vector3(fi + 1, 0f, fj));
            }

        for (int i = 0; i < vertices.Count; i++) {
            triangles.Add(i);
            colors.Add(new Color(0.506f, 0.831f, 0.980f, 0.35f));
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}
