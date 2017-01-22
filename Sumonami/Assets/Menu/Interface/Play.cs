using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Play : MonoBehaviour
{
    public string m_SceneName;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickTask()
    {
        SceneManager.LoadScene(m_SceneName);
    }
}
