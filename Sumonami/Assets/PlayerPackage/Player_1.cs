using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1 : MonoBehaviour
{
    public float m_Speed;
    public float m_JumpHeight;
    public float m_TotalWeight;
    public int m_Health;
    public float m_RotationSpeed;
    public bool m_Grounded = true;
    Rigidbody m_RB;
    //Animator m_Animator;
    // Use this for initialization
    void Start()
    {
        //m_Animator = gameObject.GetComponent<Animator>();
        m_RB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 direction = new Vector2();
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * direction.y * m_Speed;
        m_RB.velocity = new Vector3(movement.x, m_RB.velocity.y, movement.z);
        transform.Rotate(0f, direction.x * m_RotationSpeed * Time.deltaTime, 0f);
       // if (Input.GetKey(KeyCode.W))
       // {
       //     Debug.Log("W key down");
       //     m_RB.velocity += transform.forward * m_Speed * Time.deltaTime;
       // }
       // if (Input.GetKey(KeyCode.S))
       // {
       //     m_RB.velocity -= transform.forward * m_Speed * Time.deltaTime;
       // }
       // if (Input.GetKey(KeyCode.D))
       // {
       //     transform.Rotate(0.0f, m_RotationSpeed * Time.deltaTime, 0.0f);
       // }
       // if (Input.GetKey(KeyCode.A))
       // {
       //     transform.Rotate(0.0f, -m_RotationSpeed * Time.deltaTime, 0.0f);
       // }
        if (Input.GetButtonDown("Jump") && m_Grounded)
        {
            //m_Animator.Play("Player_Jump");
            m_RB.velocity = new Vector3(m_RB.velocity.x, m_JumpHeight, m_RB.velocity.z);
            m_Grounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       float angle = Vector3.Angle(collision.contacts[0].normal, Vector3.up);
        if (angle < 20)
        {
            m_Grounded = true;
        }
    } 
}
