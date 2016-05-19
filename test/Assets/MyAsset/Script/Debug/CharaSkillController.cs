using UnityEngine;
using System.Collections;

public class CharaSkillController : MonoBehaviour {

    Vector3 defaultPos;
    Quaternion defaultRot;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
        defaultPos = transform.position;
        defaultRot = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("skill1");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("skill2");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("skill3");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = defaultPos;
            transform.rotation = defaultRot;
        }
        

	
	}
}
