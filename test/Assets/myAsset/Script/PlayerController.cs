using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum CharacterAnimState
{
    idle, 
    walk, 
    attack,
    skill1,
    skill2,
    skill3,
}

public enum CHARA_ACT
{
    idle, 
    autoAttack,//keep attacking
    targetting,
    freeWalk,

}

public class PlayerController : MonoBehaviour {

	NavMeshAgent agent;
	public Transform target;
	Animator anim;

	public GameObject healthImage;
    public GameObject gaugeImage;

    public bool controllable = false;
    public bool targetting = false;
    public Transform targettingObj = null;

    public CharacterAnimState animState;

    public CHARA_ACT act;
    float attackDelay;

    bool dying;
    public bool DYING { get { return dying; } }

    CharacterSkill cSkill;
    bool skillPlaying;
    

	// Use this for initialization
	void Awake () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = true;
		agent.updatePosition = true;

        GameObject[] gigo = GameObject.FindGameObjectsWithTag("gaugeBack");
        for (int i = 0; i < gigo.Length; i++)
        {
            if (gigo[i].GetComponent<RectTransform>().localScale == Vector3.zero)
            {
                gigo[i].GetComponent<RectTransform>().localScale = Vector3.one;
                gaugeImage = gigo[i];
                gigo[i].SendMessage("SetTarget", transform);
                break;
            }
        }

        GameObject[] higo = GameObject.FindGameObjectsWithTag("healthGauge");
        for (int i = 0; i < higo.Length; i++)
        {
            if (higo[i].GetComponent<RectTransform>().localScale == Vector3.zero)
            {
                higo[i].GetComponent<RectTransform>().localScale = Vector3.one;
                healthImage = higo[i];
                higo[i].SendMessage("SetTarget", transform);
                break;
            }
        }


        anim = GetComponentInChildren<Animator>();
        animState = CharacterAnimState.idle;

        act = CHARA_ACT.idle;
        attackDelay = 0;

        targetting = false;
        targettingObj = null;

        dying = false;

