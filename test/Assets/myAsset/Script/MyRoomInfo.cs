using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyRoomInfo : MonoBehaviour {
	public GameObject name;
	public GameObject exitButton;
	public GameObject startButton;
	public GameObject client;
	public GameObject[] players;
	public GameObject textPrefab;
	public GameObject playerParent;
    //int blueCount;
    //int redCount;

	// Use this for initialization
	void Start () {
        //redCount = 0;
        //blueCount = 0;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//room初期化処理
	void OpenRoom(){
		playerParent.SetActive (true);
		SetPlayerName ();

	}

	void CloseRoom(){
		playerParent.SetActive (false);

	}

	void SetRoomInfo(Room room){
		name.GetComponent<InputField> ().text = room.name;

	}

	void SetPlayerName(){
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
			//first aid
			if(i >= 6){
				Debug.Log("Angry God");
				break;
			}
			players[i].SetActive(true);
			players[i].GetComponent<Text>().text = PhotonNetwork.playerList[i].name;

		}

		for(int i = PhotonNetwork.playerList.Length;i < 6;i++){
			players[i].SetActive(false);

		}


	}


}
