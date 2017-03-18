using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour {

    private NavMeshAgent agent;
    private AgentsManager.AgentStates m_state;
    private AgentsManager.AgentStates m_wantedState;
    private float m_okWorkDistance;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        GameObject.Find("AgentManager").GetComponent<AgentsManager>().AddNewAgent(gameObject);
    }

    void Update()
    {
        if (Vector3.Distance(agent.destination, transform.position) <= m_okWorkDistance)
        {
            m_state = m_wantedState;
        }
    }

    public void SetDestination(Vector3 p_position, AgentsManager.AgentStates p_wantedState)
    {
        m_state = AgentsManager.AgentStates.Walking;
        m_wantedState = p_wantedState;
        agent.SetDestination(p_position);
    }

    public void SetOkWorkDistance(float p_distance)
    {
        m_okWorkDistance = p_distance;
        agent.stoppingDistance = m_okWorkDistance;
    }

    public AgentsManager.AgentStates GetAgentState()
    {
        return m_state;
    }
}
