﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInformation : MonoBehaviour {

    public int resourceCost = 0;
    public float spawnTime = 0;
    public int requireBeds = 0;
    public float maxHealth = 0;
    public float range = 0;
    public float attackSpeed = 0;
    public float attackAnimation = 0;
    public float meleeDamage = 0;

    public List<Projectile.ProjectileInfo> projectiles = new List<Projectile.ProjectileInfo>();



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
