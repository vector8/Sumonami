using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerHit : MonoBehaviour
{
    public int m_Damage;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().m_Health -= m_Damage; 
        }
    }
}
