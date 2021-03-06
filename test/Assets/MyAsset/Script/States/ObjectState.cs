using UnityEngine;
using System.Collections;

public enum TEAM
{
    BLUE,
    RED,
    NEUTRAL,
}

public class ObjectState : Photon.MonoBehaviour {

    protected int health, max_health, attack, protect, attack_speed;
    protected float range, width;

    protected PhotonView myPhotonView;

    public TEAM team;

    protected virtual void Awake()
    {
        health = 100;
        max_health = 100;
        attack = 15;
        protect = 5;
        attack_speed = 100;
        range = 1.00f;
        width = 0.0f;
        //melee 1.5くら？ range 5.00くらい

        myPhotonView = gameObject.GetComponent<PhotonView>();
        
    }

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}


    public float RANGE { get { return range; } }
    public float WIDTH { get { return width; } }
    public float ATTACK { get { return attack; } }
    /// <summary>
    /// attack_speed
    /// </summary>
    public int AS { get { return attack_speed; } }
    public float NEXT_ATTACK { get { return 100.0f / (float)AS; } }

    public float HEALTH_RATE
    {
        get { return (float)health / (float)max_health; }
    }

    void SetMaxHealth(int h)
    {
        max_health = h;
    }

    protected void DamageAttack(int d)
    {
        int damage = (d - protect);
        if (damage < 1) damage = 1;
        myPhotonView.RPC("Damage", PhotonTargets.All, damage);

    }

    void SendTeam()
    {
        myPhotonView.RPC("SetTeam", PhotonTargets.All, (int)team);
    }

    [PunRPC]
    protected void SetTeam(int t)
    {
        team = (TEAM)t;
        if (GetComponent<PlayerController>() != null)
        {
            GetComponent<PlayerController>().SendMessage("SetCanvasTeam");
        }
    }

    [PunRPC]
    protected void Damage(int d)
    {
        int damage = DamageCalc(d);
        if (health > damage)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            DeadEvent();
            //dead
        }

    }

    protected virtual int DamageCalc(int d)
    {
        return d;
    }

    protected virtual void DeadEvent(){

    }


}
