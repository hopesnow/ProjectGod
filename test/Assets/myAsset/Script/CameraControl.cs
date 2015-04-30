using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Transform player;
    //public Transform AI;
	public float dist;

	public float camSpeed;

	public GameObject tapPoint;

	Vector3 movePos;
	Vector2 tapPosition;

	// Use this for initialization
	void Start () {
		dist = 10;
		camSpeed = 15.0f;

        SetPlayerCamera();

		movePos = new Vector3 ();
		tapPosition = new Vector2 ();

	}
	
	// Update is called once per frame
	void Update () {


		//palyer move
#if UNITY_EDITOR || UNITY_STANDALONE
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

#elif UNITY_ANDROID
//		for(int i = 0;i < Input.touchCount;i++){
//
//			if (Input.touches[i].phase == TouchPhase.Began) {
//				Ray ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
//				RaycastHit hit = new RaycastHit();
//				
//				if(Physics.Raycast(ray, out hit)){
//					if(hit.collider.gameObject.CompareTag("field")){
//						GameObject target = new GameObject("target");
//						target.transform.position = hit.point;
//						tapPoint.transform.position = hit.point;
//						player.gameObject.SendMessage("MoveTo", target.transform);
//					}
//				}
//				
//			}
//		}
#endif




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

    void SetPlayerCamera()
    {
        transform.position = GetPlayerCamera();
    }

}
