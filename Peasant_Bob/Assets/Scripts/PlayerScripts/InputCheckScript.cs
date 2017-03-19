﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheckScript : MonoBehaviour {

    public GameObject camera;


    // Run all input form here
    StructurePlacementSystem placeMentSystem;
    BuildableStructureSystem buildSelectSystem;
    UnitPlacementSystem unitPlacementSystem;

    StructureMenu activeMenu;
	// Use this for initialization
	void Start () {
        placeMentSystem = GetComponent<StructurePlacementSystem>();
        buildSelectSystem = GetComponent<BuildableStructureSystem>();
        unitPlacementSystem = GetComponent<UnitPlacementSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        unitPlacementSystem.UpdateSystem();
        if(buildSelectSystem.UpdateSystem())
        {
            // cancel others
            if(activeMenu != null)
                activeMenu.DestroyMenu();
            activeMenu = null;
        }
        if(placeMentSystem.UpdateSystem())
        {
            if (activeMenu != null)
                activeMenu.DestroyMenu();
            activeMenu = null;
        }
        if (Input.GetKeyDown(KeyCode.E) && activeMenu == null)
        {
            CheckRayCastHit("Buildings"); 
            placeMentSystem.CancelPlace();
            
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (unitPlacementSystem.UnitToPlace == null)
            {
                CheckRayCastHit("Units");              
            }
            else if (unitPlacementSystem.UnitToPlace != null)
            {
                unitPlacementSystem.PlaceUnit();
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeMenu != null)
            {
                if (activeMenu != null)
                    activeMenu.DestroyMenu();
                activeMenu = null;
            }
            placeMentSystem.CancelPlace();
            unitPlacementSystem.UnitToPlace = null;
        }

	}

    void CheckRayCastHit(string layer)
    {
        // Cast ray
        Vector3 direction;
        if (camera == null)
        {
            direction = transform.forward;
        }
        else
        {
            direction = camera.transform.forward;
        }

        Ray newRay = new Ray();
        newRay.origin = transform.position;
        newRay.direction = direction;
        RaycastHit hitInfo;

        if(Physics.Raycast(newRay, out hitInfo, 50, LayerMask.GetMask(layer)))
        {
            StructureMenu menu = hitInfo.transform.GetComponent<StructureMenu>();
            if (menu != null)
            {
                activeMenu = menu;
                activeMenu.CreateMenu();
            }
            NavAgent navigation = hitInfo.transform.GetComponent<NavAgent>();
            if (navigation != null && navigation.m_type != NavAgent.UnitType.Peasant)
            {
                unitPlacementSystem.UnitToPlace = navigation.gameObject;
            }
            // Else if on unti menu
        }
    }
}
