using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	NavMeshAgent agent;
	Transform target;
	Animator anim;
	public float moveSpeed;
	public float turnSpeed;

	public GameObject healthImage;
    public GameObject gaugeImage;

    public bool controllable = false;

    public bool 

	// Use this for initialization
	void Awake () {
	
		agent = GetComponentInChildren<NavMeshAgent> ();

		agent.updateRotation = true;
		agent.updatePosition = true;

        GameObject[] gigo = GameObject.FindGameObjectsWithTag("gaugeBack");
        for (int i = 0; i < gigo.Length; i++)
        {
            if (gigo[i].GetComponent<RectTransform>().localScale == Vector3.zero)
            {
                gigo[i].GetComponent<RectTransform>().localScale = Vector3.one;
                gaugeImage = gigo[i];
                gigo[i].SendMessage("SetTarget", transform);
                break;
            }
        }

        GameObject[] higo = GameObject.FindGameObjectsWithTag("healthGauge");
        for (int i = 0; i < higo.Length; i++)
        {
            if (higo[i].GetComponent<RectTransform>().localScale == Vector3.zero)
            {
                higo[i].GetComponent<RectTransform>().localScale = Vector3.one;
                healthImage = higo[i];
                higo[i].SendMessage("SetTarget", transform);
                break;
            }
        }


        anim = GetComponentInChildren<Animator>();

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


	}

	void MoveTo(Transform target){
	
		if (this.target != null) {
			Destroy(this.target.gameObject);
		}
		this.target = target;
	
	}

    void SetHealthGauge(int number)
    {

    }

}
