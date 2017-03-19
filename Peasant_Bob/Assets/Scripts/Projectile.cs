using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [System.Serializable]
    public class ProjectileInfo
    {
        public float damage;
        /// <summary>
        /// The time = distance / >>value<<
        /// </summary>
        public float timeDistanceRelation;
        public GameObject projectile;
    }

    public float damage = 0;

    bool enabledColliders = false;
    float enabletimer = 0;
    Rigidbody rig;

    GameObject parentDead = null;
    bool dead = false;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rig)
        {
            if (rig.velocity.magnitude > 0.01f)
            {
                transform.LookAt(transform.position + rig.velocity);
            }
        }

        if (enabledColliders == false)
        {
            enabletimer -= Time.deltaTime;
            if (enabletimer <= 0)
            {
                enabledColliders = true;

                foreach (Transform item in transform)
                {
                    Collider collider = item.GetComponent<Collider>();
                    if (collider)
                    {
                        collider.enabled = true;
                    }
                }
            }
        }
	}

    public void EnableColliderAfter(float timeSec)
    {
        enabletimer = timeSec;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Deal damage if not ally
        Team myTeam = GetComponent<Team>();
        bool hitMoving = false;

        Team otherTeam = collision.collider.GetComponent<Team>();
        if (otherTeam != null)
        {
            if (myTeam.team != otherTeam.team)
            {
                collision.collider.GetComponent<Health>().TakeDamage(damage);
            }
            hitMoving = true;
        }

        Destroy(gameObject, GlobalInformation.projectileDeathDelay);

        if (rig)
        {
            Destroy(rig);
        }
        foreach (Transform item in transform)
        {
            Collider collider = item.GetComponent<Collider>();
            if (collider)
            {
                Destroy(collider);
            }
        }

        if(hitMoving)
        {
            transform.parent = collision.collider.transform;
        }
    }
}
