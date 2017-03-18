using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour {
    public enum AgentStates
    {
        Idle,
        GatheringResource,
        Construction,
        Training,
        Fighting,
        Guarding,
        Walking,
        LeaveResources,
        ENDITEM
    }
    // This basically keeps track of what agent is having what goal
    Dictionary<AgentStates, List<GameObject>> m_agents = new Dictionary<AgentStates, List<GameObject>>();

    // This runs before start
    void Awake()
    {
        for (int i = 0; i < (int)AgentStates.ENDITEM; i++)
        {
            m_agents.Add((AgentStates)i, new List<GameObject>());
        }
    }

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddNewAgent(GameObject p_agent, NavAgent.UnitType p_unitType)
    {
        // Every agent start as idle NOT!!! They start as resource gatherers
        if (p_unitType == NavAgent.UnitType.Peasant)
        {
            m_agents[AgentStates.Idle].Add(p_agent);
            GameObject townHall = GameObject.FindGameObjectWithTag("TownHall");
            if (townHall != null)
            {
                townHall.GetComponent<TownhallAgentSystem>().SendNewAgentToGatherResources();
            }
        }
        else if (p_unitType == NavAgent.UnitType.Archer)
        {
            m_agents[AgentStates.Guarding].Add(p_agent);
        }
    }

    public GameObject AssignFreeAgentToFight()
    {
        // Dont know what to do here... is a bit special
        return null;
    }

    public void RemoveAgentFromTask(GameObject p_agent, AgentStates p_stateToRemoveFrom)
    {
        int index = m_agents[p_stateToRemoveFrom].FindIndex(obj => obj.transform == p_agent.transform);
        m_agents[p_stateToRemoveFrom].RemoveAt(index);
        m_agents[AgentStates.Idle].Add(p_agent);
        p_agent.GetComponent<NavAgent>().SetDestination(Vector3.zero, AgentStates.Idle);
        // p_agent.GetComponent<NavAgent>().SetDestination(new Vector3(0, 0, 0), AgentStates.Idle);
        // Apparently we want all idle units to gather resources
        GameObject townHall = GameObject.FindGameObjectWithTag("TownHall");
        if (townHall != null)
        {
            townHall.GetComponent<TownhallAgentSystem>().SendNewAgentToGatherResources();
        }

    }

    public GameObject AssignFreeAgentToTask(GameObject p_taskGameObject, AgentStates p_taskState)
    {
        AgentStates t_takenAgentPrevState;
        GameObject closestAgent = GetClosestAvailableAgentToObject(p_taskGameObject, out t_takenAgentPrevState);
        if (closestAgent != null)
        {
            int index = m_agents[t_takenAgentPrevState].FindIndex(obj => obj.transform == closestAgent.transform); // TODO this should check for idle list through
            m_agents[t_takenAgentPrevState].RemoveAt(index);
            m_agents[p_taskState].Add(closestAgent);
        }

        return closestAgent;
    }

    public bool UnitIsAvailableForWork()
    {
        return m_agents[AgentStates.Idle].Count > 0 || m_agents[AgentStates.GatheringResource].Count > 0;
    }

    // Call on destroy
    public bool RemoveUnitFromAgentSystem(GameObject p_agent, AgentStates p_inState)
    {
        int index = m_agents[p_inState].FindIndex(obj => obj.transform == p_agent.transform);
        if (index == -1)
        {
            return false;
        }
        m_agents[p_inState].RemoveAt(index);
        return true;
    }
    // HELPER FUNCTIONS
    private GameObject GetClosestAvailableAgentToObject(GameObject p_object, out AgentStates o_unitPreviousState)
    {
        o_unitPreviousState = AgentStates.ENDITEM;
        GameObject closestAgent = null;
        closestAgent = SearchThroughListForClosestAgent(p_object, AgentStates.Idle);
        if (closestAgent != null)
        {
            o_unitPreviousState = AgentStates.Idle;
            return closestAgent;
        }
        closestAgent = SearchThroughListForClosestAgent(p_object, AgentStates.GatheringResource);
        if (closestAgent != null)
        {
            // Tell the townhall to release the agent
            GameObject townHall = GameObject.FindGameObjectWithTag("TownHall");
            if (townHall != null)
            {
                townHall.GetComponent<TownhallAgentSystem>().ReleaseAgent(closestAgent);
            }
            o_unitPreviousState = AgentStates.GatheringResource;
            return closestAgent;
        }
        return closestAgent;
    }

    private GameObject SearchThroughListForClosestAgent(GameObject p_object, AgentStates p_list)
    {
        float shortestDistance = float.MaxValue;
        GameObject closestAgent = null;

        foreach (var item in m_agents[p_list]) // TODO Change this to idle if idle ever becomes a thing
        {
            float distance = Vector3.Distance(p_object.transform.position, item.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestAgent = item;
            }
        }

        return closestAgent;      
    }
}
