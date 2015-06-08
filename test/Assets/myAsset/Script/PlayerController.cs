﻿using UnityEngine;
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

	}
	
	// Update is called once per frame
	void Update () {

        if (controllable)
        {

            if (target != null)
            {

                if (targetting)
                {
                    if (Vector3.Distance(transform.position, target.position) < gameObject.GetComponent<ObjectState>().RANGE + targettingObj.gameObject.GetComponent<ObjectState>().WIDTH)
                    {
                        GameObject t = new GameObject("target");
                        t.transform.position = transform.position;
                        MoveTo(t.transform);

                        if (animState == CharacterAnimState.idle || animState == CharacterAnimState.walk)
                        {
                            anim.SetTrigger("attack");
                            animState = CharacterAnimState.attack;
                            targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
                        }

                        //AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
                        //if (!state.IsName("NormalAttack"))
                        //{
                        //    anim.SetTrigger("attack");
                        //    targettingObj.gameObject.GetComponent<ObjectState>().SendMessage("DamageAttack", GetComponent<ObjectState>().ATTACK);
                        //}

                        transform.LookAt(targettingObj);
                        targetting = false;
                        targettingObj = null;
                        //targetに対して攻撃
                        
                    }
                }

                agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));


                //transform.LookAt (agent.steeringTarget);
                //transform.LookAt (Vector3.Lerp (transform.forward + transform.position, agent.steeringTarget, Time.deltaTime * turnSpeed));

                float spd = agent.velocity.magnitude;
                anim.SetFloat("speed", spd);

                if ((transform.position - target.position).magnitude < 0.1f)
                {
                    Destroy(target.gameObject);
                    target = null;
                    anim.SetFloat("speed", 0);
                    animState = CharacterAnimState.idle;
                }
                else
                {
                    animState = CharacterAnimState.walk;
                }

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
