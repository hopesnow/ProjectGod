using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MoveTo(Vector3 pos){

		transform.position = new Vector3 (pos.x, 0, pos.z);

	}

}
