using UnityEngine;
using System.Collections;

public class BaseState : ObjectState {

    protected override void Awake()
    {
        base.Awake();

        width = transform.localScale.x / 2;
    }

    void Damage(int d)
    {
        int damage = (int)(d * 0.5f);
        if (health > damage)
        {
            health -= damage;
        }
        else
        {
            //dead

        }
    }



}
