using UnityEngine;
using System.Collections;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class MyPhotonGameClient : Photon.MonoBehaviour {
	private PhotonView myPhotonView;

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

                if (count == otherPlayers)
                {
                    myPhotonView.RPC("GameStartPrepare", PhotonTargets.All);

                }
			}


		}

	}

	
	[RPC]
	void GameStartPrepare(){

        HashTable h = new HashTable(){{"PS", PlayerState.play}};
        PhotonNetwork.player.SetCustomProperties(h);

        PhotonNetwork.room.open = true;
		
	}


}
