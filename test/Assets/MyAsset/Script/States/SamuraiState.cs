using UnityEngine;
using System.Collections;

public class SamuraiState : HeroState {

    protected override void Awake()
    {
        base.Awake();

        health = 250;
        max_health = health;

        attack = 21;
        protect = 7;

        mana = 200;
        max_mana = mana;

        magic = 15;
        resist = 8;

        move_speed = 110;

        skill1_attack = 60;
        skill2_attack = 40;
        skill3_attack = 40;

        range = 3.0f;
        skill1_range = 2.5f;
        skill2_range = 3.0f;
        skill3_range = 8.0f;
        attack_speed = 115;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();


	}
}
