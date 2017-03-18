using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawning : MonoBehaviour {

    public GameObject unitPrefab;

    int pendingUnits = 0;
    float timeLeft = 0;

	// Use this for initialization
	void Start () {
        StructureMenu structureMenu = GetComponent<StructureMenu>();

        StructureMenu.ClickFunction func1 = StartSpawning;
        StructureMenu.CancelFunction func2 = CancelSpawning;
        StructureMenu.PercentageDoneFunction func3 = PercentageDone;
        StructureMenu.NumberActiveFunction func4 = NumQueuedUnits;
        StructureMenu.AvaliableFunction func5 = CanSpawnMore;

        //structureMenu.AddMenuItem(StartSpawning, CancelSpawning, PercentageDone, NumQueuedUnits, CanSpawnMore, this);
        structureMenu.AddMenuItem(func1, func2, func3 ,func4 ,func5, this);

    }

    void StartSpawning()
    {
        pendingUnits++;
        if(pendingUnits == 1)
        {
            timeLeft = unitPrefab.GetComponent<UnitInformation>().spawnTime;
        }
    }

    void CancelSpawning()
    {
        if (pendingUnits > 0)
        {
            pendingUnits--;
            // Return resources
        }
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
        if (pendingUnits > 0)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft == 0)
            {
                // Spawn
                Debug.Log("Spawned!");

                pendingUnits--;

                if (pendingUnits > 0)
                {
                    timeLeft += unitPrefab.GetComponent<UnitInformation>().spawnTime;
                }
            }
        }
		
	}
}
