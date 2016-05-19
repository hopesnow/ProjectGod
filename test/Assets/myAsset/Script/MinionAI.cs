using UnityEngine;
using System.Collections;

//フェーズを作って行動を管理しよう

public enum MINION_ACT
{
    move,
    attack,
    target,
}

public enum MOVE_ROOT
{

    fhase1,//blue:go dir1 red go dir3
    fhase2,//go dir2
    fhase3,//blue:go dir3 red go dir1
    fhase4,//敵本陣へ

}

public enum LANE
{
    top,
    bottom,
}

public class MinionAI : MonoBehaviour {

    NavMeshAgent agent;
    GameObject target;
    Animator anim;
    public CharacterAnimState animState;

    public GameObject healthImage;
    public GameObject gaugeImage;



    MINION_ACT act;
    MOVE_ROOT root;
    public TEAM team;
    public LANE lane;

    public bool MasterAI = false;

    float searchDist;//近くに敵がいた場合攻撃する距離
    float attackDist;//同じ敵に攻撃を仕掛ける距離（rangeとは違う）

	// Use this for initialization
    void Awake()
    {

        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = true;
        agent.updatePosition = true;

        anim = GetComponent<Animator>();
        

        act = MINION_ACT.move;
        root = MOVE_ROOT.fhase1;

        searchDist = 8.0f;
        attackDist = searchDist / 3 * 2;

        GameObject gc = GameObject.Find("Photon");
        gaugeImage = GameObject.Instantiate(gc.GetComponent<MyPhotonGameClient>().gaugePrefab);
        healthImage = GameObject.Instantiate(gc.GetComponent<MyPhotonGameClient>().healthPrefab);
        gaugeImage.transform.SetParent(GameObject.Find("BackCanvas").transform, false);
        healthImage.transform.SetParent(GameObject.Find("FrontCanvas").transform, false);
        gaugeImage.GetComponent<CanvasSetter>().SendMessage("SetTarget", transform);
        healthImage.GetComponent<CanvasSetter>().SendMessage("SetTarget", transform);
        healthImage.GetComponent<CanvasSetter>().SendMessage("SetCopy", gaugeImage.GetComponent<RectTransform>());


    }

    void Start()
    {
        if (MasterAI)
        {
            GetComponent<PhotonView>().RPC("SetTeamAI", PhotonTargets.All, (int)team);
            GetComponent<PhotonView>().RPC("SetLaneAI", PhotonTargets.All, (int)lane);

        }

        

    }
	
	// Update is called once per frame
	void Update () {

        if (MasterAI)//master player
        {
            if (gameObject == null) return;
            switch (act)
            {
                case MINION_ACT.move:

                    foreach(GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject")){
                        if (go.GetComponent<ObjectState>().team == gameObject.GetComponent<ObjectState>().team) continue;
                        if (Vector3.Distance(go.transform.position, transform.position) < searchDist)
                        {//敵発見
                            act = MINION_ACT.target;
                            target = go;
                            agent.SetDestination(target.transform.position);
                            break;
                        }

                    }
                    if (act != MINION_ACT.move) break;//act stateが切り替わっていたら

                    //最後のフェーズではなくかつ、ルート目標に一定距離近づいたら次のルートへ変更
                    if (root != MOVE_ROOT.fhase4 && Vector3.Distance(GetRoot(root, team, lane).position, transform.position) < 0.5)
                    {
                        root = (MOVE_ROOT)((int)root + 1);
                    }
                    agent.SetDestination(GetRoot(root, team, lane).position);
                    
                    

                    break;
                case MINION_ACT.attack:
                    if (target == null)
                    {
                        act = MINION_ACT.move;
                        anim.SetBool("attack", false);
                        break;
                    }
                    anim.SetBool("attack", true);

                    if (Vector3.Distance(transform.position, target.transform.position) > gameObject.GetComponent<ObjectState>().RANGE + target.gameObject.GetComponent<ObjectState>().WIDTH)
                    {
                        anim.SetBool("attack", false);
                        act = MINION_ACT.target;

                    }

                    break;
                case MINION_ACT.target:
                    if (target == null) {
                        act = MINION_ACT.move;
                        break;
                    }
                    if (Vector3.Distance(transform.position, target.transform.position) < gameObject.GetComponent<ObjectState>().RANGE + target.gameObject.GetComponent<ObjectState>().WIDTH)
                    {//攻撃フェーズへ遷移
                        act = MINION_ACT.attack;
                        agent.SetDestination(transform.position);
                    }
                    else if (Vector3.Distance(transform.position, target.transform.position) > searchDist)
                    {
                        act = MINION_ACT.move;

                        target = null;

                    }
                    else if (Vector3.Distance(transform.position, target.transform.position) > attackDist)
                    {
                        GameObject nextGo = null;
                        //minion優先
                        foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        {
                            if (go.GetComponent<ObjectState>().team == gameObject.GetComponent<ObjectState>().team) continue;
                            //if (go.GetComponent<PlayerController>() != null) continue;
                            if (nextGo == null)
                            {
                                nextGo = go;
                                continue;
                            }else if (Vector3.Distance(transform.position, nextGo.transform.position) > Vector3.Distance(transform.position, go.transform.position))
                            {
                                nextGo = go;
                            }

                        }
                        ////minionののちにプレイヤーを狙う
                        //if (nextGo == null)
                        //{
                        //    foreach (GameObject go in GameObject.FindGameObjectsWithTag("canAttackObject"))
                        //    {
                        //        if (go.GetComponent<ObjectState>().team == gameObject.GetComponent<ObjectState>().team) continue;
                        //        if (go.GetComponent<PlayerController>() == null) continue;
                        //        if (nextGo == null)
                        //        {
                        //            nextGo = go;
                        //            continue;
                        //        }
                        //        else if (Vector3.Distance(transform.position, nextGo.transform.position) > Vector3.Distance(transform.position, go.transform.position))
                        //        {
                        //            nextGo = go;
                        //        }

                        //    }
                        //}

                        
                        if (Vector3.Distance(transform.position, nextGo.transform.position) <= searchDist)
                        {//次に近い場所にいる攻撃できるやつが近くにいるときターゲットを切り替える
                            target = nextGo;

                        }


                    }
                    else
                    {
                        agent.SetDestination(target.transform.position);
                    }

                    break;
            }

        }
        else//other player
        {

        }
	
	}

