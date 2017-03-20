using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainAgentSystem : ObjectAgentSystem {
    public NavAgent.UnitType m_unitType;
    public GameObject m_unitPrefab;
    public int m_unitCost;
    public float m_trainingTime;
    private float m_training;
    private int inQueue;
    private bool inTraining;
	// Use this for initialization
	void Start () {
        base.Start();
        StructureMenu structureMenu = GetComponent<StructureMenu>();
        structureMenu.AddMenuItem(StartSpawning, CancelSpawning, PercentageDone, NumQueuedUnits, CanSpawnMore);
        m_training = m_trainingTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (inQueue > 0 && m_agents.Count < m_maxAgentsAssigned && m_agentManager.UnitIsAvailableForWork())
        {
            GetAgentToTrain();
        }
        if (!inTraining && m_agents.Count > 0 && m_agents[0].GetComponent<NavAgent>().m_state == m_agentState)
        {
            inTraining = true;
            inQueue--;     
        }
        if (inTraining)
        {
            m_training -= Time.deltaTime;
            if (m_training <= 0)
            {
                Vector3 position = m_agents[0].transform.position;

                Destroy(m_agents[0]);
                m_agents.Clear();
                // Spawn dude
                Instantiate(m_unitPrefab, position, Quaternion.identity);
                BedSystem.bedSystem.OccopyBed(1);
                inTraining = false;
                m_training = m_trainingTime;
                m_agentManager.unitsOccupiedForTraining--;
            }
        }
        if (m_agentManager.GetUnitsAvailableForRecruiting() < 0)
        {
            CancelSpawning();
        }
	}

    void StartSpawning()
    {
        if (CanSpawnMore())
        {
            inQueue++;          
            GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().TakeResource(m_unitCost, ResourceType.Wood);
            m_agentManager.unitsOccupiedForTraining++;
        }
    }

    void CancelSpawning()
    {     
        if (inQueue > 0)
        {
            GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().GiveAmountOfType(m_unitCost, ResourceType.Wood);
            m_agentManager.unitsOccupiedForTraining--;
            inQueue--;
            if (inQueue == 0 && !inTraining)
            {
                foreach (var item in m_agents)
                {
                    m_agentManager.GetComponent<AgentsManager>().RemoveAgentFromTask(item, m_agentState);
                }
                m_agents.Clear();
            }
        }
        else if (inTraining && inQueue == 0)
        {
            foreach (var item in m_agents)
            {
                m_agentManager.GetComponent<AgentsManager>().RemoveAgentFromTask(item, m_agentState);
                inTraining = false;
            }
            m_agents.Clear();
            m_training = m_trainingTime;
            GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().GiveAmountOfType(m_unitCost, ResourceType.Wood);
            m_agentManager.unitsOccupiedForTraining--;
        }
        if (inQueue < 0)
        {
            inQueue = 0;
        }

        
    }

    float PercentageDone()
    {
        return 1.0f - Mathf.Max(0, m_training)/m_trainingTime;
    }

    int NumQueuedUnits()
    {
        return inQueue;
    }

    bool CanSpawnMore()
    {
        ResourceManager resourceManager = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>();
        AgentsManager agentsManager = m_agentManager.GetComponent<AgentsManager>();
        return agentsManager.GetUnitsAvailableForRecruiting() > 0 && resourceManager.ResourceExists(m_unitCost, ResourceType.Wood);
    }

    private void GetAgentToTrain()
    {
        GameObject newAgent = base.AssignAgentToObject();
        if (newAgent != null)
        {
            newAgent.GetComponent<NavAgent>().SetDestination(base.FindClosestPositionForAgent(newAgent), m_agentState);
            m_training = m_trainingTime;
        }
    }
}
