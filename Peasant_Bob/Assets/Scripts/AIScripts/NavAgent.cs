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

    public Transform nextWalkpoint = null;

    UnitBase m_unitBase;

    GameObject agentManager;
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        GameObject[] agentMans = GameObject.FindGameObjectsWithTag("AgentManager");
        Team myTeam = GetComponent<Team>();

        for (int i = 0; i < agentMans.Length; i++)
        {
            Team agentTeam = agentMans[i].GetComponent<Team>();
            if (agentTeam.team == myTeam.team)
            {
                agentManager = agentMans[i];
            }
        }
        agentManager.GetComponent<AgentsManager>().AddNewAgent(gameObject, m_type);

        m_unitBase = GetComponent<UnitBase>();
    }

    void Update()
    {
        AgentsManager.AgentStates m_previousState = m_state;
        if (Vector3.Distance(agent.destination, transform.position) <= m_okWorkDistance)
        {
            if (nextWalkpoint && nextWalkpoint.childCount != 0)
            {
                int nextPoint = Random.Range(0, nextWalkpoint.childCount - 1);
                nextWalkpoint = nextWalkpoint.GetChild(nextPoint);
                SetDestination(nextWalkpoint.transform.position, m_wantedState);
            }
            else
            {
                m_state = m_wantedState;
            }
        }
        switch (m_wantedState)
        {
            case AgentsManager.AgentStates.Idle:
                if(m_type == UnitType.Archer)
                {
                    m_state = AgentsManager.AgentStates.Walking; // For now
                    m_wantedState = AgentsManager.AgentStates.Fighting;
                }
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

                m_unitBase.FightingActions(m_state);
                break;
            case AgentsManager.AgentStates.Attacking:
                m_unitBase.AttackingActions(m_state);
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
        if (agent == null)
        {
            agent = gameObject.GetComponent<NavMeshAgent>(); 
        }
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
                float dist = GetComponent<ResourceGatheringLogic>().maxGatherDistance;
                Vector3 newPosition = Vector3.zero;
                GameObject placeToGo = GameObject.FindGameObjectWithTag("ResourceManager").GetComponent<ResourceManager>().FindOneObjectOfType(gameObject.transform.position, dist, ResourceType.Wood);
                AgentsManager.AgentStates state = AgentsManager.AgentStates.GatheringResource;
                if (placeToGo == null)
                {
                    placeToGo = GameObject.FindGameObjectWithTag("TownHall");
                    newPosition = placeToGo.GetComponent<TownhallAgentSystem>().FindClosestPositionForAgent(gameObject);
                    state = AgentsManager.AgentStates.LeaveResources;
                }
                else
                {
                    newPosition = placeToGo.transform.position;
                }
                SetDestination(newPosition, state);
            }
        }
    }

    private void ChangedState()
    {
        string clipToChangeTo = "";
        switch (m_state)
        {
            case AgentsManager.AgentStates.Idle:
            case AgentsManager.AgentStates.Training:
            case AgentsManager.AgentStates.Fighting:
            case AgentsManager.AgentStates.Guarding:
                clipToChangeTo = "Peasant_Train";
                break;
            case AgentsManager.AgentStates.GatheringResource:
            case AgentsManager.AgentStates.Construction:
            case AgentsManager.AgentStates.LeaveResources:
                clipToChangeTo = "Peasant_Harvest";
                break;
            case AgentsManager.AgentStates.Walking:
                clipToChangeTo = "Peasant_Run";
                break;
            case AgentsManager.AgentStates.ENDITEM:
                break;
            case AgentsManager.AgentStates.Attacking:
                clipToChangeTo = "Peasant_Attack";
                break;
            default:
                break;
        }
        Animation animationComponent =  GetComponentInChildren<Animation>();

        if (animationComponent != null)
        {
            animationComponent.CrossFade(clipToChangeTo);
        }
    }

    void OnDestroy()
    {
        if (m_type == UnitType.Archer)
        {
            agentManager.GetComponent<AgentsManager>().RemoveUnitFromAgentSystem(gameObject, AgentsManager.AgentStates.Fighting);
        }
        else
        {
            agentManager.GetComponent<AgentsManager>().RemoveUnitFromAgentSystem(gameObject, m_wantedState);
        }
    }
}
