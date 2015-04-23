using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyRoomInfo : MonoBehaviour {
	public GameObject name;
	public GameObject exitButton;
	public GameObject startButton;
	public GameObject client;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetRoomInfo(Room room){
		name.GetComponent<Text> ().text = room.name;

	}


}
