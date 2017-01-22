using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingParticleTrail : MonoBehaviour {

    Rigidbody physicsBody;
    ParticleSystem flyingParticleSystem;
    public float triggerSpeed;
    public float scaleByObjectSpeed;
    public float maxScale;
	// Use this for initialization
	void Start ()
    {
        physicsBody = this.GetComponent<Rigidbody>();
        flyingParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        flyingParticleSystem.Stop();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float speed = Mathf.Abs(physicsBody.velocity.y);
        flyingParticleSystem.startSize = Mathf.Min(maxScale, scaleByObjectSpeed * speed);
        if (flyingParticleSystem.isPaused && (speed >= triggerSpeed)) //does c# do lazy evaluation from left to right or right to left
        {
            flyingParticleSystem.Play();
        } else flyingParticleSystem.Pause();

    }
}
