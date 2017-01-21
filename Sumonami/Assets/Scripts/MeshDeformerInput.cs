using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour
{
    public float force = 10f;
    public float forceOffset = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            handleInput();
        }
    }

    private void handleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            //MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            //if (deformer && deformer.isActiveAndEnabled)
            //{
            //    Vector3 point = hit.point;
            //    point += hit.normal * forceOffset;
            //    deformer.addDeformingForce(point, force);
            //}

            RippleDeformer ripple = hit.collider.GetComponent<RippleDeformer>();
            if (ripple)
            {
                ripple.hitMesh(hit);
            }
        }
    }
}
