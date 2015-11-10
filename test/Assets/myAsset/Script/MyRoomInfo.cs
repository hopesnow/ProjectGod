using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MyRoomInfo : MonoBehaviour {
	public GameObject name;
	public GameObject exitButton;
	public GameObject startButton;
	public GameObject client;
	public GameObject[] players;
    public GameObject[] heroIcon;
	public GameObject textPrefab;
	public GameObject playerParent;
    //HeroSelector heroSelector;
    public HeroSelector heroSelector;



    void Awake()
    {

    }

	// Use this for initialization
	void Start () {


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

        int blueNum = 0;
        int redNum = 0;
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {

			if(i >= 6){
				Debug.Log("Angry God");
				break;
			}
            
            if ((TEAM)PhotonNetwork.playerList[i].customProperties["TS"] == TEAM.BLUE)
            {
                players[blueNum * 2].SetActive(true);
                players[blueNum * 2].GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
                heroIcon[blueNum * 2].SetActive(true);
                //players[blueNum * 2].GetComponentInChildren<Image>()
                heroIcon[blueNum * 2].GetComponent<Image>().sprite = heroSelector.GetIcon((HeroCharacter)PhotonNetwork.playerList[i].customProperties["HS"]);
                players[blueNum * 2].transform.FindChild("HeroName").gameObject.GetComponent<Text>().text = heroSelector.GetName((HeroCharacter)PhotonNetwork.playerList[i].customProperties["HS"]);

                blueNum++;
            }
            else if ((TEAM)PhotonNetwork.playerList[i].customProperties["TS"] == TEAM.RED)
            {
                players[redNum * 2 + 1].SetActive(true);
                players[redNum * 2 + 1].GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
                heroIcon[redNum * 2 + 1].SetActive(true);
                heroIcon[redNum * 2 + 1].GetComponent<Image>().sprite = heroSelector.GetIcon((HeroCharacter)PhotonNetwork.playerList[i].customProperties["HS"]);
                players[redNum * 2 + 1].transform.FindChild("HeroName").gameObject.GetComponent<Text>().text = heroSelector.GetName((HeroCharacter)PhotonNetwork.playerList[i].customProperties["HS"]);
                

                redNum++;
            }

            //players[i].SetActive(true);
            //players[i].GetComponent<Text>().text = PhotonNetwork.playerList[i].name;

		}

		while(blueNum < 3){
            players[blueNum * 2].SetActive(false);
            blueNum++;
		}
        while (redNum < 3)
        {
            players[redNum * 2 + 1].SetActive(false);
            redNum++;
        }


	}


}
