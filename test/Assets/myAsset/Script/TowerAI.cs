using UnityEngine;
using System.Collections;

public enum TOWER_ACT
{
    idle,
    targetting,
    attack,
    destroyed,

}

public class TowerAI : MonoBehaviour {

    GameObject target;

    TOWER_ACT act;
    public TEAM team;

    float searchDist;

    float attackDelay = 0;


    void Awake()
    {
        act = TOWER_ACT.idle;
        target = null;
        

    }

	// Use this for initialization
	void Start () {

        searchDist = GetComponent<TowerState>().RANGE;
        attackDelay = 0;
	
	}
	
	// Update is called once per frame
	void Update () {

        if (PhotonNetwork.player.isMasterClient) {
            switch (act)
            {
                case TOWER_ACT.idle:
                    if (PhotonNetwork.player.isMasterClient)
                    {

                        //minion優先のため
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        {
                            if (go.GetComponent<ObjectState>().team == team) continue;
                            if (Vector3.Distance(transform.position, go.transform.position) <= searchDist)
                            {
                                act = TOWER_ACT.targetting;
                                target = go;
                                break;
                            }

                        }
                        //minionののちプレイヤーを狙う
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject)"))
                        {
                            if (go.GetComponent<ObjectState>().team == team) continue;
                            if (Vector3.Distance(transform.position, go.transform.position) <= searchDist)
                            {
                                act = TOWER_ACT.targetting;
                                target = go;
                                break;
                            }

                        }


                    }

                    break;
                case TOWER_ACT.targetting:
                    //ターゲットが既に倒されていたら
                    if (target == null)
                    {
                        act = TOWER_ACT.idle;
                        break;
                    }
                    //ターゲットが範囲内にいるか
                    if (Vector3.Distance(transform.position, target.transform.position) > searchDist)
                    {
                        act = TOWER_ACT.idle;
                        target = null;
                        break;
                    }

                    //アタックタイミングかどうか
                    attackDelay -= Time.deltaTime;
                    if (attackDelay < 0)
                    {
                        act = TOWER_ACT.attack;
                    }

                    break;
                case TOWER_ACT.attack:
                    //do attack method
                    AttackTarget();
                    act = TOWER_ACT.targetting;
                    attackDelay = GetComponent<ObjectState>().NEXT_ATTACK;

                    break;
                case TOWER_ACT.destroyed:

                    break;

            }

    }

	
	}

    void DeadAndGiveResource()
    {

        gameObject.tag = "destroyedObject";
        act = TOWER_ACT.destroyed;

    }

    void AttackTarget()
    {

        if (target)
        {
            
            GameObject attacker = PhotonNetwork.Instantiate("TowerAttacker", transform.position + new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0), Quaternion.identity, 0);
            attacker.GetComponent<TowerAttaker>().master = true;
            attacker.GetComponent<TowerAttaker>().Spawn(target, GetComponent<ObjectState>().ATTACK);

        }

    }

}
