using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : UnitBase {

    public float aggroRange = 10.0f;
    float shootRange;
    float cooldown = 0.0f;


	// Use this for initialization
	void Start () {
        base.Start();
        shootRange = GetComponent<UnitInformation>().range;
	}
	
	// Update is called once per frame
	void Update () {
        cooldown -= Time.deltaTime;
	}

    public override void FightingActions(AgentsManager.AgentStates p_state)
    {
        if (p_state == AgentsManager.AgentStates.Walking)
        {
            CheckInAggro(p_state);
        }
        else if (p_state == AgentsManager.AgentStates.Fighting)
        {
            // Check if still in fighting
            // else find new point to fight to (enemy main buildng?)

            CheckInAggro(p_state);
        }


    }

    void AttackUpdate(GameObject attackObj)
    {
        if (cooldown <= 0)
        {
            cooldown = GetComponent<UnitInformation>().attackSpeed;
            Projectile.ProjectileInfo projInfo = GetComponent<UnitInformation>().projectiles[0];

            Rigidbody rig = attackObj.GetComponent<Rigidbody>();
            VelocityTracker velTrack = attackObj.GetComponent<VelocityTracker>();

            Vector3 velocity;
            Vector3 acceleration = Physics.gravity;
            Vector3 position = transform.position;
            Vector3 positionObj = attackObj.transform.position + new Vector3(0, 1, 0); // TODO get form collsion box halfway
            Vector3 velocityObj = Vector3.zero;

            if (rig != null)
            {
                velocityObj = rig.velocity;
            }
            else if(velTrack != null)
            {
                velocityObj = velTrack.velocity;
            }

            //float airTime = projInfo.projectileAirTime;
            float airTime = (attackObj.transform.position - transform.position).magnitude / projInfo.timeDistanceRelation;
            float shootoffset = 0.0f;

            velocity = (positionObj + velocityObj * airTime - position - new Vector3(0, 1, 0) - (acceleration * airTime * airTime / 2.0f))/ airTime;

            
            

            GameObject proj = Instantiate(projInfo.projectile, transform.position + velocity.normalized* shootoffset,Quaternion.identity);
            //proj.transform.LookAt(proj.transform.position + velocity);
            Rigidbody rigProj = proj.GetComponent<Rigidbody>();
            rigProj.AddForce(velocity, ForceMode.VelocityChange);
            proj.GetComponent<Projectile>().damage = projInfo.damage;
            proj.GetComponent<Team>().team = m_team.team;
            proj.GetComponent<Projectile>().EnableColliderAfter(1.0f);
            

            //Set damage
        }

        transform.LookAt(attackObj.transform);
    }

    public void CheckInAggro(AgentsManager.AgentStates p_laststate)
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
                if (p_laststate == AgentsManager.AgentStates.Fighting)
                {
                    // Continue fighting stuff
                    AttackUpdate(potential);
                }
                else
                {
                    GetComponent<NavAgent>().m_state = AgentsManager.AgentStates.Fighting;
                    GetComponent<NavAgent>().SetDestination(transform.position, AgentsManager.AgentStates.Fighting);
                    Debug.Log("KILLMODE ENGAGE");
                }
            }
            else
            {
                // Move to closest
                GetComponent<NavAgent>().SetDestination(transform.position + lastDir, AgentsManager.AgentStates.Fighting);
                Debug.Log("I SEE ENEMY");
            }
        }
    }

}
