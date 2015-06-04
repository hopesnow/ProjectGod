using UnityEngine;
using System.Collections;

//champion state
public class HeroState : ObjectState
{

    protected int mana, max_mana, magic, resist, move_speed, exp, gold;

    protected override void Awake()
    {

        base.Awake();
        mana = 100;
        max_mana = 100;
        magic = 0;
        resist = 5;
        move_speed = 100;
        exp = 0;
        gold = 0;

        range = 3.0f;
        //melee 1.0f? range 3.0f?

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
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

    void SetMaxMana(int m)
    {
        max_mana = m;
    }

    protected override void DeadEvent()
    {

        GetComponent<PlayerController>().SendMessage("RespawnPrepare");
        GameObject.Find("Main Camera").SendMessage("CheckDeadPlayer", this.gameObject);

    }

    public void Respawn()
    {
        health = max_health;
        mana = max_mana;

    }

}
