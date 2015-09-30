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

//room hash
//"RS" room state
//"BN" blue num
//"RN" red num  


//player hash
//"TS" team state
//"PS" player state
//"HS" hero state (character state)

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
    public GameObject heroSelect;



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

        int blueNum = (int)PhotonNetwork.room.customProperties["BN"];
        int redNum = (int)PhotonNetwork.room.customProperties["RN"];
        if (blueNum <= redNum)
        {
            HashTable th = new HashTable() { { "TS", TEAM.BLUE } };
            PhotonNetwork.player.SetCustomProperties(th);
            HashTable rh = new HashTable() { { "BN", (blueNum + 1) } };
            PhotonNetwork.room.SetCustomProperties(rh);
        }
        else
        {
            HashTable th = new HashTable() { { "TS", TEAM.RED } };
            PhotonNetwork.player.SetCustomProperties(th);
            HashTable rh = new HashTable() { { "RN", (redNum + 1) } };
            PhotonNetwork.room.SetCustomProperties(rh);
        }

		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(false);
		}
        myRoom.SetActive(true);
		myRoom.SendMessage ("SetRoomInfo", PhotonNetwork.room);
		myRoom.SendMessage ("OpenRoom");
        myPhotonView.RPC("RoomUpdate", PhotonTargets.Others);
        heroSelect.SetActive(true);

	}

	void OnLeftRoom(){

		myPhotonView = null;

		for (int i = 0; i < roomUI.Length; i++) {
			roomUI[i].SetActive(true);
		}
		myRoom.SendMessage ("CloseRoom");
		myRoom.SetActive (false);
        heroSelect.SetActive(false);

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

    //誰かが自分のルームと同じところに入ってきたら
	void OnPhotonPlayerConnected (){
        //myRoom.SendMessage ("SetPlayerName");

	}

    //誰かが自分のルームから出て行ったら
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
        ro.customRoomPropertiesForLobby = s;//lobbyで認識できるルームプロパティ。のはず
        ro.customRoomProperties = new HashTable() { { "RS", RoomState.wait }, { "BN", 0 }, { "RN", 0 } };

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

    public void JoinBlueTeam()
    {
        if ((TEAM)PhotonNetwork.player.customProperties["TS"] == TEAM.BLUE) return;
        int blueNum = (int)PhotonNetwork.room.customProperties["BN"];
        int redNum = (int)PhotonNetwork.room.customProperties["RN"];
        if (blueNum < 3)
        {
            HashTable th = new HashTable() { { "TS", TEAM.BLUE } };
            PhotonNetwork.player.SetCustomProperties(th);
            HashTable bh = new HashTable() { { "BN", (blueNum + 1) } };
            PhotonNetwork.room.SetCustomProperties(bh);
            HashTable rh = new HashTable() { { "RN", (redNum - 1) } };
            PhotonNetwork.room.SetCustomProperties(rh);
            myPhotonView.RPC("RoomUpdate", PhotonTargets.All);
        }

    }

    public void JoinRedTeam()
    {
        if ((TEAM)PhotonNetwork.player.customProperties["TS"] == TEAM.RED) return;
        int blueNum = (int)PhotonNetwork.room.customProperties["BN"];
        int redNum = (int)PhotonNetwork.room.customProperties["RN"];
        if (redNum < 3)
        {
            HashTable th = new HashTable() { { "TS", TEAM.RED } };
            PhotonNetwork.player.SetCustomProperties(th);
            HashTable rh = new HashTable() { { "RN", (redNum + 1) } };
            PhotonNetwork.room.SetCustomProperties(rh);
            HashTable bh = new HashTable() { { "BN", (blueNum - 1) } };
            PhotonNetwork.room.SetCustomProperties(bh);
            myPhotonView.RPC("RoomUpdate", PhotonTargets.All);
        }

    }

    [RPC]
    void RoomUpdate()
    {
        myRoom.SendMessage("SetPlayerName");
    }

	[RPC]
	void ToGameMain(PhotonMessageInfo info){
        PhotonNetwork.isMessageQueueRunning = false;

		Application.LoadLevel ("Stage1");

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
