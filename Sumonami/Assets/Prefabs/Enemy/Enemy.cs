using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject m_FoodPrefab;
    public float m_Speed;
    public float m_DistanceToAttack;
    public float detectionDistance;
    public int m_Health;
    public bool m_IsGrounded;
    public float knockupThreshold = 1f;
    public float knockupPower = 10f;
    public float m_GroundCheckDistance = 0.3f;
    public float dropDistanceToDie;

    private GameObject m_PlayerRef;
    private Rigidbody m_rb;
    private RippleDeformer currentRippleSurface;
    private Vector3 m_GroundNormal;
    private float yOffset;
    private bool willDie = false;

    // Use this for initialization
    void Start()
    {
        m_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        m_rb = gameObject.GetComponent<Rigidbody>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        yOffset = (mesh.bounds.max.y - mesh.bounds.min.y) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = m_PlayerRef.transform.position - gameObject.transform.position;
        float distance = direction.magnitude;

        CheckGroundStatus();

        if (distance <= detectionDistance && m_IsGrounded)
        {
            direction.y = 0;
            direction = direction.normalized;
            gameObject.transform.forward = direction;

            if (distance <= m_DistanceToAttack)
            {
                Attack();
            }
            else
            {
                Vector3 velocity = gameObject.transform.forward * m_Speed;
                velocity.y = m_rb.velocity.y;
                m_rb.velocity = velocity;
            }
        }

        if (m_Health <= 0)
        {
            onDeath();
        }
    }

    void Attack()
    {

    }

    void onDeath()
    {
        GameObject food = Instantiate<GameObject>(m_FoodPrefab);
        food.transform.position = transform.position;
        Food f = food.GetComponent<Food>();
        f.rippleSurface = currentRippleSurface;
        
        Destroy(gameObject);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character

        if (m_IsGrounded && currentRippleSurface != null)
        {
            if (currentRippleSurface.isPositionOnRippleSurface(transform.position))
            {
                // Sample the height of the ripple surface
                float height = currentRippleSurface.sampleMeshHeight(transform.position);

                // Set our y position to the ripple surface height
                Vector3 pos = transform.position;
                pos.y = height + yOffset;
                transform.position = pos;
                m_rb.useGravity = false;

                // if height is greater than knockup threshold, knock the character into the air
                if (height - currentRippleSurface.transform.position.y > knockupThreshold)
                {
                    m_rb.velocity = new Vector3(m_rb.velocity.x, knockupPower, m_rb.velocity.z);
                    m_IsGrounded = false;
                    //m_Animator.applyRootMotion = false;
                    m_GroundCheckDistance = 0.1f;
                    m_rb.useGravity = true;
                }
            }
            else
            {
                // We are no longer in the bounds of the ripple surface, set us to not be grounded so we can check if there is anything under us.
                currentRippleSurface = null;
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                //m_Animator.applyRootMotion = false;
                m_rb.useGravity = true;
            }
        }
        else if (!m_IsGrounded)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))
            {
                RippleDeformer ripple = hitInfo.collider.GetComponent<RippleDeformer>();
                if (ripple)  // We are above a surface that can ripple
                {
                    currentRippleSurface = ripple;
                    // Sample the height of the ripple surface
                    float height = currentRippleSurface.sampleMeshHeight(hitInfo);
                    float distanceToGround = transform.position.y - yOffset - height;
                    if (distanceToGround > dropDistanceToDie)
                        willDie = true;
                    // Check if we can land (we are close to the ground and our velocity is downwards
                    if (distanceToGround <= m_GroundCheckDistance && m_rb.velocity.y <= 0)
                    {
                        // We've hit the ripple surface, die
                        m_GroundNormal = hitInfo.normal;
                        m_IsGrounded = true;
                        ////m_Animator.applyRootMotion = true;
                        m_rb.useGravity = false;

                        if(willDie)
                        {
                            onDeath();
                        }
                    }
                }
                else    // We are above a surface that cannot ripple
                {
                    if (Vector3.Distance(hitInfo.point, transform.position) < m_GroundCheckDistance)
                    {
                        m_GroundNormal = hitInfo.normal;
                        m_IsGrounded = true;
                        //m_Animator.applyRootMotion = true;
                    }
                    else
                    {
                        m_IsGrounded = false;
                        m_GroundNormal = Vector3.up;
                        //m_Animator.applyRootMotion = false;
                    }
                }
            }
        }
    }
}
