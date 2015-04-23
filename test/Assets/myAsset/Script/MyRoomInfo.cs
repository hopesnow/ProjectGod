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
	int blueCount;
	int redCount;


	// Use this for initialization
	void Start () {
		players = new GameObject[6];
		redCount = 0;
		blueCount = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetRoomInfo(Room room){
		name.GetComponent<InputField> ().text = room.name;

	}

	void SetPlayerName(){
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
			GameObject obj = (GameObject)Instantiate(textPrefab);
			obj.GetComponentInChildren<Text>().text = PhotonNetwork.playerList[i].name;
			obj.transform.parent = playerParent.transform.parent;
			obj.GetComponent<RectTransform>().position = new Vector3();
			obj.GetComponent<RectTransform>().Translate(0, -50.0f * i + 150, 0);


		}

	}


}
