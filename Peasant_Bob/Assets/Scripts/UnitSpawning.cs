using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawning : MonoBehaviour {

    public GameObject unitPrefab;

	// Use this for initialization
	void Start () {
        StructureMenu structureMenu = GetComponent<StructureMenu>();

        structureMenu.AddMenuItem(StartSpawning, CancelSpawning, PercentageDone, NumQueuedUnits, CanSpawnMore);
    }

    void StartSpawning()
    {

    }

    void CancelSpawning()
    {

    }

    float PercentageDone()
    {
        return 0;
    }

    int NumQueuedUnits()
    {
        return 0;
    }

    bool CanSpawnMore()
    {
        return true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
