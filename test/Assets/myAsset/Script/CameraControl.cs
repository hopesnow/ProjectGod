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

    bool ending = false;
    GameObject endObject = null;

    CharacterSkill cSkill;

    void Awake()
    {
        dist = 15;
        camSpeed = 15.0f;

        movePos = new Vector3();
        tapPosition = new Vector2();

    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if(ending){

            transform.position = Vector3.Lerp(transform.position, GetCameraPos(endObject.transform.position), Time.deltaTime * 2.0f);

            return;

        }

        if (!player.GetComponent<PlayerController>().DYING)
        {

            //player move
#if UNITY_EDITOR || UNITY_STANDALONE


            if (Input.GetMouseButtonDown(1))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();

                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.collider.gameObject.CompareTag("field"))
                    {
                        GameObject target = new GameObject("target");
                        target.transform.position = hit.point;
                        tapPoint.transform.position = hit.point;
                        tapPoint.GetComponent<ParticleSystem>().Play();
                        player.gameObject.GetComponent<PlayerController>().targetting = false;
                        player.gameObject.SendMessage("MoveTo", target.transform);

                    }
                    else if (hit.collider.gameObject.CompareTag("canAttackObject"))
                    {

                        if (player.GetComponent<ObjectState>().team != hit.collider.gameObject.GetComponent<ObjectState>().team)
                        {
                            player.gameObject.GetComponent<PlayerController>().SetTargetFromObj(hit.collider.gameObject);
                            tapPoint.transform.position = hit.collider.gameObject.transform.position + Vector3.Normalize(player.transform.position - hit.collider.gameObject.transform.position) * 0.5f;
                            tapPoint.GetComponent<ParticleSystem>().Play();
                        }
                        else
                        {
                            GameObject target = new GameObject("target");
                            //最後の1.5fはオブジェクトの少し外側に行くように（navmesh対策）
                            target.transform.position = hit.collider.gameObject.transform.position + Vector3.Normalize(player.transform.position - hit.collider.gameObject.transform.position) * (hit.collider.gameObject.GetComponent<CapsuleCollider>().radius + 1.5f);
                            tapPoint.transform.position = target.transform.position;
                            tapPoint.GetComponent<ParticleSystem>().Play();
                            player.gameObject.GetComponent<PlayerController>().SendMessage("MoveTo", target.transform);
                        }

                    }

                }

            }

#elif UNITY_ANDROID


        if (Input.GetMouseButtonDown (0)) {
		
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
		
			if(Physics.Raycast(ray, out hit)){
		
				if(hit.collider.gameObject.CompareTag("field")){

                    GameObject target = new GameObject("target");
                    target.transform.position = hit.point;
                    tapPoint.transform.position = hit.point;
                    player.gameObject.SendMessage("MoveTo", target.transform);
		
				}
                else if(hit.collider.gameObject.CompareTag("canAttackObject"))
                {
                    GameObject target = new GameObject("target");
                    target.transform.position = hit.collider.gameObject.transform.position + Vector3.Normalize(player.transform.position - hit.collider.gameObject.transform.position) * 0.5f;
                    tapPoint.transform.position = target.transform.position;
                    player.gameObject.SendMessage("MoveTo", target.transform);
                    player.gameObject.GetComponent<PlayerController>().targetting = true;
                    player.gameObject.GetComponent<PlayerController>().targettingObj = hit.collider.gameObject.transform;

                }
		
			}
		
		}

#endif

        }//死んでるときはタップできないように


#if UNITY_EDITOR || UNITY_STANDALONE

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

#endif

#if UNITY_EDITOR || UNITY_STANDALONE
		//Camera Move to Player
		if (Input.GetKey(KeyCode.Space)) {
			transform.position = Vector3.Lerp (transform.position, GetPlayerCamera(), Time.deltaTime * camSpeed);
		} else {

		}
#elif UNITY_ANDROID
        transform.position = Vector3.Lerp(transform.position, GetPlayerCamera(), Time.deltaTime * camSpeed);
#endif


	}

    Vector3 GetPlayerCamera(){
		Vector3 pos = new Vector3 (player.position.x, player.position.y + dist * (Mathf.Sqrt (3) / 2), player.position.z - dist / 2);
		return pos;
	}

    Vector3 GetCameraPos(Vector3 pos)
    {
        return new Vector3(pos.x, pos.y + dist * (Mathf.Sqrt(3) / 2), pos.z - dist / 2);
    }

    void SetPlayerCamera()
    {
        transform.position = GetPlayerCamera();
        cSkill = player.GetComponent<CharacterSkill>();

    }

    void EndGame(string defeatObject)
    {

        player.GetComponent<PlayerController>().controllable = false;
        ending = true;
        endObject = GameObject.Find(defeatObject);

    }


    void CheckDeadPlayer(GameObject deadPlayer)
    {
        if (deadPlayer == player.gameObject)
        {
            player.GetComponent<PlayerController>().controllable = false;
        }

    }

    //生き返ったのが自身のプレイヤーか？
    void CheckRespawnPlayer(GameObject respawnPlayer)
    {
        if (respawnPlayer == player.gameObject)
        {
            player.GetComponent<PlayerController>().controllable = true;
        }

    }


    public void PushSkill1()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill1);
    }

    public void PushSkill2()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill2);
    }

    public void PushSkill3()
    {
        cSkill.ButtonTrigger(NEXT_ATTACK.skill3);
    }

}
