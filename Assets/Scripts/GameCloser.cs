using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCloser : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("Has Quit Game");
        Application.Quit();
    }
}