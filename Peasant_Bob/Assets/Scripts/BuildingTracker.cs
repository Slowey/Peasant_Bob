using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTracker : MonoBehaviour {
    public static BuildingTracker buildingTracker;
    Dictionary<Team.Teams, List<GameObject>> buildingsPerTeam = new Dictionary<Team.Teams, List<GameObject>>();
    // Use this for initialization
    void Awake() {
        if (buildingTracker != null)
        {
            Debug.Log("We have 2 building trackers");
            return;
        }
        buildingTracker = this;
    }

    public void RegisterNewBuilding(GameObject p_building, Team.Teams p_team)
    {
        if (!buildingsPerTeam.ContainsKey(p_team))
        {
            buildingsPerTeam.Add(p_team, new List<GameObject>());
        }
        buildingsPerTeam[p_team].Add(p_building);
    }

    public void RemoveBuilding(GameObject p_building, Team.Teams p_team)
    {
        int index = buildingsPerTeam[p_team].FindIndex(obj => obj.transform == p_building.transform);
        if (index == -1)
        {
            Debug.Log("OOO NO!!!! we should not get here!!!");
        }
        buildingsPerTeam[p_team].RemoveAt(index);
    }

    public List<GameObject> GetAllBuildingsOnTeam(Team.Teams p_team)
    {
        if (!buildingsPerTeam.ContainsKey(p_team))
        {
            return null;
        }
        return buildingsPerTeam[p_team];
    }

    public GameObject GetClosestBuildinOnTeam(Vector3 p_position, Team.Teams p_team)
    {
        if (!buildingsPerTeam.ContainsKey(p_team))
        {
            return null;
        }
        float shortestDistance = float.MaxValue;
        GameObject closestResource = null;
        foreach (var item in buildingsPerTeam[p_team])
        {
            float distance = Vector3.Distance(p_position, item.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestResource = item;
            }
        }
        return closestResource;
    }

    public int GetAmountOfBuildingOnTeam(Team.Teams p_team)
    {
        int amount = 0;
        if (!buildingsPerTeam.ContainsKey(p_team))
        {
            return amount;
        }
        amount = buildingsPerTeam[p_team].Count;
        return amount;
    }
}
