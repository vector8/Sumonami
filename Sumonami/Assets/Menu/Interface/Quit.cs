using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{
    float m_Timer = 0.0f;
    bool m_CountDown = false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CountDown)
        {
            m_Timer += Time.deltaTime;
        }
        if (m_Timer > 3.0f)
        {
            Application.Quit();
        }
    }
    public void TaskOnClick()
    {
        m_CountDown = true; 
    }
}
