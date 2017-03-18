using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehavior : UnitBase {

    public float aggroRange = 10.0f;
    float shootRange;


	// Use this for initialization
	void Start () {
        shootRange = GetComponent<UnitInformation>().range;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void FightingActions(AgentsManager.AgentStates p_state)
    {
        if (p_state == AgentsManager.AgentStates.Walking)
        {
            CheckInAggro();
        }


    }

    public void CheckInAggro()
    {
        Vector3 mypos = transform.position;
        Vector3 lastDir = new Vector3(0, 0, 0);
        GameObject potential = null;

        foreach (var item in enemyAgentManager)
        {
            foreach (var agentList in item.m_agents)
            {
                foreach (var agent in agentList.Value)
                {
                    Vector3 dir = agent.transform.position - mypos;
                    if (dir.magnitude < aggroRange)
                    {
                        if (potential != null && dir.magnitude < lastDir.magnitude)
                        {
                            lastDir = dir;
                            potential = agent;
                        }
                        else if(potential == null)
                        {
                            lastDir = dir;
                            potential = agent;
                        }
                    }
                }
            }
        }

        // add buildings

        // check if in shoot range

        if (potential != null)
        {
            if (lastDir.magnitude < shootRange)
            {
                // Change state to killmode
                GetComponent<NavAgent>().m_state = AgentsManager.AgentStates.Fighting;
            }
            else
            {
                // Move to closest
                GetComponent<NavAgent>().SetDestination(transform.position + lastDir, AgentsManager.AgentStates.Fighting);

            }
        }
    }

}
