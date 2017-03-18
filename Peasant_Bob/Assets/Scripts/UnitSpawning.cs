using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawning : MonoBehaviour {

    public GameObject unitPrefab;

    int pendingUnits = 0;
    float timeLeft = 0;

    bool paused = false;

	// Use this for initialization
	void Start () {
        StructureMenu structureMenu = GetComponent<StructureMenu>();

        StructureMenu.ClickFunction func1 = StartSpawning;
        StructureMenu.CancelFunction func2 = CancelSpawning;
        StructureMenu.PercentageDoneFunction func3 = PercentageDone;
        StructureMenu.NumberActiveFunction func4 = NumQueuedUnits;
        StructureMenu.AvaliableFunction func5 = CanSpawnMore;

        //structureMenu.AddMenuItem(StartSpawning, CancelSpawning, PercentageDone, NumQueuedUnits, CanSpawnMore, this);
        structureMenu.AddMenuItem(func1, func2, func3 ,func4, func5);
        timeLeft = unitPrefab.GetComponent<UnitInformation>().spawnTime;
    }

    void StartSpawning()
    {
        if(BedSystem.bedSystem.GetNumFreeBeds() < unitPrefab.GetComponent<UnitInformation>().requireBeds)
        {
            return;
        }

        pendingUnits++;
        if(pendingUnits == 1)
        {
            BedSystem.bedSystem.OccopyBed(unitPrefab.GetComponent<UnitInformation>().requireBeds);
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

        if (pendingUnits == 0)
        {
            timeLeft = unitPrefab.GetComponent<UnitInformation>().spawnTime;
        }
    }

    float PercentageDone()
    {
        return (unitPrefab.GetComponent<UnitInformation>().spawnTime - timeLeft)/ unitPrefab.GetComponent<UnitInformation>().spawnTime;
    }

    int NumQueuedUnits()
    {
        if (pendingUnits > 1)
        {
            return pendingUnits -1;
        }
        else
        {
            return 0;
        }
    }

    bool CanSpawnMore()
    {
        return BedSystem.bedSystem.GetNumFreeBeds() >= unitPrefab.GetComponent<UnitInformation>().requireBeds;
    }

    // Update is called once per frame
    void Update () {
        if (pendingUnits > 0)
        {
            if (paused)
            {
                if (BedSystem.bedSystem.GetNumFreeBeds() >= unitPrefab.GetComponent<UnitInformation>().requireBeds)
                {
                    paused = false;
                }
            }

            if (!paused)
            {
                timeLeft -= Time.deltaTime;

                if (timeLeft < 0)
                {
                    // Spawn

                    Instantiate(unitPrefab, transform.position + new Vector3(20, 0, 0), Quaternion.identity);

                    pendingUnits--;

                    if (pendingUnits > 0)
                    {
                        if (BedSystem.bedSystem.GetNumFreeBeds() < unitPrefab.GetComponent<UnitInformation>().requireBeds)
                        {
                            paused = true;
                            timeLeft += unitPrefab.GetComponent<UnitInformation>().spawnTime;
                        }
                        else
                        {
                            BedSystem.bedSystem.OccopyBed(unitPrefab.GetComponent<UnitInformation>().requireBeds);
                            timeLeft = unitPrefab.GetComponent<UnitInformation>().spawnTime;
                        }
                    }
                    else
                    {
                        timeLeft = unitPrefab.GetComponent<UnitInformation>().spawnTime;
                    }
                }
            }
        }
		
	}
}
