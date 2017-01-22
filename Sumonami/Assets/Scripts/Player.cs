using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float m_TotalWeight;
    public float MAX_WEIGHT;
    public float m_Health;

    public ScaleUIControl scaleUI;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addWeight(float weight)
    {
        m_TotalWeight += weight;
        scaleUI.setWeight(m_TotalWeight / MAX_WEIGHT);
    }
}
