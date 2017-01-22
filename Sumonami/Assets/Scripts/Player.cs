﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_TotalWeight;
    public float MAX_WEIGHT;
    public float damage;

    public ScaleUIControl scaleUI;

    // Use this for initialization
    void Start()
    {
        scaleUI.setWeight(m_TotalWeight / MAX_WEIGHT);
    }
    public float getCurrentWeight()
    {
        return m_TotalWeight;
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

    public void removeWeight(float weight)
    {
        m_TotalWeight -= weight;
        if(m_TotalWeight <= 0)
        {
            m_TotalWeight = 0;
            // TODO: die
        }

        scaleUI.setWeight(m_TotalWeight / MAX_WEIGHT);
    }
}
