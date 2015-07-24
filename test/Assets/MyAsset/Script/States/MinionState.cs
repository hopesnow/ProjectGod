using UnityEngine;
using System.Collections;

public class MinionState : ObjectState {

    protected int resist, move_speed;

    protected override void Awake()
    {
        base.Awake();
        resist = 0;
        move_speed = 80;

        attack = 10;

        range = 2.5f;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    void DamageMagic(int d)
    {
        int damage = (d - resist);
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

    protected override void DeadEvent()
    {
        GetComponent<MinionAI>().SendMessage("DeadAndGiveResource");

    }

    //goldをLHとった人に上げる処理を追加する

}