    [PunRPC]
    void SetLaneAI(int l)
    {
        lane = (LANE)l;
    }

    [PunRPC]
    void SetTeamAI(int t)
    {
        team = (TEAM)t;
        healthImage.GetComponent<CanvasSetter>().SendMessage("SetTeamColor", t);

    }

    

    Transform GetRoot(MOVE_ROOT p, TEAM t, LANE l)
    {

        switch (p)
        {
            case MOVE_ROOT.fhase1:
                switch (l)
                {
                    case LANE.top:
                        switch (t)
                        {
                            case TEAM.BLUE:
                                return GameObject.Find("TopDir1").transform;
                            case TEAM.RED:
                                return GameObject.Find("TopDir3").transform;
                        }
                        break;

                    case LANE.bottom:
                        switch (t)
                        {
                            case TEAM.BLUE:
                                return GameObject.Find("BottomDir1").transform;
                            case TEAM.RED:
                                return GameObject.Find("BottomDir3").transform;
                        }
                        break;

                }

                break;
            case MOVE_ROOT.fhase2:
                switch (l)
                {
                    case LANE.top:
                        return GameObject.Find("TopDir2").transform;
                    case LANE.bottom:
                        return GameObject.Find("BottomDir2").transform;
                }

                break;
            case MOVE_ROOT.fhase3:
                switch (l)
                {
                    case LANE.top:
                        switch (t)
                        {
                            case TEAM.BLUE:
                                return GameObject.Find("TopDir3").transform;
                            case TEAM.RED:
                                return GameObject.Find("TopDir1").transform;
                        }
                        break;
                    case LANE.bottom:
                        switch (t)
                        {
                            case TEAM.BLUE:
                                return GameObject.Find("BottomDir3").transform;
                            case TEAM.RED:
                                return GameObject.Find("BottomDir1").transform;
                        }

                        break;
                }

                break;
            case MOVE_ROOT.fhase4:
                switch (t)
                {
                    case TEAM.BLUE:
                        return GameObject.Find("red_Inhibitor").transform;

                    case TEAM.RED:
                        return GameObject.Find("blue_Inhibitor").transform;
                }

                break;
        }

        Debug.Log("RootError");
        return null;

    }

    void DeadAndGiveResource()
    {
        Destroy(gaugeImage);
        Destroy(healthImage);
        Destroy(gameObject);

        //gold exを倒したプレイヤーへ

    }

    public void AttackTarget()
    {
        if (target)
        {
            target.SendMessage("DamageAttack", gameObject.GetComponent<ObjectState>().ATTACK);
        }
        else
        {
            return;
        }
        
        if (target.GetComponent<ObjectState>().HEALTH_RATE <= 0)
        {
            target = null;
        }

    }

}
