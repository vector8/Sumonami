using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingParticleTrail : MonoBehaviour {

    Rigidbody physicsBody;
    ParticleSystem flyingParticleSystem;
    public float triggerSpeed;
    public float scaleByObjectSpeed;
    public float maxScale;
    private float emitRate;
	// Use this for initialization
	void Start ()
    {
        physicsBody = this.GetComponent<Rigidbody>();
        flyingParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        emitRate = flyingParticleSystem.emissionRate;
        flyingParticleSystem.emissionRate = 0;
        flyingParticleSystem.Play();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float speed = physicsBody.velocity.magnitude;
        flyingParticleSystem.startSize = Mathf.Min(maxScale, scaleByObjectSpeed * speed);
        if (speed >= triggerSpeed) //does c# do lazy evaluation from left to right or right to left
        {
            flyingParticleSystem.emissionRate = emitRate;
        }
        else flyingParticleSystem.emissionRate = 0;
    }
}
