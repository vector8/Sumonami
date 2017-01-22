using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovLand : MonoBehaviour {

    public GameObject Player;
    public float weightCheck = 0.0f;
    public Transform origPos;
    public Transform newPos;
    public int TestLimit;
    float t = 0.0f;

	// Use this for initialization
	void Start () {
        transform.position = origPos.position;
	}
	
	// Update is called once per frame
	void Update () {

       

        if(weightCheck < TestLimit)
        {
            weightCheck = Player.GetComponent<Player>().getCurrentWeight();
        }
        else
        {
            transform.position = Vector3.Lerp(origPos.position, newPos.position, t);

            if (t < 1)
            {
                t += 0.005f;
            }
        }

        
        

	}
}
