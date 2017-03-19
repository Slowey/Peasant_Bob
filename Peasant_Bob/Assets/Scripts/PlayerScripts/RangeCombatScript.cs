using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCombatScript : MonoBehaviour {

    public static RangeCombatScript m_instance;
    public Projectile.ProjectileInfo projInfo;
    public float m_timeDistanceChanger = 10f;
    // Use this for initialization
    public float m_shootCooldown = 1f;

    private float m_timeSinceLastShot = 0f;
    void Awake()
    {
        m_instance = this;
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_timeSinceLastShot > 0f)
        {
            m_timeSinceLastShot -= Time.deltaTime;
        }
	}

    public void ShootArrow()
    {
        if (m_timeSinceLastShot>0f)
        {
            return;
        }
        Vector3 p_endpoint = TP_AimTrajectoryScript.m_instance.m_endpoint;
        //m_startpoint = Vector3.zero;
        Vector3 Startpoint = transform.position;



        Vector3 velocity;
        Vector3 acceleration = Physics.gravity;

        //float airTime = projInfo.projectileAirTime;
        float airTime = (p_endpoint - Startpoint).magnitude / m_timeDistanceChanger;

        velocity = (p_endpoint - Startpoint - (acceleration * airTime * airTime / 2.0f)) / airTime;


        GameObject proj = Instantiate(projInfo.projectile, transform.position + velocity.normalized, Quaternion.identity);
        //proj.transform.LookAt(proj.transform.position + velocity);
        Rigidbody rigProj = proj.GetComponent<Rigidbody>();
        rigProj.AddForce(velocity, ForceMode.VelocityChange);
        proj.GetComponent<Projectile>().damage = projInfo.damage;
        proj.GetComponent<Team>().team = Team.Teams.Blue;  //1234GlobalInformation.team
        proj.GetComponent<Projectile>().EnableColliderAfter(1.0f);
        m_timeSinceLastShot = m_shootCooldown;

    }
    public List<Vector3> GetTrajectory(Vector3 p_endpoint, int p_numberOfPoints)
    {
        Vector3 Startpoint = transform.position;


        Vector3 velocity;
        Vector3 acceleration = Physics.gravity;

        //float airTime = projInfo.projectileAirTime;
        float airTime = (p_endpoint - Startpoint).magnitude / m_timeDistanceChanger;
        
        velocity = (p_endpoint - Startpoint  - (acceleration * airTime * airTime / 2.0f)) / airTime;


        List<Vector3> t_tempList = new List<Vector3>();
        float t = 0;
        for (int i = 0; i < p_numberOfPoints; i++)
        {
            Vector3 pos = Startpoint + velocity * t + (acceleration * t * t /2f);
            t_tempList.Add(pos);
            t += airTime / (p_numberOfPoints-1);
        }

        return t_tempList;
    }
}
