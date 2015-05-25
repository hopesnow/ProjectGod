using UnityEngine;
using System.Collections;

public enum NEXT_ATTACK
{
    normal,
    skill1,
    skill2,
    skill3,
}

public class CharacterSkill : MonoBehaviour {

    protected NEXT_ATTACK nAttack;


	// Use this for initialization
	protected virtual void Start () {
        nAttack = NEXT_ATTACK.normal;

	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

    protected virtual void Attack()
    {

    }

    protected virtual void Skill1()
    {

    }

    protected virtual void Skill2()
    {

    }

    protected virtual void Skill3()
    {

    }

}
