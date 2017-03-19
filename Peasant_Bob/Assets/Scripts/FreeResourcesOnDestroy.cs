using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeResourcesOnDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        UnitInformation unitInfo = GetComponent<UnitInformation>();
        if (unitInfo)
        {
            BedSystem.bedSystem.FreeBeds(unitInfo.requireBeds);
        }
    }
}
