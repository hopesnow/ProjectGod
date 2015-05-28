using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;




public enum PlayerState{
	room = 0,
	init = 1,
	playstart = 2,
    play = 3,

}

public enum RoomState
{
    wait = 0,
    play = 1,

}


public class MyPhotonClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView = null;
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
//		PhotonNetwork.JoinRandomRoom();
		SetPlayerName ();
		playerName.GetComponent<InputField> ().interactable = true;

	}

	void OnLeftLobby(){
		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(true);
		}
		playerName.GetComponent<InputField> ().interactable = false;

	}

	void OnJoinedRoom(){

		myPhotonView = this.GetComponent<PhotonView> ();


		HashTable h = new HashTable (){{"PS", PlayerState.room}};
		PhotonNetwork.player.SetCustomProperties (h);

		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(false);
		}
        myRoom.SetActive(true);
		myRoom.SendMessage ("SetRoomInfo", PhotonNetwork.room);
		myRoom.SendMessage ("OpenRoom");

        if (myRoom.GetComponent<MyRoomInfo>().blueCount <= myRoom.GetComponent<MyRoomInfo>().redCount)
        {
            HashTable th = new HashTable() { { "TS", TEAM.BLUE } };
            PhotonNetwork.player.SetCustomProperties(th);
            myRoom.GetComponent<MyRoomInfo>().blueCount++;
        }
        else
        {
            HashTable th = new HashTable() { { "TS", TEAM.RED } };
            PhotonNetwork.player.SetCustomProperties(th);
            myRoom.GetComponent<MyRoomInfo>().redCount++;
        }

	}

	void OnLeftRoom(){

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
            if ((RoomState)roomInfo[i].customProperties["RS"] == RoomState.wait)
                roomUI[i].GetComponentInChildren<Button>().interactable = true;
            else roomUI[i].GetComponentInChildren<Button>().interactable = false;

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

		startButton.GetComponent<Button> ().interactable = true;

        RoomOptions ro = new RoomOptions();
        ro.maxPlayers = 6;
        ro.isOpen = true;
        ro.isVisible = true;
        string[] s = { "RS" };
        ro.customRoomPropertiesForLobby = s;
        ro.customRoomProperties = new HashTable() { {"RS", RoomState.wait } };

        PhotonNetwork.CreateRoom(theRoomName.GetComponent<InputField>().text, ro, TypedLobby.Default);


	}

	public void JoinRoom(string roomName){
		PhotonNetwork.JoinRoom (roomName);


	}

	public void StartGame(){
		//Send RPC other players

		PhotonNetwork.room.open = false;

        HashTable h = new HashTable() {{"RS", RoomState.play} };
        PhotonNetwork.room.SetCustomProperties(h);

        PhotonNetwork.DestroyAll();

		if (myPhotonView != null) {
            myPhotonView.RPC("ToGameMain", PhotonTargets.All);
		}

	}

	[RPC]
	void ToGameMain(PhotonMessageInfo info){
        PhotonNetwork.isMessageQueueRunning = false;

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
