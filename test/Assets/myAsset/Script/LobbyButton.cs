using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour {
	public GameObject client;
	public GameObject name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void JoinRoom(){
		client.SendMessage ("JoinRoom", name.GetComponent<InputField> ().text);

	}

	public void ExitRoom(){


	}

}
