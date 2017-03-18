using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSystem : MonoBehaviour {

    public static BedSystem bedSystem;

    int occopiedBeds = 0;
    int maxBeds = 0;

    void Awake()
    {
        if (bedSystem != null)
        {
            Debug.LogError("Multiple bed systems!");
        }

        bedSystem = this;
    }

    int GetNumFreeBeds()
    {
        return maxBeds - occopiedBeds;
    }

    public void OccopyBed(int number)
    {
        occopiedBeds += number;
    }

    public void FreeBeds(int number)
    {
        occopiedBeds -= number;
    }

    public void AddBeds(int number)
    {
        maxBeds += number;
    }
    public void RemoveBeds(int number)
    {
        maxBeds -= number;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
