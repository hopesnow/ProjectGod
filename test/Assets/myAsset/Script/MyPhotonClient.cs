using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;




public enum PlayerState{
	room = 0,
	init = 1,
	play = 2,

}

public enum RoomState
{
    wait = 0,
    play = 1,

}


public class MyPhotonClient : Photon.MonoBehaviour {
	public PhotonView myPhotonView = null;
	public GameObject[] roomUI;
	public GameObject[] roomName;
	public GameObject[] roomMember;
	public GameObject theRoomName;
	public GameObject createButton;
	public GameObject myRoom;
	public GameObject playerName;
	public GameObject startButton;//ownerのみOnにする




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

		myPhotonView = this.GetComponent<PhotonView> ();

		HashTable h = new HashTable (){{"PS", PlayerState.room}};
		PhotonNetwork.player.SetCustomProperties (h);

		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(false);
		}
		myRoom.SetActive (true);
		myRoom.SendMessage ("SetRoomInfo", PhotonNetwork.room);
		myRoom.SendMessage ("OpenRoom");
		Debug.Log("playerName : " + PhotonNetwork.playerName);

	}

	void OnLeftRoom(){
		Debug.Log ("Leave Room");

		myPhotonView = null;

		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(true);
		}
		myRoom.SendMessage ("CloseRoom");
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

	void OnPhotonPlayerConnected (){
		myRoom.SendMessage ("SetPlayerName");

	}

	void OnPhotonPlayerDisconnected (){
		myRoom.SendMessage ("SetPlayerName");

	}

	void CreateRoom(){
		Debug.Log ("CreateRoom : " + theRoomName.GetComponent<InputField>().text);
		PhotonNetwork.CreateRoom (theRoomName.GetComponent<InputField>().text, true, true, 6);
		startButton.GetComponent<Button> ().interactable = true;

        HashTable h = new HashTable() {{"RS", RoomState.wait}};
        

	}

	public void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom (roomName);


	}

	public void StartGame(){
		Debug.Log ("GameMain へ移行");
		//Send RPC other players

		PhotonNetwork.room.open = false;

//		PhotonNetwork.DestroyAll ();

		if (myPhotonView != null) {
			myPhotonView.RPC ("ToGameMain", PhotonTargets.All);
			Debug.Log("send success");
		}

	}

	[RPC]
	void ToGameMain(){
		PhotonNetwork.isMessageQueueRunning = false;
		Debug.Log ("to game main");
		Application.LoadLevel ("GameMain");

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
