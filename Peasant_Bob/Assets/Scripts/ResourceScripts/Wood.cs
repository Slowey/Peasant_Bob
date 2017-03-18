using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("ResourceManager").GetComponent<ResourceManager>().AddNewResource(gameObject, ResourceType.Wood);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
