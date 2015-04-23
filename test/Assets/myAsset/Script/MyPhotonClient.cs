using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyPhotonClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView;
	public GameObject[] roomUI;
	public GameObject[] roomName;
	public GameObject[] roomMember;
	public GameObject theRoomName;
	public GameObject createButton;
	public GameObject myRoom;
	public GameObject playerName;

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");

	}

	void OnJoinedLobby() {
		Debug.Log("Joined Lobby");
//		PhotonNetwork.JoinRandomRoom();
		SetPlayerName ();
		playerName.GetComponent<InputField> ().interactable = true;

	}

	void OnLeftLobby(){
		Debug.Log ("Lefted Lobby");
		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(true);
		}
		playerName.GetComponent<InputField> ().interactable = false;

	}

	void OnJoinedRoom(){
		Debug.Log ("Join Room Sccess : " + PhotonNetwork.room.name);
		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(false);
		}
		myRoom.SetActive (true);
		myRoom.SendMessage ("SetRoomInfo", PhotonNetwork.room);
		myRoom.SendMessage ("SetPlayerName");
		Debug.Log(PhotonNetwork.playerName);

	}

	void OnLeftRoom(){
		Debug.Log ("Leave Room");
		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(true);
		}
		myRoom.SetActive (false);

	}

	void OnPhotonCreateRoomFailed(){
		Debug.Log ("failed create room");

	}

	//not called if u are in a room(lobby only)
	void OnReceivedRoomListUpdate(){
		RoomInfo[] roomInfo = PhotonNetwork.GetRoomList ();
		for (int i = 0; i < roomInfo.Length; i++) {
			roomName[i].GetComponent<InputField>().text = roomInfo[i].name;
			roomMember[i].GetComponent<Text>().text = roomInfo[i].playerCount.ToString() + "/6";
			roomUI[i].GetComponentInChildren<Button>().interactable = true;
			Debug.Log("Room No." + (i).ToString() + " Player:" + roomInfo[i].playerCount);

		}
		for (int i = roomInfo.Length; i < 3; i++) {

		}

	}

	void OnPhotonPlayerPropertiesChanged(){
		Debug.Log ("よばれたよ");

	}

	void OnPhotonCustomRoomPropertiesChanged(){
		Debug.Log ("room changed");


	}

	void CreateRoom(){
		Debug.Log ("CreateRoom : " + theRoomName.GetComponent<InputField>().text);
		PhotonNetwork.CreateRoom (theRoomName.GetComponent<InputField>().text, true, true, 6);


	}

	public void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom (roomName);

	}

	public void ExitRoom(){
		PhotonNetwork.LeaveRoom ();

	}

	public void SetPlayerName(){
		PhotonNetwork.playerName = playerName.GetComponent<InputField>().text;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