        cSkill = gameObject.GetComponent<CharacterSkill>();
        skillPlaying = false;

	}
	
	// Update is called once per frame
	void Update () {


        if (controllable)
        {


            attackDelay -= Time.deltaTime;

            switch (act)
            {
                case CHARA_ACT.idle:

                    anim.SetFloat("speed", 0.0f);
                    animState = CharacterAnimState.idle;

                    break;
                case CHARA_ACT.freeWalk:

                    //目的地にたどり着いた場合
                    if ((transform.position - target.position).magnitude < 0.1f)
                    {
                        act = CHARA_ACT.idle;
                        StopMove();
                        break;
                    }

                    anim.SetFloat("speed", agent.velocity.magnitude);
                    animState = CharacterAnimState.walk;

                    break;
                case CHARA_ACT.targetting:

                    //targetが死んだら
                    if (!targettingObj)
                    {

                        targetting = false;
                        targettingObj = null;
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        {
                            if (go.GetComponent<ObjectState>().team == GetComponent<ObjectState>().team) continue;
                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(go.transform.position)) <= GetComponent<ObjectState>().RANGE + go.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj(go);
                                break;
                            }

                        }

                        if (targettingObj == null)
                        {
                            act = CHARA_ACT.idle;
                            StopMove();
                        }

                    }
                    else
                    {
                        MoveToTarget();
                    }

                    //攻撃可能範囲に入った場合
                    if(Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(targettingObj.position)) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH){
                        StopMove();
                        act = CHARA_ACT.autoAttack;
                        break;
                    }

                    anim.SetFloat("speed", agent.velocity.magnitude);
                    animState = CharacterAnimState.walk;

                    //targetが死んだら
                    if (!targettingObj)
                    {

                        targetting = false;
                        targettingObj = null;
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        {
                            if (go.GetComponent<ObjectState>().team == GetComponent<ObjectState>().team) continue;
                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(go.transform.position)) <= GetComponent<ObjectState>().RANGE + go.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj(go);
                                break;
                            }

                        }

                        if (targettingObj == null)
                        {
                            act = CHARA_ACT.idle;
                            StopMove();
                        }

                    }
                    else
                    {
                        MoveToTarget();
                    }

                    break;
                case CHARA_ACT.autoAttack:

                    //targetが死んだら
                    if (!targettingObj)
                    {

                        targetting = false;
                        targettingObj = null;
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        {
                            if (go.GetComponent<ObjectState>().team == GetComponent<ObjectState>().team) continue;
                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(go.transform.position)) <= GetComponent<ObjectState>().RANGE + go.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj(go);
                                break;
                            }

                        }

                        if (targettingObj == null)
                        {
                            act = CHARA_ACT.idle;
                            StopMove();
                        }

                    }

                    if (skillPlaying) break;

                    //スキル発動範囲かどうか
                    switch (cSkill.nAttack)
                    {
                        case NEXT_ATTACK.normal:

                            //攻撃可能範囲から外れた場合
                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(targettingObj.position)) > gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj();
                                act = CHARA_ACT.targetting;
                                break;
                            }

                            //何事もなく攻撃可能タイミングになったか
                            if (attackDelay <= 0)
                            {
                                transform.LookAt(targettingObj);
                                //anim.SetTrigger("attack");
                                animState = CharacterAnimState.attack;
                                cSkill.Attack(targettingObj.gameObject);
                                attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
                            }
                            else
                            {
                                animState = CharacterAnimState.idle;
                            }

                            break;
                        case NEXT_ATTACK.skill1:



                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(targettingObj.position)) > gameObject.GetComponent<HeroState>().RANGE_SKILL1 + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj();
                                act = CHARA_ACT.targetting;
                                break;
                            }

                            skillPlaying = true;
                            transform.LookAt(targettingObj);
                            animState = CharacterAnimState.skill1;
                            cSkill.Attack(targettingObj.gameObject);
                            attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;


                            break;
                        case NEXT_ATTACK.skill3:


                            if (Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(targettingObj.position)) > gameObject.GetComponent<HeroState>().RANGE_SKILL3 + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
                            {
                                SetTargetFromObj();
                                act = CHARA_ACT.targetting;
                                break;
                            }

                            skillPlaying = true;
                            transform.LookAt(targettingObj);
                            animState = CharacterAnimState.skill3;
                            cSkill.Attack(targettingObj.gameObject);
                            attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;


                            break;
                    }
                    

                    

                    break;
            }





        }
        else //自分が操作しない場合
        {

            switch (animState)
            {
                case CharacterAnimState.idle:
                    anim.SetFloat("speed", 0);
                    break;
                case CharacterAnimState.walk:
                    anim.SetFloat("speed", 3.5f);
                    break;
                case CharacterAnimState.attack:
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Base.NormalAttack"))
                    {
                        anim.SetTrigger("attack");
                    }
                    break;


            }

        }

        

	}

    void AttackTargetObj()
    {
        if (targetting && targettingObj != null)
        {
            targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);

        }

    }

    public void SetTargetFromObj(GameObject go)
    {
        targetting = true;
        targettingObj = go.transform;
        SetTargetFromObj();
    }


    public void SetTargetFromObj()
    {
        GameObject target = new GameObject("target");
        target.transform.position = targettingObj.position + Vector3.Normalize(transform.position - targettingObj.position) * 0.5f;
        MoveTo(target.transform);

    }

	void MoveTo(Transform t){

        if (target != null)
        {
            Destroy(target.gameObject);
        }
        target = t;

        if (targetting)
        {
            act = CHARA_ACT.targetting;
        }
        else
        {
            act = CHARA_ACT.freeWalk;
        }
	
		
        agent.Resume();
        agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));
	
	}

    void MoveToTarget()
    {
        agent.SetDestination(new Vector3(targettingObj.position.x, transform.position.y, targettingObj.position.z));
    }

    Vector3 GetVecXZ(Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }

    void StopMove()
    {
        agent.Stop();
        anim.SetFloat("speed", 0.0f);
        if (target != null)
        {
            Destroy(target.gameObject);
            target = null;
        }

    }

    void SetCanvasTeam()
    {
        healthImage.SendMessage("SetTeamColor", (int)GetComponent<ObjectState>().team);

    }

    //死んだ直後
    void RespawnPrepare()
    {

        dying = true;
        act = CHARA_ACT.idle;
        StopMove();
        targetting = false;
        targettingObj = null;
        animState = CharacterAnimState.idle;

        //respawn位置
        switch (GetComponent<ObjectState>().team)
        {
            case TEAM.BLUE:
                agent.Warp(GameObject.Find("blue_base").transform.position);

                break;
            case TEAM.RED:
                agent.Warp(GameObject.Find("red_base").transform.position);

                break;
        }

        foreach (SkinnedMeshRenderer ren in this.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            ren.enabled = false;
        }
        healthImage.SetActive(false);
        gaugeImage.SetActive(false);

        Invoke("RespawnInit", 10.0f);

    }

    //生き返る直前
    void RespawnInit()
    {

        dying = false;

        //health関係
        GetComponent<HeroState>().Respawn();

        //再表示
        foreach (SkinnedMeshRenderer ren in this.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            ren.enabled = true;
        }
        healthImage.SetActive(true);
        gaugeImage.SetActive(true);

        GameObject.Find("Main Camera").SendMessage("CheckRespawnPlayer", this.gameObject);

    }


    //スキルボタン押下
    public void OnSkillButton1()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill1);
    }

    public void OnSkillButton2()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill2);
    }

    public void OnSkillButton3()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill3);
    }


    public void EndSkill()
    {
        skillPlaying = false;
        animState = CharacterAnimState.idle;
    }


    //スキル発動
    public void OnSkill1Action()
    {
        cSkill.Skill1();
    }

    public void OnSkill2Action()
    {
        cSkill.Skill2();

    }

    public void OnSkill3Action()
    {
        cSkill.Skill3();

    }

}
