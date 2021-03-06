﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject[] Inputs;
    private bool[] Sources;
    [SerializeField]
    internal bool isEndDoor = false;


    private void Start()
    {
        if (Inputs.Length > 0)
        {
            Sources = new bool[Inputs.Length];
            for (int i = 0; i < Sources.Length; i++)
            {
                Sources[i] = false;
            }
        }
        else
        {
            isPowered();
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
                isPowered(false);
                break;
            }
            else
            {
                isPowered();
            }
        }
    }

    private void isPowered(bool yes = true)
    {
        if (yes)
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            transform.GetChild(0).GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
