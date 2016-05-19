using UnityEngine;
using System.Collections;

public class TowerState : ObjectState {



    protected override void Awake()
    {
        base.Awake();

        health = 200;
        max_health = 200;
        attack = 50;
        protect = 10;
        attack_speed = 40;
        range = 11.0f;
        width = 1.5f;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

	}

    protected override void DeadEvent()
    {

        GetComponent<TowerAI>().SendMessage("DeadAndGiveResource");

    }




}
