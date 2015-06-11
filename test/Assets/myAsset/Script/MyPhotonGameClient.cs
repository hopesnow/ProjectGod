using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class MyPhotonGameClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView;


    GameObject blueStart;
    GameObject redStart;

    GameObject player;

    public GameObject victory;
    public GameObject defeat;

    public GameObject blueTopGate;
    public GameObject blueBottomGate;
    public GameObject redTopGate;
    public GameObject redBottomGate;

    public GameObject gaugePrefab;
    public GameObject healthPrefab;

    float startTime;
    float nextMinion;

	// Use this for initialization
    void Awake()
    {
        PhotonNetwork.isMessageQueueRunning = true;

        HashTable h = new HashTable() { { "PS", PlayerState.init } };
        PhotonNetwork.player.SetCustomProperties(h);

        myPhotonView = this.GetComponent<PhotonView>();

        nextMinion = 10;

        blueStart = GameObject.Find("bluePoint");
        redStart = GameObject.Find("redPoint");
    }

	// Update is called once per frame
	void Update () {


        if (PhotonNetwork.player.isMasterClient && (PlayerState)PhotonNetwork.player.customProperties["PS"] == PlayerState.init)
        {

            int count = 0;
            int otherPlayers = PhotonNetwork.otherPlayers.Length;

            if (otherPlayers != 0)
            {
                foreach (PhotonPlayer pp in PhotonNetwork.otherPlayers)
                {
                    if ((PlayerState)pp.customProperties["PS"] == PlayerState.init)
                    {
                        count++;
                    }
                }


            }

            if (count == otherPlayers)
            {
                //全員init終わったらprepare実行
                myPhotonView.RPC("GameStartPrepare", PhotonTargets.All);

            }

        }
        //}else if(PhotonNetwork.player.isMasterClient && (PlayerState)PhotonNetwork.player.customProperties["PS"] == PlayerState.playstart){

        //    int count = 0;
        //    int otherPlayers = PhotonNetwork.otherPlayers.Length;

        //    if (otherPlayers != 0)
        //    {
        //        foreach (PhotonPlayer pp in PhotonNetwork.otherPlayers)
        //        {
        //            if ((PlayerState)pp.customProperties["PS"] == PlayerState.playstart)
        //            {
        //                count++;
        //            }
        //        }
        //    }

        //    if (count == otherPlayers)
        //    {
        //        //全員prepare終わったらゲーム開始直前処理
        //        myPhotonView.RPC("GameStarting", PhotonTargets.All);
        //    }

        //}

        if (PhotonNetwork.player.isMasterClient && (PlayerState)PhotonNetwork.player.customProperties["PS"] == PlayerState.play)
        {
            if ((Time.time - startTime) > nextMinion)
            {
                nextMinion += 60;
                //minionPop
                for (int i = 0; i < 4; i++)
                {
                    if (i < 3)
                    {
                        Invoke("MeleeMinionCreate", 1.0f * i);
                    }
                    else
                    {
                        Invoke("RangeMinionCreate", 1.0f * i);
                    }
                }

            }

        }

	}
	
	[RPC]
	void GameStartPrepare(){

        startTime = Time.time;

        Vector3 startPos;
        if ((TEAM)PhotonNetwork.player.customProperties["TS"] == TEAM.BLUE)
        {
            startPos = blueStart.transform.position;
        }
        else
        {
            startPos = redStart.transform.position;
        }
        player = PhotonNetwork.Instantiate("ethanPrefab", startPos, Quaternion.identity, 0);
        player.GetComponent<PlayerController>().controllable = true;


        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<CameraControl>().player = player.transform;
        camera.SendMessage("SetPlayerCamera");

        HashTable h = new HashTable(){{"PS", PlayerState.play}};
        PhotonNetwork.player.SetCustomProperties(h);

        PhotonNetwork.room.open = true;

        player.GetComponent<ObjectState>().team = (TEAM)PhotonNetwork.player.customProperties["TS"];
        player.GetComponent<ObjectState>().SendMessage("SendTeam");


	}

    //[RPC]
    //void GameStarting()
    //{

    //    player.GetComponent<PlayerController>().controllable = true;
    //    player.GetComponent<Collider>().enabled = false;

    //    GameObject[] statusObj = GameObject.FindGameObjectsWithTag("canAttackObject");
    //    foreach (GameObject go in statusObj)
    //    {
    //        if (go.GetComponent<ObjectState>().team == player.GetComponent<ObjectState>().team)
    //        {
    //            go.GetComponent<Collider>().enabled = false;
    //        }

    //    }

    //    HashTable h = new HashTable() { { "PS", PlayerState.play } };
    //    PhotonNetwork.player.SetCustomProperties(h);

    //    PhotonNetwork.room.open = true;

    //}

    void GameEndRPC(string defeatBase)
    {
        myPhotonView.RPC("GameEnd", PhotonTargets.All, defeatBase);

    }

    [RPC]
    void GameEnd(string defeatBase)
    {

        GameObject.Find("Main Camera").SendMessage("EndGame", defeatBase);
        //GameObject.Find(defeatBase).transform.localScale = Vector3.zero;
        if (GameObject.Find(defeatBase).GetComponent<ObjectState>().team != (TEAM)PhotonNetwork.player.customProperties["TS"])
        {//victory
            Invoke("Victory", 2.0f);
        }
        else //defeat
        {
            Invoke("Defeat", 2.0f);
        }

    }

    void Victory()
    {

        victory.SetActive(true);
        victory.GetComponent<Image>().color = new Color(255, 255, 255, 255);

    }

    void Defeat()
    {

        defeat.SetActive(true);
        defeat.GetComponent<Image>().color = new Color(255, 255, 255, 255);

    }

    void MeleeMinionCreate()
    {
        BlueTopMinionPop(PhotonNetwork.Instantiate("Goblin_Sword", blueTopGate.transform.position, Quaternion.identity, 0));
        BlueBottomMinionPop(PhotonNetwork.Instantiate("Goblin_Sword", blueBottomGate.transform.position, Quaternion.identity, 0));
        RedTopMinionPop(PhotonNetwork.Instantiate("Goblin_Sword", redTopGate.transform.position, Quaternion.identity, 0));
        RedBottomMinionPop(PhotonNetwork.Instantiate("Goblin_Sword", redBottomGate.transform.position, Quaternion.identity, 0));

    }

    void RangeMinionCreate()
    {
        BlueTopMinionPop(PhotonNetwork.Instantiate("Goblin_Bow", blueTopGate.transform.position, Quaternion.identity, 0));
        BlueBottomMinionPop(PhotonNetwork.Instantiate("Goblin_Bow", blueBottomGate.transform.position, Quaternion.identity, 0));
        RedTopMinionPop(PhotonNetwork.Instantiate("Goblin_Bow", redTopGate.transform.position, Quaternion.identity, 0));
        RedBottomMinionPop(PhotonNetwork.Instantiate("Goblin_Bow", redBottomGate.transform.position, Quaternion.identity, 0));

    }

    
    void BlueTopMinionPop(GameObject minion)
    {
        MinionAI ai = minion.GetComponent<MinionAI>();
        ai.MasterAI = true;
        ai.team = TEAM.BLUE;
        ai.lane = LANE.top;
        minion.GetComponent<MinionState>().team = TEAM.BLUE;
        

    }


    void BlueBottomMinionPop(GameObject minion)
    {
        MinionAI ai = minion.GetComponent<MinionAI>();
        ai.MasterAI = true;
        ai.team = TEAM.BLUE;
        ai.lane = LANE.bottom;
        minion.GetComponent<MinionState>().team = TEAM.BLUE;

    }


    void RedTopMinionPop(GameObject minion)
    {
        MinionAI ai = minion.GetComponent<MinionAI>();
        ai.MasterAI = true;
        ai.team = TEAM.RED;
        ai.lane = LANE.top;
        minion.GetComponent<MinionState>().team = TEAM.RED;

    }


    void RedBottomMinionPop(GameObject minion)
    {
        MinionAI ai = minion.GetComponent<MinionAI>();
        ai.MasterAI = true;
        ai.team = TEAM.RED;
        ai.lane = LANE.bottom;
        minion.GetComponent<MinionState>().team = TEAM.RED;

    }

}
