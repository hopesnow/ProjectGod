﻿using UnityEngine;
using System.Collections;

public class CanvasSetter : MonoBehaviour {
    public Transform target;
    public RectTransform copy;
    public bool health;
    float maxHealthWidth;

	// Use this for initialization
	void Start () {
        maxHealthWidth = GetComponent<RectTransform>().rect.width;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.localScale == Vector3.zero) return;

        if (copy != null)
        {
            GetComponent<RectTransform>().position = copy.position + new Vector3((copy.rect.width - GetComponent<RectTransform>().rect.width) / 2, 0, 0);
        }
        else if (target != null)
        {
            GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(target.position) + new Vector3(-GetComponent<RectTransform>().rect.width / 2, 40, 0);

        }

        if (health)
        {
            //GetComponent<RectTransform>().rect.Set(GetComponent<RectTransform>().rect.left, GetComponent<RectTransform>().rect.top, target.GetComponent<ObjectState>().HEALTH_RATE * maxHealthWidth, GetComponent<RectTransform>().rect.height);
            GetComponent<RectTransform>().localScale = new Vector3(target.GetComponent<ObjectState>().HEALTH_RATE, 1, 1);
        }


	}

    void SetTarget(Transform t)
    {
        target = t;
    }

    void SetCopy(RectTransform t)
    {
        copy = t;
    }

}