using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receptacle : MonoBehaviour
{
    [SerializeField]
    GameObject Linked;

    private void OnTriggerExit(Collider other)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = Color.blue;
        }
        Linked.GetComponent<Door>().updateSource(gameObject, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in GetComponentsInChildren<Renderer>())
        {
            item.material.color = other.gameObject.GetComponent<Renderer>().material.color;
        }
        if (Linked.name.Contains("Door"))
        {
            Linked.GetComponent<Door>().updateSource(gameObject, true);
        }
    }
}
