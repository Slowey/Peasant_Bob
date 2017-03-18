using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour {
    protected Team m_team;
    protected AgentsManager myAgentManager;
    protected List<AgentsManager> enemyAgentManager;

	// Use this for initialization
	void Start () {
        m_team = GetComponent<Team>();

        GameObject[] agentMans = GameObject.FindGameObjectsWithTag("AgentManager");

        for (int i = 0; i < agentMans.Length; i++)
        {
            Team agentTeam = agentMans[i].GetComponent<Team>();
            if (agentTeam.team == m_team.team)
            {
                myAgentManager = agentMans[i].GetComponent<AgentsManager>();
            }
            else
            {
                enemyAgentManager.Add(agentMans[i].GetComponent<AgentsManager>());
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract void FightingActions(AgentsManager.AgentStates type);
}
