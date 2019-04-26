using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : MonoBehaviour
{
    [SerializeField]
    GameObject Linked;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = other.GetComponent<Renderer>().material.color;
    }

    private void ActivateLinked(GameObject Linked)
    {
        //Linked.getComponent<>()
    }
}
