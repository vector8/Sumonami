using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTest : MonoBehaviour
{
    public float scale = 0.1f;
    public float speed = 1f;

    private Vector3[] baseHeight;
    private Mesh mesh;

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        baseHeight = mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = new Vector3[baseHeight.Length];

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];
            vertex.y = Mathf.Sin(Time.time * -speed + Mathf.Sqrt(vertex.x * vertex.x + vertex.z * vertex.z)) * scale;
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
