using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;
    }
}
