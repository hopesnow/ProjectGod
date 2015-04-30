using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	NavMeshAgent agent;
	Transform target;
	Animator anim;
	public float moveSpeed;
	public float turnSpeed;

	public RectTransform healthImage;
    public RectTransform gaugeImage;

    public bool controllable = false;

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

        if (controllable)
        {

            if (target != null)
            {


                agent.SetDestination(new Vector3(target.position.x, transform.position.y, target.position.z));


                //transform.LookAt (agent.steeringTarget);
                //transform.LookAt (Vector3.Lerp (transform.forward + transform.position, agent.steeringTarget, Time.deltaTime * turnSpeed));

            }

        }

		healthImage.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 40, 0);
        gaugeImage.position = healthImage.position;



	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

}
