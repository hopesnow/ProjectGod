using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public Transform player;
	public Transform AI;
	public float dist;

	// Use this for initialization
	void Start () {
		dist = 8;
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (player.position.x, player.position.y + dist * (Mathf.Sqrt(3) / 2), player.position.z - dist / 2);

		if (Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if(Physics.Raycast(ray, out hit)){

				if(hit.collider.gameObject.CompareTag("field")){

					GameObject target = new GameObject("target");
					target.transform.position = hit.point;
					player.gameObject.SendMessage("MoveTo", target.transform);

				}

			}

		}

	}
}
