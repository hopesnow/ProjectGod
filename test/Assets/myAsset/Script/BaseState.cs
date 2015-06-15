using UnityEngine;
using System.Collections;

public class BaseState : ObjectState {

    protected override void Awake()
    {
        base.Awake();

        health = 250;
        max_health = health;

        width = transform.localScale.x / 2;
    }

    protected override void Update()
    {
        base.Update();


    }


    protected override int DamageCalc(int d)
    {
        int damage = (int)(d * 0.5f);
        return damage;
    }

    protected override void DeadEvent()
    {

        GameObject.Find("Photon").SendMessage("GameEndRPC", gameObject.name);
        Debug.Log("Dead BASE");

    }



}
