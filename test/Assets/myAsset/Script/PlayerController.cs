using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	NavMeshAgent agent;
	Transform target;
	Animator anim;
	public float moveSpeed;
	public float turnSpeed;

	public RectTransform healthImage;

	// Use this for initialization
	void Start () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = true;
		agent.updatePosition = true;

		anim = GetComponentInChildren<Animator> ();

		moveSpeed = 3.0f;
		turnSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 spd = new Vector2();
	
		if (target != null) {

//			if (agent.SetDestination (new Vector3(target.position.x, transform.position.y, target.position.z))) {
//				agent.Stop (true);
//
//				if ((new Vector2 (target.position.x, target.position.z) - new Vector2 (transform.position.x, transform.position.z)).magnitude < moveSpeed * Time.deltaTime) {
//
//					//目的地到達
//					transform.position = new Vector3 (target.position.x, transform.position.y, target.position.z);
//					Destroy (target.gameObject);
//					target = null;
//				
//				} else if ((new Vector2 (agent.steeringTarget.x, agent.steeringTarget.z) - new Vector2 (transform.position.x, transform.position.z)).magnitude <= moveSpeed * Time.deltaTime) {
//					transform.position = new Vector3 (agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
//
//				} else {
//					Vector3 temp = (agent.steeringTarget - transform.position).normalized * moveSpeed * Time.deltaTime;
//					spd = new Vector2 (temp.x, temp.z);
//					//agent.Move (temp);
//
//				}
//
//			}

			agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));

			
			//transform.LookAt (agent.steeringTarget);
			//transform.LookAt (Vector3.Lerp (transform.forward + transform.position, agent.steeringTarget, Time.deltaTime * turnSpeed));

		}

		//animation
		//Vector2 spd = new Vector2 (agent.velocity.x, agent.velocity.z);
		//anim.SetFloat ("speed", agent.velocity.magnitude);




		healthImage.position = Camera.main.WorldToScreenPoint(transform.position);



	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

}
