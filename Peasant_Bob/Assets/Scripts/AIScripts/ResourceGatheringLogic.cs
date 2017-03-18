using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatheringLogic : MonoBehaviour {
    public float m_maxResourceCarry;
    public float m_resourceGatherSpeed;
    private float m_currentResourceCarry;
    // Use this for initialization
    void Start() {

    }

    public bool AgentUpdate(AgentsManager.AgentStates p_currentState)
    {
        if (p_currentState == AgentsManager.AgentStates.GatheringResource)
        {
            if (m_currentResourceCarry >= m_maxResourceCarry)
            {
                m_currentResourceCarry = m_maxResourceCarry;
                return false;
            }
            m_currentResourceCarry += m_resourceGatherSpeed * Time.deltaTime;
        }
        else if (p_currentState == AgentsManager.AgentStates.LeaveResources)
        {
            m_currentResourceCarry = 0;
            return false;
        }
        return true;
    }
}
