using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FetchPeasantCount : MonoBehaviour {
    AgentsManager teamManager;
	// Use this for initialization
	void Start () {
        GameObject[] agentMans = GameObject.FindGameObjectsWithTag("AgentManager");
        Team myTeam = GetComponent<Team>();

        for (int i = 0; i < agentMans.Length; i++)
        {
            Team agentTeam = agentMans[i].GetComponent<Team>();
            if (agentTeam.team == myTeam.team)
            {
                teamManager = agentMans[i].GetComponent<AgentsManager>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<Text>().text = "" + teamManager.GetUnitsAvailableForRecruiting();
    }
}
