using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCurtain : MonoBehaviour
{
    float m_BottomValue;
    bool m_Move = false;
    public float m_Speed;
    public Canvas m_Canvas;
    // Use this for initialization
    void Start()
    {
        m_BottomValue = m_Canvas.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Move)
        {
            if (transform.position.y <= m_BottomValue)
            {
                m_Move = false;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, m_BottomValue, gameObject.transform.position.z);
            }
            else
            {
                gameObject.transform.position -= new Vector3(0.0f, m_Speed * Time.deltaTime, 0.0f);
            }
        }

    }

    public void OnClickTask()
    {
        m_Move = true;
    }
}
