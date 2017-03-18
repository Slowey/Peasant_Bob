using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectAgentSystem : MonoBehaviour {
    public int m_maxAgentsAssigned;
    public AgentsManager.AgentStates m_agentState;
    protected List<GameObject> m_agents = new List<GameObject>();
    protected AgentsManager m_agentManager;
	// Use this for initialization
	protected void Start () {
        GameObject[] agentMans = GameObject.FindGameObjectsWithTag("AgentManager");
        Team myTeam = GetComponent<Team>();

        for (int i = 0; i < agentMans.Length; i++)
        {
            Team agentTeam = agentMans[i].GetComponent<Team>();
            if (agentTeam.team == myTeam.team)
            {
                m_agentManager = agentMans[i].GetComponent<AgentsManager>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // This object calls the agent manager to get a free agent
    public GameObject AssignAgentToObject()
    {
        if (m_agentManager == null)
        {
            GameObject[] agentMans = GameObject.FindGameObjectsWithTag("AgentManager");
            Team myTeam = GetComponent<Team>();

            for (int i = 0; i < agentMans.Length; i++)
            {
                Team agentTeam = agentMans[i].GetComponent<Team>();
                if (agentTeam.team == myTeam.team)
                {
                    m_agentManager = agentMans[i].GetComponent<AgentsManager>();
                }
            }
        }

        GameObject newAgent = null;
        if (m_agents.Count < m_maxAgentsAssigned)
        {
            newAgent = m_agentManager.AssignFreeAgentToTask(gameObject, m_agentState);
            if (newAgent != null)
            {
                m_agents.Add(newAgent);
            }
        }
        return newAgent;
    }

    public Vector3 FindClosestPositionForAgent(GameObject p_agent)
    {
        Vector3 bestPosition = Vector3.zero;
        Vector3 agentPosition = p_agent.transform.position;
        Vector3 myPosition = transform.position;
        Vector3 navMeshSize = GetComponent<NavMeshObstacle>().size;
        float distance = float.MaxValue;
        // Find closest wall. We are counting on the fact that all walls are reachable, not good
        Vector3 side = myPosition;
        side.x += navMeshSize.x / 2.0f;
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
