using UnityEngine;
using System.Collections;

public class NinjaState : HeroState {

    protected override void Awake()
    {
        base.Awake();

        health = 200;
        max_health = health;

        attack = 23;
        protect = 6;

        mana = 150;
        max_mana = mana;

        magic = 0;
        resist = 6;

        move_speed = 115;

        skill1_attack = 50;
        skill2_attack = 30;
        skill3_attack = 80;

        range = 2.0f;
        skill1_range = 5.0f;
        skill2_range = 2.5f;
        skill3_range = 3.0f;
        attack_speed = 105;

    }


	// Use this for initialization
	void Start () {

        
	
	}
	
	// Update is called once per frame
	protected override void Update () {

        base.Update();
	
	}





}
