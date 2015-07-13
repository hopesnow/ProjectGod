using UnityEngine;
using System.Collections;

public class SamuraiSkill : CharacterSkill {

	// Use this for initialization
	protected override void Start () {
        base.Start();

	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

    public override void Attack(GameObject t)
    {
        
        base.Attack(t);
        switch (nAttack)
        {
            case NEXT_ATTACK.normal:
                anim.SetTrigger("attack");


                break;

        }

    }

    protected override void Skill1()
    {
        base.Skill1();

    }

    protected override void Skill2()
    {
        base.Skill2();

    }

    protected override void Skill3()
    {
        base.Skill3();

    }



}
