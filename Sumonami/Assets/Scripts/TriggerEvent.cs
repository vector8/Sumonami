using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using UnityEngine.Events;
public class TriggerEvent : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter()
    {
        print("Working");

        if (OnTriggerEnterEvent != null)
        {
            OnTriggerEnterEvent.Invoke();
        }

    }
}
