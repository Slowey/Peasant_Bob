﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterBuilding : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BuildingTracker.buildingTracker.RegisterNewBuilding(gameObject, GetComponent<Team>().team);
	}

    private void OnDestroy()
    {
        BuildingTracker.buildingTracker.RemoveBuilding(gameObject, GetComponent<Team>().team);
    }
}
