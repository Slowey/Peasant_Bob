using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAgentSystem : MonoBehaviour {
    public int m_maxAgentsAssigned;
    public AgentsManager.AgentStates m_agentState;
    public float m_OkToWorkDistance;
    protected List<GameObject> m_agents;
    protected AgentsManager m_agentManager;
	// Use this for initialization
	protected void Start () {
        m_agentManager = GameObject.Find("AgentManager").GetComponent<AgentsManager>();
        m_agents = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // This object calls the agent manager to get a free agent
    public void AssignAgentToObject()
    {
        if (m_agents.Count < m_maxAgentsAssigned)
        {
            GameObject newAgent = m_agentManager.AssignFreeAgentToTask(gameObject, m_agentState);
            m_agents.Add(newAgent);
            newAgent.GetComponent<NavAgent>().SetOkWorkDistance(m_OkToWorkDistance);
        }
    }
}
