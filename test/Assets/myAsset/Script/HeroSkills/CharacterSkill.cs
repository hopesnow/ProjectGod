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

    /// <summary>
    /// 通常攻撃で発動するタイプのスキル
    /// </summary>
    public NEXT_ATTACK nAttack;

    /// <summary>
    /// 対象指定のスキルの対象
    /// </summary>
    protected GameObject target;

    protected Animator anim;



	// Use this for initialization
	protected virtual void Start () {
        nAttack = NEXT_ATTACK.normal;

        anim = gameObject.GetComponentInChildren<Animator>();

	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

    public virtual void Attack(GameObject t)
    {
        target = t;
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

    public virtual void ButtonTrigger(NEXT_ATTACK na)
    {

    }

}
