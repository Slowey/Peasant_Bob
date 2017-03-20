using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Health : MonoBehaviour {

    float maxHealth = 0;
    public float curHealth = 0;
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
        else
        {
            maxHealth = curHealth;
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
        if (curHealth <= 0.0f)
        {
            Animation animComp = GetComponentInChildren<Animation>();
            if(animComp)
            {
                var clip = animComp.GetClip("Peasant_Death");
                if (clip)
                {
                    foreach (Component item in transform.GetComponentsInChildren<Component>())
                    {
                        if (!(item is Animation) && !(item is MeshFilter) && !(item is Renderer) && !(item is Transform))
                        {
                            Destroy(item);
                        }
                    }

                    animComp.Play("Peasant_Death");
                    Destroy(gameObject, 20);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
   
    // Update is called once per frame
    void Update () {
		
	}
}
