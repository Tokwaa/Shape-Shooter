using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject[] Inputs;
    private bool[] Sources;
    [SerializeField]
    private bool isEndDoor = false;

    private void Start()
    {
        Sources = new bool[Inputs.Length];
        for (int i = 0; i < Sources.Length; i++)
        {
            Sources[i] = false;
        }
    }

    internal void updateSource(GameObject Source, bool active)
    {
        for (int i = 0; i < gameObject.GetComponent<Door>().Inputs.Length; i++)
        {
            if (Source == Inputs[i])
            {
                Sources[i] = active;
            }
        }
        foreach (bool b in Sources)
        {
            if (!b)
            {
                break;
            }
            else
            {
                isPowered(true);
            }
        }
    }

    private void isPowered(bool yes)
    {
        if (yes)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && isEndDoor)
        {
            //GO BACK TO MENU
        }
    }
}
