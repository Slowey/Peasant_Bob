using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBeds : MonoBehaviour {
    public int bedAmount;

	// Use this for initialization
	void Start () {
        BedSystem.bedSystem.AddBeds(bedAmount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        BedSystem.bedSystem.RemoveBeds(bedAmount);
    }
}
