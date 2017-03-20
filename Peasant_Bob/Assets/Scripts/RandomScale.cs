using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomScale : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float scale = Random.Range(0.75f, 3.0f);
        transform.localScale = new Vector3(scale, scale, scale);
	}
	
}
