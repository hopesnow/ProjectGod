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
            case NEXT_ATTACK.skill1:
                anim.SetTrigger("skill1");

                break;
            case NEXT_ATTACK.skill2:
                anim.SetTrigger("skill2");

                break;
            case NEXT_ATTACK.skill3:
                anim.SetTrigger("skill3");

                break;

        }
        nAttack = NEXT_ATTACK.normal;
        GameObject.Find("Main Camera").GetComponent<CameraControl>().SkillColorSet(nAttack);

    }

    public override void Skill1()
    {
        base.Skill1();
        //蹴り上げ
        target.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<SamuraiState>().ATTACK_SKILL1);

    }

    public override void Skill2()
    {
        base.Skill2();
        //二段斬り
        target.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<SamuraiState>().ATTACK_SKILL2);

    }

    public override void Skill3()
    {
        base.Skill3();
        //掌底突き
        target.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<SamuraiState>().ATTACK_SKILL3);

    }


    public override void ButtonTrigger(NEXT_ATTACK na)
    {

        switch (na)
        {
            case NEXT_ATTACK.normal:
                nAttack = NEXT_ATTACK.normal;

                break;
            case NEXT_ATTACK.skill1:
                if (nAttack == na)
                    nAttack = NEXT_ATTACK.normal;
                else
                    nAttack = NEXT_ATTACK.skill1;

                break;
            case NEXT_ATTACK.skill2:
                if (nAttack == na)
                    nAttack = NEXT_ATTACK.normal;
                else
                    nAttack = NEXT_ATTACK.skill2;

                break;
            case NEXT_ATTACK.skill3:
                if (nAttack == na)
                    nAttack = NEXT_ATTACK.normal;
                else
                    nAttack = NEXT_ATTACK.skill3;

                break;
            default:
                nAttack = NEXT_ATTACK.normal;

                break;

        }

        GameObject.Find("Main Camera").GetComponent<CameraControl>().SkillColorSet(nAttack);


    }


}
