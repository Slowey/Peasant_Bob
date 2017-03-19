﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityTracker : MonoBehaviour {

    public Vector3 velocity;
    Vector3 lastPos;
    void Start()
    {
        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        velocity = (lastPos - transform.position)/ Time.fixedDeltaTime;
        lastPos = transform.position;
    }
}
