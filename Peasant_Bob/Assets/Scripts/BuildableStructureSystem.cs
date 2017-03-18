﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableStructureSystem : MonoBehaviour {

    public GameObject testObj;

    struct BuildableStructure
    {
        public GameObject structureObj;
        public KeyCode inputCode;
    }

    List<BuildableStructure> buildableStructures = new List<BuildableStructure>();
    KeyCode nextKeyCode = KeyCode.Alpha1;

    StructurePlacementSystem strucPlaceSys;

    // Use this for initialization
    void Start () {
        strucPlaceSys = GetComponent<StructurePlacementSystem>();


        AddStructure(testObj);
    }
	
	// Update is called once per frame
	void Update () {

        foreach (var item in buildableStructures) // Should be moved to some input system, or a call to here to cancel build when select weapon
        {
            if (Input.GetKeyDown(item.inputCode))
            {
                strucPlaceSys.StartPlacing(item.structureObj);
                break;
            }
        }
	}

    void AddStructure(GameObject structure)
    {
        BuildableStructure newBuild;
        newBuild.structureObj = structure;
        newBuild.inputCode = nextKeyCode;
        buildableStructures.Add(newBuild);
        nextKeyCode++;
    }
}
