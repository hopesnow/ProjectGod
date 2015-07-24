using UnityEngine;
using System.Collections;

//champion state
public class HeroState : ObjectState
{

    protected int mana, max_mana, magic, resist, move_speed, exp, gold;
    protected float skill1_range, skill2_range, skill3_range;
    protected int skill1_attack, skill2_attack, skill3_attack;

    protected override void Awake()
    {

        base.Awake();

        health = 150;
        max_health = 150;

        attack = 20;
        protect = 8;

        mana = 100;
        max_mana = 100;
        magic = 0;
        resist = 5;
        move_speed = 100;
        exp = 0;
        gold = 0;

        range = 6.0f;
        skill1_range = 3.0f;
        skill2_range = 3.0f;
        skill3_range = 3.0f;
        attack_speed = 100;
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

    public float RANGE_SKILL1 { get { return skill1_range; } }
    public float RANGE_SKILL2 { get { return skill2_range; } }
    public float RANGE_SKILL3 { get { return skill3_range; } }

    public int ATTACK_SKILL1 { get { return skill1_attack; } }
    public int ATTACK_SKILL2 { get { return skill2_attack; } }
    public int ATTACK_SKILL3 { get { return skill3_attack; } }

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
