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
	Transform target;
	Animator anim;

	public GameObject healthImage;
    public GameObject gaugeImage;

    public bool controllable = false;
    public bool targetting = false;
    public Transform targettingObj = null;

    public CharacterAnimState animState;

    public CHARA_ACT act;
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

                    //攻撃可能範囲に入った場合
                    if(Vector3.Distance(GetVecXZ(transform.position), GetVecXZ(target.position)) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH){
                        StopMove();
                        act = CHARA_ACT.autoAttack;
                        break;
                    }

                    anim.SetFloat("speed", agent.velocity.magnitude);
                    animState = CharacterAnimState.walk;

                    break;
                case CHARA_ACT.autoAttack:

                    if (attackDelay <= 0)
                    {
                        transform.LookAt(targettingObj);
                        anim.SetTrigger("attack");
                        animState = CharacterAnimState.attack;
                        targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
                        attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
                    }
                    else
                    {
                        animState = CharacterAnimState.idle;
                    }

                    //targetが死んだら
                    if (!targettingObj)
                    {
                        
                        targetting = false;
                        targettingObj = null;
                        foreach(GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject")){
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

                    break;
            }


            //////////////////////////
            ///second state machine///
            //////////////////////////

            //attackDelay -= Time.deltaTime;

            //string s = "";
            //switch (act)
            //{
            //    case CHARA_ACT.idle:
            //        s = "idle";
            //        break;
            //    case CHARA_ACT.freeWalk:
            //        s = "freeWalk";
            //        break;
            //    case CHARA_ACT.targetting:
            //        s = "targetting";
            //        break;
            //    case CHARA_ACT.autoAttack:
            //        s = "autoAttack";
            //        break;
            //}
            //GameObject.Find("DebugText").GetComponent<Text>().text = s;

            //switch (act)
            //{
            //    case CHARA_ACT.idle:
            //        //基本ないがtargetがいた場合なにか確認する為freeWalkへ
            //        if (target)
            //        {
            //            act = CHARA_ACT.freeWalk;
            //            break;
            //        }

            //        anim.SetFloat("speed", 0);
            //        animState = CharacterAnimState.idle;

            //        break;
            //    case CHARA_ACT.freeWalk:
            //        //移動、攻撃をするかどうか
            //        if (target == null)
            //        {
            //            act = CHARA_ACT.idle;
            //            break;
            //        }
            //        //攻撃対象がいるかどうか
            //        if (targetting)
            //        {
            //            act = CHARA_ACT.targetting;
            //            break;
            //        }
            //        //agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));

            //        //目的地にたどり着いたか、あとモーション等
            //        if ((transform.position - target.position).magnitude < 0.1f)
            //        {
            //            act = CHARA_ACT.idle;
            //            StopMove();
            //        }
            //        else
            //        {
            //            float spd = agent.velocity.magnitude;
            //            anim.SetFloat("speed", spd);
            //            animState = CharacterAnimState.walk;
            //        }

            //        break;
            //    case CHARA_ACT.targetting:

            //        if (Vector3.Distance(transform.position, target.position) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
            //        {
            //            //攻撃範囲に入ったのでそれ以上近づかないように
            //            StopMove();

            //            if (animState == CharacterAnimState.idle || animState == CharacterAnimState.walk)
            //            {
            //                if (attackDelay <= 0)
            //                {
            //                    transform.LookAt(targettingObj);
            //                    anim.SetTrigger("attack");
            //                    targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
            //                    attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
            //                    act = CHARA_ACT.autoAttack;
            //                    animState = CharacterAnimState.attack;
            //                }
            //            }

            //        }

            //        break;
            //    case CHARA_ACT.autoAttack:
            //        //camera controlからのsendmessageで別ケースへ移動する
            //        //もしくは攻撃オブジェクトが破壊されたら
            //        if (targettingObj.gameObject == null)
            //        {
            //            foreach(GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject")){
            //                if(go.GetComponent<ObjectState>().team == GetComponent<ObjectState>().team) continue;
            //                if (Vector3.Distance(transform.position, go.transform.position) <= GetComponent<ObjectState>().RANGE)
            //                {
            //                    act = CHARA_ACT.targetting;
            //                    GameObject t = new GameObject("target");
            //                    target.transform.position = go.transform.position + Vector3.Normalize(transform.position - go.transform.position) * 0.5f;
            //                    MoveTo(t.transform);
            //                    targetting = true;
            //                    targettingObj = go.transform;
            //                    Debug.Log(go);
            //                    break;

            //                }
            //            }
            //            //近くに攻撃可オブジェクトがなければ
            //            if (targettingObj.gameObject == null)
            //            {
            //                act = CHARA_ACT.idle;
            //                StopMove();
            //            }
            //            break;
            //        }

            //        if (attackDelay <= 0)
            //        {
            //            transform.LookAt(targettingObj);
            //            anim.SetTrigger("attack");
            //            targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
            //            attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;
            //            animState = CharacterAnimState.attack;
            //        }
            //        else
            //        {
            //            animState = CharacterAnimState.idle;
            //            StopMove();
            //        }

            //        break;
            //}





            /////////////////////////
            ///first state machine///
            /////////////////////////

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
        targetting = false;

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
