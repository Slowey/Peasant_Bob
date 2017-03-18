using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour {
    public enum AgentStates
    {
        Idle,
        GatheringResource,
        Construction,
        Fighting,
        Walking,
        ENDITEM
    }
    public float m_maxDistanceToGatherResources;
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

    public void AddNewAgent(GameObject p_agent)
    {
        // Every agent start as idle NOT!!! They start as resource gatherers
        m_agents[AgentStates.Idle].Add(p_agent);
        SendAgentToGatherResources(p_agent);
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
        // p_agent.GetComponent<NavAgent>().SetDestination(new Vector3(0, 0, 0), AgentStates.Idle);
        // Apparently we want all idle units to gather resources
        SendAgentToGatherResources(p_agent);
        
    }

    public GameObject AssignFreeAgentToTask(GameObject p_taskGameObject, AgentStates p_taskState)
    {
        GameObject closestAgent = GetClosestIdleAgentToObject(p_taskGameObject);
        int index = m_agents[AgentStates.GatheringResource].FindIndex(obj => obj.transform == closestAgent.transform); // TODO this should check for idle list through
        m_agents[AgentStates.GatheringResource].RemoveAt(index);
        m_agents[p_taskState].Add(closestAgent);
        closestAgent.GetComponent<NavAgent>().SetDestination(p_taskGameObject.transform.position, p_taskState);
        return closestAgent;
    }

    // HELPER FUNCTIONS
    private GameObject SendAgentToGatherResources(GameObject p_agent)
    {
        ResourceManager resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceManager>();
        GameObject closestResource = resourceManager.FindClosestResourceOfType(p_agent.transform.position, m_maxDistanceToGatherResources, ResourceType.Wood);
        if (closestResource == null)
        {
            return null;
        }
        int index = m_agents[AgentStates.Idle].FindIndex(obj => obj.transform == p_agent.transform);
        m_agents[AgentStates.Idle].RemoveAt(index);
        m_agents[AgentStates.GatheringResource].Add(p_agent);

        p_agent.GetComponent<NavAgent>().SetDestination(closestResource.transform.position, AgentStates.GatheringResource);

        return p_agent;
    }

    private GameObject GetClosestIdleAgentToObject(GameObject p_object)
    {
        float shortestDistance = float.MaxValue;
        GameObject closestAgent = null;
        foreach (var item in m_agents[AgentStates.GatheringResource]) // TODO Change this to idle if idle ever becomes a thing
        {
            float distance = Vector3.Distance(p_object.transform.position, item.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestAgent = item;
            }
        }
        foreach (var item in m_agents[AgentStates.Idle]) // TODO Change this to idle if idle ever becomes a thing
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
