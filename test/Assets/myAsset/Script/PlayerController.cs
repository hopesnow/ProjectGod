using UnityEngine;
using System.Collections;

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
	Transform target;
	Animator anim;

	public GameObject healthImage;
    public GameObject gaugeImage;

    public bool controllable = false;
    public bool targetting = false;
    public Transform targettingObj = null;

    public CharacterAnimState animState;

    CHARA_ACT act;
    float attackDelay;

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

	}
	
	// Update is called once per frame
	void Update () {

        if (controllable)
        {

            attackDelay -= Time.deltaTime;

            switch (act)
            {
                case CHARA_ACT.idle:
                    //基本ないがtargetがいた場合なにか確認する為freeWalkへ
                    if (target)
                    {
                        act = CHARA_ACT.freeWalk;
                        break;
                    }

                    anim.SetFloat("speed", 0);
                    animState = CharacterAnimState.idle;

                    break;
                case CHARA_ACT.freeWalk:
                    //移動、攻撃をするかどうか
                    if (target == null)
                    {
                        act = CHARA_ACT.idle;
                        break;
                    }
                    //攻撃対象がいるかどうか
                    if (targetting)
                    {
                        act = CHARA_ACT.targetting;
                        break;
                    }
                    agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));

                    //目的地にたどり着いたか、あとモーション等
                    if ((transform.position - target.position).magnitude < 0.1f)
                    {
                        act = CHARA_ACT.idle;
                        Destroy(target.gameObject);
                        target = null;
                    }
                    else
                    {
                        float spd = agent.velocity.magnitude;
                        anim.SetFloat("speed", spd);
                        animState = CharacterAnimState.walk;
                    }

                    break;
                case CHARA_ACT.targetting:
                    if(Vector3.Distance(transform.position, target.position) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH){
                        //攻撃範囲に入ったのでそれ以上近づかないように
                        agent.SetDestination(transform.position);

                        if (animState == CharacterAnimState.idle || animState == CharacterAnimState.walk)
                        {
                            if (attackDelay <= 0)
                            {
                                transform.LookAt(targettingObj);
                                anim.SetTrigger("attack");
                                targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
                                attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
                                act = CHARA_ACT.autoAttack;
                                animState = CharacterAnimState.attack;
                            }
                        }

                    }

                    break;
                case CHARA_ACT.autoAttack:
                    //camera controlからのsendmessageで別ケースへ移動する
                    //もしくは攻撃オブジェクトが破壊されたら
                    if (targettingObj == null)
                    {
                        foreach(GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject")){
                            if(go.GetComponent<ObjectState>().team == GetComponent<ObjectState>().team) continue;
                            if (Vector3.Distance(transform.position, go.transform.position) <= GetComponent<ObjectState>().RANGE)
                            {
                                act = CHARA_ACT.targetting;
                                GameObject t = new GameObject("target");
                                target.transform.position = go.gameObject.transform.position + Vector3.Normalize(transform.position - go.gameObject.transform.position) * 0.5f;
                                MoveTo(t.transform);
                                targettingObj = go.transform;
                                break;

                            }
                        }
                        //近くに攻撃可オブジェクトがなければ
                        if (targettingObj == null)
                        {
                            act = CHARA_ACT.idle;
                        }
                        break;
                    }

                    if (attackDelay <= 0)
                    {
                        transform.LookAt(targettingObj);
                        anim.SetTrigger("attack");
                        targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
                        attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
                        animState = CharacterAnimState.attack;
                    }
                    else
                    {
                        animState = CharacterAnimState.idle;
                        GameObject t = new GameObject("target");
                        t.transform.position = transform.position;
                        MoveTo(t.transform);
                    }

                    break;
            }




            //if (target != null)
            //{

            //    if (targetting)
            //    {
            //        if (Vector3.Distance(transform.position, target.position) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
            //        {
            //            GameObject t = new GameObject("target");
            //            t.transform.position = transform.position;
            //            MoveTo(t.transform);

            //            if (animState == CharacterAnimState.idle || animState == CharacterAnimState.walk)
            //            {
            //                anim.SetTrigger("attack");
            //                animState = CharacterAnimState.attack;
            //                targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
            //            }

            //            //AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            //            //if (!state.IsName("NormalAttack"))
            //            //{
            //            //    anim.SetTrigger("attack");
            //            //    targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
            //            //}

            //            transform.LookAt(targettingObj);
            //            targetting = false;
            //            targettingObj = null;
            //            //targetに対して攻撃
                        
            //        }
            //    }

            //    agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));


            //    //transform.LookAt (agent.steeringTarget);
            //    //transform.LookAt (Vector3.Lerp (transform.forward + transform.position, agent.steeringTarget, Time.deltaTime * turnSpeed));

            //    float spd = agent.velocity.magnitude;
            //    anim.SetFloat("speed", spd);

            //    if ((transform.position - target.position).magnitude < 0.1f)
            //    {
            //        Destroy(target.gameObject);
            //        target = null;
            //        anim.SetFloat("speed", 0);
            //        animState = CharacterAnimState.idle;
            //    }
            //    else
            //    {
            //        animState = CharacterAnimState.walk;
            //    }

            //}

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
                    anim.SetTrigger("attack");
                    break;


            }

        }

        

	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
        act = CHARA_ACT.freeWalk;
	
	}

    void SetCanvasTeam()
    {
        healthImage.SendMessage("SetTeamColor", (int)GetComponent<ObjectState>().team);

    }

    //死んだ直後
    void RespawnPrepare()
    {

        //respawn位置
        switch (GetComponent<ObjectState>().team)
        {
            case TEAM.BLUE:
                agent.Warp(GameObject.Find("bluePoint").transform.position);

                break;
            case TEAM.RED:
                agent.Warp(GameObject.Find("redPoint").transform.position);

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
    

}
