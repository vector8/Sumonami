using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    public float springForce = 20f;
    public float damping = 5f;

    private Mesh deformingMesh;
    private Vector3[] originalVertices, displacedVertices, vertexVelocities;

    // Use this for initialization
    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];

        for(int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }

        vertexVelocities = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < displacedVertices.Length; i++)
        {
            updateVertex(i);
        }

        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    private void addForceToVertex(int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = originalVertices[i] - point;
        float attenuatedForce = force / (1f + pointToVertex.magnitude / 10f);
        float velocity = attenuatedForce * Time.deltaTime;
        pointToVertex.x = 0f;
        pointToVertex.z = 0f;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    private void updateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * Time.deltaTime;
    }

    public void addDeformingForce(Vector3 point, float force)
    {
        for (int i = 0; i < displacedVertices.Length; i++ )
        {
            addForceToVertex(i, point, force);
        }
        Debug.DrawLine(Camera.main.transform.position, point);
    }
}
