using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConstructionAgentSystem : ObjectAgentSystem {
    public float m_buildTime;
    public GameObject m_finalBuildingPrefab;
    
	// Use this for initialization
	void Start () {
        base.Start();
        Vector3 navMeshObstacleSize = new Vector3();
        navMeshObstacleSize.x = m_finalBuildingPrefab.GetComponent<StructureInformation>().gridSizeX;
        navMeshObstacleSize.z = m_finalBuildingPrefab.GetComponent<StructureInformation>().gridSizeY;
        navMeshObstacleSize.y = 5.0f;
        GetComponent<NavMeshObstacle>().size = navMeshObstacleSize;
        GameObject newAgent = AssignAgentToObject();
        newAgent.GetComponent<NavAgent>().SetDestination(FindBestConstructionPositionForAgent(newAgent), m_agentState);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject newAgent = AssignAgentToObject();
            newAgent.GetComponent<NavAgent>().SetDestination(FindBestConstructionPositionForAgent(newAgent), m_agentState);
        }
        int m_agentsAtConstructionSite = 0;
        foreach (var item in m_agents)
        {
            if (item.GetComponent<NavAgent>().GetAgentState() == AgentsManager.AgentStates.Construction)
            {
                m_agentsAtConstructionSite++;
            }
        }
        m_buildTime -= Time.deltaTime * m_agentsAtConstructionSite;
        if (m_buildTime <= 0)
        {
            GameObject tempObj = Instantiate(m_finalBuildingPrefab, transform.position, transform.rotation);

            // Building is done, release all agents...
            foreach (var item in m_agents)
            {
                m_agentManager.RemoveAgentFromTask(item, m_agentState);
            }
            m_agents.Clear();

            Destroy(gameObject);
        }
	}

    Vector3 FindBestConstructionPositionForAgent(GameObject p_agent)
    {
        Vector3 bestPosition = Vector3.zero;
        Vector3 agentPosition = p_agent.transform.position;
        Vector3 myPosition = transform.position;
        Vector3 navMeshSize = GetComponent<NavMeshObstacle>().size;
        float distance = float.MaxValue;
        // Find closest wall. We are counting on the fact that all walls are reachable, not good
        Vector3 side = myPosition;
        side.x += navMeshSize.x/2.0f;
        float distanceBetween = Vector3.Distance(side, agentPosition);
        if (distanceBetween < distance)
        {
            distance = distanceBetween;
            bestPosition = side;
        }

        side = myPosition;
        side.x -= navMeshSize.x / 2.0f;
        distanceBetween = Vector3.Distance(side, agentPosition);
        if (distanceBetween < distance)
        {
            distance = distanceBetween;
            bestPosition = side;
        }

        side = myPosition;
        side.z -= navMeshSize.z / 2.0f;
        distanceBetween = Vector3.Distance(side, agentPosition);
        if (distanceBetween < distance)
        {
            distance = distanceBetween;
            bestPosition = side;
        }

        side = myPosition;
        side.z += navMeshSize.z / 2.0f;
        distanceBetween = Vector3.Distance(side, agentPosition);
        if (distanceBetween < distance)
        {
            distance = distanceBetween;
            bestPosition = side;
        }
        
        return bestPosition;
    }
}
