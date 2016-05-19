using UnityEngine;
using System.Collections;

public class screenShotter : MonoBehaviour {

    public float speed = 5.0f;

    private Vector3 oldPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = transform.position;
        

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))//left
        {
            pos -= transform.right * speed;

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))//right
        {
            pos += transform.right * speed;

        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))//up
        {
            pos += transform.forward * speed;

        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))//down
        {
            pos -= transform.forward * speed;

        }

        transform.position = pos;
	
	}

    void MouseEvent()
    {
        if (Input.GetMouseButtonDown(1))
        {

            oldPos = Input.mousePosition;

            Vector3 diff = Input.mousePosition - oldPos;
            if (diff.magnitude < Vector3.kEpsilon)
            {
                //差分の長さが極少数より小さかったらドラッグしていないと判断する
                return;
            }

            

        }

    }


}
