using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float m_TotalWeight;
    public float MAX_WEIGHT;
    public float damage;
    public AudioClip eatFood;
    public AudioSource audio;

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void addWeight(float weight)
    {
        m_TotalWeight += weight;
        scaleUI.setWeight(m_TotalWeight / MAX_WEIGHT);
        audio.clip = eatFood;
        audio.Play();
    }

    public void removeWeight(float weight)
    {
        m_TotalWeight -= weight;
        if(m_TotalWeight <= 0)
        {
            m_TotalWeight = 0;
            SceneManager.LoadScene("MenuScene");
        }

        scaleUI.setWeight(m_TotalWeight / MAX_WEIGHT);
    }
}
