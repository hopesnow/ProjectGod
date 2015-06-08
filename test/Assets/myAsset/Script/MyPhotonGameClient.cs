﻿using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class MyPhotonGameClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView;


    public GameObject blueStart;
    public GameObject redStart;

    GameObject player;

    public GameObject victory;
    public GameObject defeat;

    public GameObject blueTopMinion;
    public GameObject blueBottomMinion;
    public GameObject redTopMinion;
    public GameObject redBottomMinion;

    float startTime;
    float nextMinion;

	// Use this for initialization
    void Awake()
    {
        PhotonNetwork.isMessageQueueRunning = true;

        HashTable h = new HashTable() { { "PS", PlayerState.init } };
        PhotonNetwork.player.SetCustomProperties(h);

        myPhotonView = this.GetComponent<PhotonView>();

        nextMinion = 30;

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

        if (PhotonNetwork.player.isMasterClient)
        {
            if ((Time.time - startTime) > nextMinion)
            {
                nextMinion += 60;
                //minionPop
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

    [RPC]
    void BlueTopMinionPop()
    {

    }

    [RPC]
    void BlueBottomMinionPop()
    {

    }

    [RPC]
    void RedTopMinionPop()
    {

    }

    [RPC]
    void RedBottomMinionPop()
    {

    }

}
