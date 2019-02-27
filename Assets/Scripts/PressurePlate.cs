﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    private Vector3 StartPos;
    private bool active = false;
    private Transform ct;
    private void Start()
    {
        ct = transform.GetChild(0);
        StartPos = ct.position;
    }
    private void Update()
    {
        if (!active && ct.position.y < StartPos.y) //not triggered and plate not up move up
        {
            ct.position = new Vector3(ct.position.x, Mathf.Lerp(ct.position.y, StartPos.y, 1), ct.position.z);
        }
        if(active && ct.position.y > StartPos.y - gameObject.GetComponents<BoxCollider>()[1].bounds.extents.y) //triggered and plate not down move down
        {
            ct.position = new Vector3(ct.position.x, Mathf.Lerp(ct.position.y, StartPos.y - gameObject.GetComponents<BoxCollider>()[1].bounds.extents.y, 1), ct.position.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        active = true;
    }
}