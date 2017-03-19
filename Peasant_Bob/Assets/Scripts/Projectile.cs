using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Rigidbody rig;
	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rig)
        {
            if (rig.velocity.magnitude > 0.01f)
            {
                transform.LookAt(transform.position + rig.velocity);
            }
        }
	}
}
