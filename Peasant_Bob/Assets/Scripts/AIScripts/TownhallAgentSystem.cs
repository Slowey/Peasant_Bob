﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownhallAgentSystem : ObjectAgentSystem {

    public float m_maxDistanceToGatherResources;
    private List<GameObject> m_idleAgentsAssigned = new List<GameObject>();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(m_agents.Count);
        if (m_idleAgentsAssigned.Count > 0)
        {
            ResourceManager resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
            GameObject closestResource = resourceManager.FindClosestResourceOfType(transform.position, m_maxDistanceToGatherResources, ResourceType.Wood);
            foreach (var item in m_idleAgentsAssigned)
            {
                item.GetComponent<NavAgent>().SetDestination(closestResource.transform.position, m_agentState);
            }
            m_idleAgentsAssigned.Clear();
        }
	}

    public void SendNewAgentToGatherResources()
    {
        ResourceManager resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        GameObject closestResource = resourceManager.FindClosestResourceOfType(transform.position, m_maxDistanceToGatherResources, ResourceType.Wood);
        if (closestResource == null)
        {
            GameObject newAgent = base.AssignAgentToObject();
            newAgent.GetComponent<NavAgent>().SetDestination(newAgent.transform.position, AgentsManager.AgentStates.Idle);
            m_idleAgentsAssigned.Add(newAgent);
        }
        else
        {
            GameObject newAgent = base.AssignAgentToObject();
            newAgent.GetComponent<NavAgent>().SetDestination(closestResource.transform.position, m_agentState);
        }
    }

    public void ReleaseAgent(GameObject p_agentToRelease)
    {
        int index = m_agents.FindIndex(obj => obj.transform == p_agentToRelease.transform);
        m_agents.RemoveAt(index);
    }
}
