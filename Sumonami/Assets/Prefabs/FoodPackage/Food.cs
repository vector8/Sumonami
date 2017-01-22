using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float m_weight;
    public RippleDeformer rippleSurface;

    private float yOffset;

    // Use this for initialization
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        yOffset = (mesh.bounds.max.y - mesh.bounds.min.y) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rippleSurface != null && rippleSurface.isPositionOnRippleSurface(transform.position))
        {
            // Sample the height of the ripple surface
            float height = rippleSurface.sampleMeshHeight(transform.position);

            // Set our y position to the ripple surface height
            Vector3 pos = transform.position;
            pos.y = height + yOffset;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().addWeight(m_weight);
            Destroy(gameObject);
        }
    }
}
