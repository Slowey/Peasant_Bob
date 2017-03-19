using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    float maxHealth = 0;
    float curHealth = 0;
	// Use this for initialization
	void Start () {
        if(GetComponent<UnitInformation>() != null)
        {
            maxHealth = GetComponent<UnitInformation>().maxHealth;
        }
        else if(GetComponent<StructureInformation>() != null)
        {
            maxHealth = GetComponent<StructureInformation>().maxHealth;
        }
        curHealth = maxHealth;
    }
	
    public void SetMaxHealth(float val)
    {
        maxHealth = val;
    }

    public void SetHealth(float val)
    {
        curHealth = val;
    }

    public void TakeDamage(float val)
    {
        curHealth -= val;
        CheckDeath();
    }

    public void CheckDeath()
    {
        if (curHealth < 0.0f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
