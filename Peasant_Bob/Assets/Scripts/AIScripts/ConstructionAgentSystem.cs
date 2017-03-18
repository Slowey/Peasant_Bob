using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionAgentSystem : ObjectAgentSystem {
    public float m_buildTime;
    
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AssignAgentToObject();
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
            // Building is done, release all agents...
            foreach (var item in m_agents)
            {
                m_agentManager.RemoveAgentFromTask(item, m_agentState);
            }
            m_agents.Clear();
        }
	}
}
