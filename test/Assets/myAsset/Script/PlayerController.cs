using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public NavMeshAgent agent;
	public Transform target;

	// Use this for initialization
	void Start () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = false;
		agent.updatePosition = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target != null) {

			agent.SetDestination(target.position);


		}


	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

}
