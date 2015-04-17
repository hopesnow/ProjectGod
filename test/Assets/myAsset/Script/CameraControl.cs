using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Transform player;
	public Transform AI;
	public float dist;

	public float camSpeed;

	public GameObject tapPoint;

	Vector3 movePos;

	// Use this for initialization
	void Start () {
		dist = 10;
		camSpeed = 15.0f;

		transform.position = GetPlayerCamera();

		movePos = new Vector3 ();

	}
	
	// Update is called once per frame
	void Update () {


		//palyer move
		if (Input.GetMouseButtonDown (1)) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if(Physics.Raycast(ray, out hit)){

				if(hit.collider.gameObject.CompareTag("field")){

					GameObject target = new GameObject("target");
					target.transform.position = hit.point;
					tapPoint.transform.position = hit.point;
					player.gameObject.SendMessage("MoveTo", target.transform);

				}

			}

		}

		//Camera Moving
		if (Input.GetMouseButton (0)) {

			//Moving Map Mode LoL
//			if (Input.GetMouseButtonDown (0)) {
//				movePos = Input.mousePosition;
//
//			}
//
//			transform.position = transform.position + new Vector3(Input.mousePosition.x - movePos.x, 0, Input.mousePosition.y - movePos.y) * 0.01f;

			//Moving Map Mode VG
			if(Input.GetMouseButtonDown(0)){

			}else{
				transform.position = transform.position - new Vector3(Input.mousePosition.x - movePos.x, 0, Input.mousePosition.y - movePos.y) * 0.03f;
			}
			movePos = Input.mousePosition;
			

		}

#if UNITY_ANDROID
		transform.position = Vector3.Lerp (transform.position, GetPlayerCamera(), Time.deltaTime * camSpeed);
#endif
		//Camera Move to Player
		if (Input.GetKey(KeyCode.Space)) {
			transform.position = Vector3.Lerp (transform.position, GetPlayerCamera(), Time.deltaTime * camSpeed);
		} else {
			
		}


	}

	Vector3 GetPlayerCamera(){
		Vector3 pos = new Vector3 (player.position.x, player.position.y + dist * (Mathf.Sqrt (3) / 2), player.position.z - dist / 2);
		return pos;
	}
}
