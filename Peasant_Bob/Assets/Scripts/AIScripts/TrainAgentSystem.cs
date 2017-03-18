using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAgentSystem : ObjectAgentSystem {
    public NavAgent.UnitType m_unitType;
    public GameObject m_unitPrefab;
    public int m_unitCost;
    private int inQueue;
	// Use this for initialization
	void Start () {
        StructureMenu structureMenu = GetComponent<StructureMenu>();
        structureMenu.AddMenuItem(StartSpawning, CancelSpawning, PercentageDone, NumQueuedUnits, CanSpawnMore);
    }
	
	// Update is called once per frame
	void Update () {
        if (inQueue > 0 && m_agents.Count < m_maxAgentsAssigned && CanSpawnMore())
        {
            StartSpawning();
            inQueue--;     
        }
	}

    void StartSpawning()
    {
        if (CanSpawnMore())
        {
            GameObject newAgent = base.AssignAgentToObject();
            if (newAgent == null)
            {
                inQueue++;
            }
            else
            {
                GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().TakeResource(m_unitCost, ResourceType.Wood);
            }
        }
    }

    void CancelSpawning()
    {
        foreach (var item in m_agents)
        {
            GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().GiveAmountOfType(m_unitCost, ResourceType.Wood);
            GameObject.FindGameObjectWithTag("AgentManager").GetComponent<AgentsManager>().RemoveAgentFromTask(item, m_agentState);
        }
    }

    float PercentageDone()
    {
        return 0;
    }

    int NumQueuedUnits()
    {
        return inQueue;
    }

    bool CanSpawnMore()
    {
        ResourceManager resourceManager = GameObject.FindGameObjectWithTag("AgentManager").GetComponent<ResourceManager>();
        AgentsManager agentsManager = GameObject.FindGameObjectWithTag("AgentManager").GetComponent<AgentsManager>();
        return agentsManager.UnitIsAvailableForWork() && resourceManager.ResourceExists(m_unitCost, ResourceType.Wood);
    }
}
