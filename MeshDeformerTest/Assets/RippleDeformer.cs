using UnityEngine;
using System.Collections;

public class RippleDeformer : MonoBehaviour
{

    private float[] buffer1;
    private float[] buffer2;
    private int[] vertexIndices;

    private Mesh mesh;

    private Vector3[] vertices;
    private Vector3[] tempVertices;
    //private Vector3[] normals ;

    public float dampener = 0.999f;
    public float maxWaveHeight = 2.0f;

    public int splashDiameter = 3;

    public float splashForce = 1000;

    public int cols = 128;
    public int rows = 128;

    // Use this for initialization
    void Start()
    {
        MeshFilter mf = (MeshFilter)GetComponent(typeof(MeshFilter));
        mesh = mf.mesh;
        vertices = mesh.vertices;
        tempVertices = new Vector3[vertices.Length];
        buffer1 = new float[vertices.Length];
        buffer2 = new float[vertices.Length];

        Bounds bounds = mesh.bounds;

        float xStep = (bounds.max.x - bounds.min.x) / cols;
        float zStep = (bounds.max.z - bounds.min.z) / rows;

        vertexIndices = new int[vertices.Length];
        int i = 0;
        for (i = 0; i < vertices.Length; i++)
        {
            vertexIndices[i] = -1;
            buffer1[i] = 0;
            buffer2[i] = 0;
        }

        // this will produce a list of indices that are sorted the way I need them to 
        // be for the algo to work right
        for (i = 0; i < vertices.Length; i++)
        {
            float column = ((vertices[i].x - bounds.min.x) / xStep);// + 0.5;
            float row = ((vertices[i].z - bounds.min.z) / zStep);// + 0.5;
            float position = (row * (cols + 1)) + column + 0.5f;
            if (vertexIndices[(int)position] >= 0) print("smash");
            vertexIndices[(int)position] = i;
        }
    }


    void splashAtPoint(int x, int y)
    {
        // Make sure splashRadius is an odd number
        if (splashDiameter % 2 == 0)
        {
            splashDiameter++;
        }

        float splashRadius = splashDiameter / 2f;

        int position = ((y * (cols + 1)) + x);

        for (int i = -(int)splashRadius; i <= (int)splashRadius; i++)
        {
            for(int j = -(int)splashRadius; j <= (int)splashRadius; j++)
            {
                buffer1[position + j * (cols + 1) + i] = splashForce;
            }
        }

        //buffer1[position] = splashForce;
        //buffer1[position - 1] = splashForce;
        //buffer1[position + 1] = splashForce;
        //buffer1[position + (cols + 1)] = splashForce;
        //buffer1[position + (cols + 1) + 1] = splashForce;
        //buffer1[position + (cols + 1) - 1] = splashForce;
        //buffer1[position - (cols + 1)] = splashForce;
        //buffer1[position - (cols + 1) + 1] = splashForce;
        //buffer1[position - (cols + 1) - 1] = splashForce;
    }

    // Update is called once per frame
    void Update()
    {
        // Process the ripples for this frame
        float[] tempBuffer;
        processRipples(buffer1, buffer2);
        // Swap the buffers so that buffer1 is always the current buffer.
        tempBuffer = buffer1;
        buffer1 = buffer2;
        buffer2 = tempBuffer;

        // Apply the ripples to this mesh's vertices
        int vertIndex;
        for (int i = 0; i < buffer1.Length; i++)
        {
            vertIndex = vertexIndices[i];
            tempVertices[vertIndex] = vertices[vertIndex];
            tempVertices[vertIndex].y += (buffer1[i] / splashForce) * maxWaveHeight;
        }
        mesh.vertices = tempVertices;
    }

    public void hitMesh(RaycastHit hit)
    {
        Bounds bounds = mesh.bounds;
        float xStep = (bounds.max.x - bounds.min.x) / cols;
        float zStep = (bounds.max.z - bounds.min.z) / rows;
        float xCoord = (bounds.max.x - bounds.min.x) - ((bounds.max.x - bounds.min.x) * hit.textureCoord.x);
        float zCoord = (bounds.max.z - bounds.min.z) - ((bounds.max.z - bounds.min.z) * hit.textureCoord.y);
        float column = (xCoord / xStep);// + 0.5;
        float row = (zCoord / zStep);// + 0.5;
        splashAtPoint((int)column, (int)row);
    }

    void processRipples(float[] source, float[] dest)
    {
        int x = 0;
        int y = 0;
        int position = 0;
        for (y = 1; y < rows - 1; y++)
        {
            for (x = 1; x < cols; x++)
            {
                position = (y * (cols + 1)) + x;
                dest[position] = (((source[position - 1] +
                                     source[position + 1] +
                                     source[position - (cols + 1)] +
                                     source[position + (cols + 1)]) / 2f) - dest[position]);
                dest[position] = dest[position] * dampener;
            }
        }
    }

}

