using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConstructionAgentSystem : ObjectAgentSystem {
    public float m_buildTime;
    public GameObject m_finalBuildingPrefab;
    private bool m_needFirstWorker = true;
    
	// Use this for initialization
	void Start () {
        base.Start();
        Vector3 navMeshObstacleSize = new Vector3();
        navMeshObstacleSize.x = m_finalBuildingPrefab.GetComponent<StructureInformation>().gridSizeX;
        navMeshObstacleSize.z = m_finalBuildingPrefab.GetComponent<StructureInformation>().gridSizeY;
        navMeshObstacleSize.y = 5.0f;
        GetComponent<NavMeshObstacle>().size = navMeshObstacleSize;
        GameObject newAgent = AssignAgentToObject();

        if (newAgent == null)
        {
            m_needFirstWorker = true;
        }
        else
        {
            m_needFirstWorker = false;
            newAgent.GetComponent<NavAgent>().SetDestination(FindClosestPositionForAgent(newAgent), m_agentState);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (m_needFirstWorker)
        {
            GameObject newAgent = AssignAgentToObject();
            if (newAgent == null)
            {
                m_needFirstWorker = true;
            }
            else
            {
                m_needFirstWorker = false;
                newAgent.GetComponent<NavAgent>().SetDestination(FindClosestPositionForAgent(newAgent), m_agentState);
            }
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


}
