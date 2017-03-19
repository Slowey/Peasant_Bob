using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    [System.Serializable]
    public class UnitSpawnInfo
    {
        public float waveSpawn; // Will be floored down
        public float waveSpawnIncrement;
        public GameObject unitPrefab;
    }

    public UnitSpawnInfo[] unitsToSpawn;
    public float timeBetweenWaves;
    public GameObject spawnArea;
    public Team.Teams enemyColor;
    public Transform enemyBaseCenter;
    private float timer;
    private float halfWidth;
    private float halfHeight;
    
	// Use this for initialization
	void Start () {
        timer = timeBetweenWaves;

        halfWidth = spawnArea.transform.lossyScale.x / 2;
        halfHeight = spawnArea.transform.lossyScale.z / 2;
    }
	
	// Update is called once per frame
	void Update () {
        if (BuildingTracker.buildingTracker.GetAmountOfBuildingOnTeam(enemyColor) > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {

                timer += timeBetweenWaves;
                foreach (var item in unitsToSpawn)
                {
                    for (int i = 0; i < (int)item.waveSpawn; i++)
                    {
                        Vector3 position = this.transform.position;
                        float additionX = Random.Range(-halfWidth, halfWidth);
                        float additionZ = Random.Range(-halfHeight, halfHeight);
                        position.x += additionX;
                        position.z += additionZ;

                        GameObject buildingToAttack = BuildingTracker.buildingTracker.GetClosestBuildinOnTeam(position, enemyColor);               
                        GameObject newUnit = Instantiate(item.unitPrefab, position, Quaternion.identity);

                        newUnit.GetComponent<Team>().team = GetComponent<Team>().team;
                        Vector3 positionToAttack = enemyBaseCenter.position;//buildingToAttack.GetComponent<ObjectAgentSystem>().FindClosestPositionForAgent(newUnit);
                        newUnit.GetComponent<NavAgent>().SetDestination(positionToAttack, AgentsManager.AgentStates.Fighting);
                    }
                    item.waveSpawn += item.waveSpawnIncrement;
                }
            }
        }
    }
}
