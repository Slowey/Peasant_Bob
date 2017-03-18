using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgent : MonoBehaviour {
    public enum UnitType
    {
        Peasant,
        Archer,
    }
    private NavMeshAgent agent;
    public AgentsManager.AgentStates m_state;
    public AgentsManager.AgentStates m_wantedState;
    public float m_okWorkDistance;
    public UnitType m_type;
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        GameObject.FindGameObjectWithTag("AgentManager").GetComponent<AgentsManager>().AddNewAgent(gameObject, m_type);
    }

    void Update()
    {
        AgentsManager.AgentStates m_previousState = m_state;
        if (Vector3.Distance(agent.destination, transform.position) <= m_okWorkDistance)
        {
            m_state = m_wantedState;
        }
        switch (m_wantedState)
        {
            case AgentsManager.AgentStates.Idle:
                break;
            case AgentsManager.AgentStates.GatheringResource:
            case AgentsManager.AgentStates.LeaveResources:
                {
                    GatheringResourceUpdate();
                }
                break;
            case AgentsManager.AgentStates.Construction:
                break;
            case AgentsManager.AgentStates.Fighting:
                break;
            case AgentsManager.AgentStates.Walking:
                break;
            case AgentsManager.AgentStates.ENDITEM:
                break;
            default:
                break;
        }
        if (m_previousState != m_state)
        {
            ChangedState();
        }
    }

    public void SetDestination(Vector3 p_position, AgentsManager.AgentStates p_wantedState)
    {
        m_state = AgentsManager.AgentStates.Walking;
        m_wantedState = p_wantedState;
        agent.SetDestination(p_position);
        ChangedState();
    }

    public AgentsManager.AgentStates GetAgentState()
    {
        return m_state;
    }

    private void GatheringResourceUpdate()
    {
        bool stayInState = GetComponent<ResourceGatheringLogic>().AgentUpdate(m_state);
        if (!stayInState)
        {
            if (m_state == AgentsManager.AgentStates.GatheringResource)
            {
                SetDestination(GameObject.FindGameObjectWithTag("TownHall").GetComponent<TownhallAgentSystem>().FindClosestPositionForAgent(gameObject), AgentsManager.AgentStates.LeaveResources);
            }
            else if (m_state == AgentsManager.AgentStates.LeaveResources)
            {
                Vector3 newPosition = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().FindClosestResourceOfType(gameObject.transform.position,1000, ResourceType.Wood).transform.position;
                SetDestination(newPosition, AgentsManager.AgentStates.GatheringResource);
            }
        }
    }

    private void ChangedState()
    {
        string clipToChangeTo = "";
        switch (m_state)
        {
            case AgentsManager.AgentStates.Idle:
            case AgentsManager.AgentStates.GatheringResource:
            case AgentsManager.AgentStates.Construction:
            case AgentsManager.AgentStates.Training:
            case AgentsManager.AgentStates.Fighting:
            case AgentsManager.AgentStates.Guarding:
            case AgentsManager.AgentStates.LeaveResources:
                clipToChangeTo = "Peasant_Attack";
                break;
            case AgentsManager.AgentStates.Walking:
                clipToChangeTo = "Peasant_Run";
                break;
            case AgentsManager.AgentStates.ENDITEM:
                break;
            default:
                break;
        }
        Animation animationComponent =  GetComponent<Animation>();

        if (animationComponent != null)
        {
            animationComponent.CrossFade(clipToChangeTo);
        }
    }
}
