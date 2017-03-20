using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

    float cooldown = 0.0f;
    public Vector3 verticalOffset;
    public float offset = 0.0f;
    public float projectileActivationTime = 0.0f;

    Team m_team;
    AgentsManager myAgentManager;
    List<AgentsManager> enemyAgentManager = new List<AgentsManager>();

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
        cooldown -= Time.deltaTime;

        if (cooldown <= 0)
        {
            Vector3 mypos = transform.position;
            Vector3 lastDir = new Vector3(0, 0, 0);
            GameObject target = null;
            float range = GetComponent<StructureInformation>().range;

            foreach (var item in enemyAgentManager)
            {
                foreach (var agentList in item.m_agents)
                {
                    foreach (var agent in agentList.Value)
                    {
                        Vector3 dir = agent.transform.position - mypos;
                        if (dir.magnitude < range)
                        {
                            if (target != null && dir.magnitude < lastDir.magnitude)
                            {
                                lastDir = dir;
                                target = agent;
                            }
                            else if (target == null)
                            {
                                lastDir = dir;
                                target = agent;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < (int)Team.Teams.NumberOfTeams; i++)
            {
                if (m_team.team != (Team.Teams)i)
                {
                    var buildings = BuildingTracker.buildingTracker.GetAllBuildingsOnTeam((Team.Teams)i);
                    if (buildings != null)
                    {
                        foreach (var building in buildings)
                        {
                            Vector3 dir = building.transform.position - mypos;
                            if (dir.magnitude < range)
                            {
                                if (target != null && dir.magnitude < lastDir.magnitude)
                                {
                                    lastDir = dir;
                                    target = building;
                                }
                                else if (target == null)
                                {
                                    lastDir = dir;
                                    target = building;
                                }
                            }
                        }
                    }
                }
            }

            if (m_team.team != GameObject.Find("Player").GetComponent<Team>().team)
            {
                Vector3 dirPlay = GameObject.Find("Player").transform.position - mypos;
                if (dirPlay.magnitude < range)
                {
                    if (target != null && dirPlay.magnitude < lastDir.magnitude)
                    {
                        lastDir = dirPlay;
                        target = GameObject.Find("Player");
                    }
                    else if (target == null)
                    {
                        lastDir = dirPlay;
                        target = GameObject.Find("Player");
                    }
                }
            }

            if (target == null)
            {
                return;
            }

            Rigidbody rig = target.GetComponent<Rigidbody>();
            VelocityTracker velTrack = target.GetComponent<VelocityTracker>();

            Vector3 velocity;
            Vector3 acceleration = Physics.gravity;
            Vector3 position = transform.position;
            Vector3 positionObj = target.transform.position + new Vector3(0, 1, 0); // TODO get form collsion box halfway
            Vector3 velocityObj = Vector3.zero;

            if (rig != null)
            {
                velocityObj = rig.velocity;
            }
            if (velTrack != null)
            {
                velocityObj = velTrack.velocity;
            }

            Projectile.ProjectileInfo projInfo = GetComponent<StructureInformation>().projectiles[0];
            //float airTime = projInfo.projectileAirTime;
            float airTime = (target.transform.position - transform.position).magnitude / projInfo.timeDistanceRelation;

            velocity = (positionObj + velocityObj * airTime - position - verticalOffset - (acceleration * airTime * airTime / 2.0f)) / airTime;


            GameObject proj = Instantiate(projInfo.projectile, transform.position + verticalOffset + velocity.normalized * offset, Quaternion.identity);
            //proj.transform.LookAt(proj.transform.position + velocity);
            Rigidbody rigProj = proj.GetComponent<Rigidbody>();
            rigProj.AddForce(velocity, ForceMode.VelocityChange);
            proj.GetComponent<Projectile>().damage = projInfo.damage;
            proj.GetComponent<Team>().team = m_team.team;
            proj.GetComponent<Projectile>().EnableColliderAfter(projectileActivationTime);

            cooldown = GetComponent<StructureInformation>().attackSpeed;
        }
		
	}
}
