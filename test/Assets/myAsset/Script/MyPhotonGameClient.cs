using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class MyPhotonGameClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView;


    public GameObject blueStart;
    public GameObject redStart;

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;

		HashTable h = new HashTable (){{"PS", PlayerState.init}};
		PhotonNetwork.player.SetCustomProperties (h);

		myPhotonView = this.GetComponent<PhotonView> ();





	}

	// Update is called once per frame
	void Update () {

		if (PhotonNetwork.player.isMasterClient && (PlayerState)PhotonNetwork.player.customProperties["PS"] == PlayerState.init) {

			int count = 0;
			int otherPlayers = PhotonNetwork.otherPlayers.Length;

			if(otherPlayers != 0){
				foreach(PhotonPlayer pp in PhotonNetwork.otherPlayers){
					if((PlayerState)pp.customProperties["PS"] == PlayerState.init){
						count++;
					}
				}

                
			}

            if (count == otherPlayers)
            {
                myPhotonView.RPC("GameStartPrepare", PhotonTargets.All);

            }

		}

	}

	
	[RPC]
	void GameStartPrepare(){

        GameObject player = PhotonNetwork.Instantiate("ethanPrefab", blueStart.transform.position, Quaternion.identity, 0);
        player.GetComponent<PlayerController>().controllable = true;

        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<CameraControl>().player = player.transform;
        camera.SendMessage("SetPlayerCamera");

        HashTable h = new HashTable(){{"PS", PlayerState.play}};
        PhotonNetwork.player.SetCustomProperties(h);

        PhotonNetwork.room.open = true;


		
	}


}
