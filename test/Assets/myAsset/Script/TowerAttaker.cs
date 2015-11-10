using UnityEngine;
using System.Collections;

public class TowerAttaker : MonoBehaviour {

    GameObject target;
    float speed;
    /// <summary>
    /// 攻撃力
    /// </summary>
    float attack;
    public bool master;

    Vector3 targetOffset;

    void Awake()
    {
        speed = 0.2f;
        master = false;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (!master) return;
        if (target == null)
        {
            GetComponent<PhotonView>().RPC("attacked", PhotonTargets.All);
            master = false;
            return;
        }

        Vector3 vec = ((target.transform.position + targetOffset) - transform.position).normalized;
        transform.position = transform.position + vec * speed;
        if ((transform.position - (targetOffset + target.transform.position)).magnitude < speed)
        {
            //攻撃成功
            target.SendMessage("DamageAttack", attack);
            GetComponent<PhotonView>().RPC("attacked", PhotonTargets.All);
            master = false;
        }
	
	}

    public void Spawn(GameObject t, float atk)
    {
        target = t;
        attack = atk;

        targetOffset = new Vector3(0, target.GetComponent<CapsuleCollider>().height / 2, 0);

    }

    [PunRPC]
    void attacked()
    {
        Destroy(gameObject);
    }



}
