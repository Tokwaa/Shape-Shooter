using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject[] Inputs;
    private bool[] Sources;
    bool isPowered = false;
    bool isEndDoor = false;

    private void Start()
    {
        Sources = new bool[Inputs.Length];
        for (int i = 0; i < Sources.Length; i++)
        {
            Sources[i] = false;
        }
    }

    private void updateSource(GameObject Source, bool active)
    {
        for (int i = 0; i < gameObject.GetComponent<Door>().Inputs.Length; i++)
        {
            if (Source == Inputs[i])
            {
                Sources[i] = active;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && isEndDoor)
        {

        }
    }
}
