using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;
	Animator anim;

	// Use this for initialization
	void Start () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = false;
		agent.updatePosition = true;

		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target != null) {

			agent.SetDestination(target.position);


		}

		//animation
		Vector2 speed = new Vector2 (agent.velocity.x, agent.velocity.z);
		anim.SetFloat ("speed", speed.magnitude);

		transform.LookAt (transform.position + agent.velocity);


	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

}
