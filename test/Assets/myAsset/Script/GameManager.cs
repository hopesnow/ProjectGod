using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject[] redTeamPlayer;
	public GameObject[] blueTeamPlayer;

	public GameObject healthPrefabRed;
	public GameObject healthPrefabBlue;
	public GameObject healthPrefabGreen;
	public Sprite[] healthSprite;

	public Transform redStart;
	public Transform blueStart;

	// Use this for initialization
	void Start () {
		redTeamPlayer = new GameObject[3];
		blueTeamPlayer = new GameObject[3];

	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void AddRedPlayer(GameObject player){
		for (int i = 0; i < redTeamPlayer.Length; i++) {
			if(redTeamPlayer[i] == null){
				redTeamPlayer[i] = player;
				GameObject health = (GameObject)Instantiate(healthPrefabRed);
				player.GetComponent<PlayerController>().healthImage = health.GetComponent<RectTransform>();
				player.transform.position = redStart.position;

			}

		}

	}

	void AddBluePlayer(GameObject player){
		for (int i = 0; i < blueTeamPlayer.Length; i++) {
			if(blueTeamPlayer[i] == null){
				blueTeamPlayer[i] = player;
				GameObject health = (GameObject)Instantiate(healthPrefabBlue);
				player.GetComponent<PlayerController>().healthImage = health.GetComponent<RectTransform>();
				player.transform.position = blueStart.position;

			}

		}

	}

}
