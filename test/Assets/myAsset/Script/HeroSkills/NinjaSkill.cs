using UnityEngine;
using System.Collections;

public class NinjaSkill : CharacterSkill {

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
        //ブリンクからの通常攻撃
        target.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<NinjaState>().ATTACK_SKILL1);

    }

    public override void Skill2()
    {
        base.Skill2();
        //自分の周りに範囲攻撃
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
        {
            if(Vector3.Distance(GetVecXZ(transform.position) , GetVecXZ(go.transform.position)) <= GetComponent<NinjaState>().RANGE_SKILL2){
                if (go.GetComponent<ObjectState>().team == transform.GetComponent<ObjectState>().team) continue;
                go.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<NinjaState>().ATTACK_SKILL2);
            }

        }


    }

    public override void Skill3()
    {
        base.Skill3();
        //目の前の○度の敵にノックバック攻撃
        target.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<NinjaState>().ATTACK_SKILL3);


    }

    public override void ButtonTrigger(NEXT_ATTACK na)
    {

        

        switch(na){
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
                anim.SetTrigger("skill2");
                nAttack = NEXT_ATTACK.normal;
                
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



    Vector3 GetVecXZ(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }


}
