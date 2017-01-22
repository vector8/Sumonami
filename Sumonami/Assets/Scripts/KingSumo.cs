using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSumo : MonoBehaviour
{
    public GameObject smokeParticlesPrefab;
    public float m_Speed;
    public float detectionDistance;
    public float m_Health;
    public bool m_IsGrounded;
    public float knockupThreshold = 1f;
    public float knockupPower = 10f;
    public float m_GroundCheckDistance = 0.3f;
    public float jumpTimer = 0f;
    public float JUMP_DELAY = 3f;
    public float m_JumpPower = 10f;
    public float damage = 40f;
    public Animator sumoAnimator;

    private GameObject m_PlayerRef;
    private Rigidbody m_rb;
    private RippleDeformer currentRippleSurface;
    private Vector3 m_GroundNormal;
    private float yOffset;
    private float height;
    bool immuneToKnockup = false;
    float immunityFromKnockupTimer = 0f;
    public float IMMUNITY_DURATION = 2f;
    float m_OrigGroundCheckDistance;
    private float damageTimer = 0f;
    private float damageDelay = 1.5f;

    // Use this for initialization
    void Start()
    {
        m_PlayerRef = GameObject.FindGameObjectWithTag("Player");
        m_rb = gameObject.GetComponent<Rigidbody>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        yOffset = 0;
        height = mesh.bounds.max.y - mesh.bounds.min.y;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    // Update is called once per frame
    void Update()
    {
        damageTimer -= Time.deltaTime;

        Vector3 direction = m_PlayerRef.transform.position - gameObject.transform.position;
        float distance = direction.magnitude;

        CheckGroundStatus();

        if (distance <= detectionDistance && m_IsGrounded)
        {
            direction.y = 0;
            direction = direction.normalized;
            gameObject.transform.forward = direction;

            jumpTimer += Time.deltaTime;
            if(jumpTimer >= JUMP_DELAY)
            {
                jumpTimer = 0f;
                m_rb.velocity = new Vector3(m_rb.velocity.x, m_JumpPower, m_rb.velocity.z);
                m_IsGrounded = false;
                m_GroundCheckDistance = 0.1f;
                m_rb.useGravity = true;
                sumoAnimator.Play("Player_Jump");
            }
            else
            {
                Vector3 velocity = gameObject.transform.forward * m_Speed;
                velocity.y = m_rb.velocity.y;
                m_rb.velocity = velocity;
                sumoAnimator.Play("Player_Run");
            }
        }
        else if(m_IsGrounded)
        {
            sumoAnimator.Play("Player_Idle");
        }
        else
        {
            m_GroundCheckDistance = m_rb.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
        }

        if (m_Health <= 0)
        {
            onDeath();
        }
    }

    void onDeath()
    {
        GameObject smoke = Instantiate<GameObject>(smokeParticlesPrefab);
        smoke.transform.position = transform.position;

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (collision.gameObject.transform.position.y > transform.position.y + 4f)
            {
                Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
                playerRB.velocity = new Vector3(playerRB.velocity.x, 10, playerRB.velocity.z);

                m_Health -= p.damage;

                if(m_Health <= 0)
                {
                    onDeath();
                }
            }
            else
            {
                if(damageTimer <= 0f)
                {
                    p.removeWeight(damage);
                    damageTimer = damageDelay;
                }
            }
        }
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
                if (!immuneToKnockup && height - currentRippleSurface.transform.position.y > knockupThreshold)
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
                    // Check if we can land (we are close to the ground and our velocity is downwards
                    if (distanceToGround <= m_GroundCheckDistance && m_rb.velocity.y <= 0)
                    {
                        // We've hit the ripple surface, grant us knockup immunity
                        immuneToKnockup = true;
                        immunityFromKnockupTimer = IMMUNITY_DURATION;
                        // Scale the mesh hit force based on the downward velocity

                        currentRippleSurface.splashDiameter = 25;
                        currentRippleSurface.hitMesh(hitInfo);

                        m_GroundNormal = hitInfo.normal;
                        m_IsGrounded = true;
                        m_rb.useGravity = false;

                        // Instantiate ground smash particles
                        GameObject g = Instantiate<GameObject>(smokeParticlesPrefab);
                        g.transform.position = transform.position;
                    }
                }
                else    // We are above a surface that cannot ripple
                {
                    //if (Vector3.Distance(hitInfo.point, transform.position) < m_GroundCheckDistance)
                    //{
                    //    m_GroundNormal = hitInfo.normal;
                    //    m_IsGrounded = true;
                    //    //m_Animator.applyRootMotion = true;
                    //}
                    //else
                    //{
                    //    m_IsGrounded = false;
                    //    m_GroundNormal = Vector3.up;
                    //    //m_Animator.applyRootMotion = false;
                    //}
                }
            }
        }
    }
}
