using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	Animator anim;
	public float moveSpeed;
	public float turnSpeed;

	// Use this for initialization
	void Start () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = false;
		agent.updatePosition = true;

		anim = GetComponentInChildren<Animator> ();

		moveSpeed = 3.0f;
		turnSpeed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target != null) {

			if(agent.SetDestination(target.position)){
				agent.Stop ();

				if((new Vector2(agent.steeringTarget.x, agent.steeringTarget.z) - new Vector2(transform.position.x, transform.position.z)).magnitude <= moveSpeed * Time.deltaTime){

					if((new Vector2(target.position.x, target.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude < moveSpeed * Time.deltaTime){
						//目的地到達
						transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
						Destroy(target.gameObject);
						target = null;

					}else{

						transform.position = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);

					}

				}else{
					agent.Move((agent.steeringTarget - transform.position).normalized * moveSpeed * Time.deltaTime);
				}

			}

			//agent.SetDestination(target.position);
			//if((new Vector2(target.position.x, target.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude < speed * Time.deltaTime){
			//	transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
			//	Destroy(target.gameObject);
			//	target = null;
			//}else{
			//	agent.Move((agent.steeringTarget - transform.position).normalized * speed * Time.deltaTime);
			//}

		}

		//animation
		Vector2 spd = new Vector2 (agent.velocity.x, agent.velocity.z);
		anim.SetFloat ("speed", spd.magnitude);

		//transform.LookAt (agent.steeringTarget);
		transform.LookAt (Vector3.Lerp (transform.forward + transform.position, agent.steeringTarget, Time.deltaTime * turnSpeed));




	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

}
