﻿using UnityEngine;
using System.Collections;

public class ObjectState : MonoBehaviour {

    protected int health, max_health, attack, protect, attack_speed;
    protected float range, width;

    protected virtual void Awake()
    {
        health = 100;
        max_health = 100;
        attack = 15;
        protect = 5;
        attack_speed = 100;
        range = 1.00f;
        width = 0.0f;
        //melee 1.5くら？ range 5.00くらい
        
    }

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public float RANGE { get { return range; } }
    public float WIDTH { get { return width; } }
    public float ATTACK { get { return attack; } }

    public float HEALTH_RATE
    {
        get { return (float)health / (float)max_health; }
    }

    void SetMaxHealth(int h)
    {
        max_health = h;
    }

    protected void DamageAttack(int d)
    {
        int damage = (d - protect);
        if (health > damage)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            //dead

        }

    }

    public int HEALTH { get { return health; } set { health = value; } }


}