using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorOTE : MonoBehaviour
{
    [SerializeField]
    GameObject levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject!= null && other.transform.gameObject.name.Contains("Player") && transform.parent.GetComponent<Door>().isEndDoor)
        {
            if (!SceneManager.GetActiveScene().name.Contains("Last"))
            {
                levelLoader.GetComponent<LevelLoader>().LoadLevel(2);
            }
            else
            {
                levelLoader.GetComponent<LevelLoader>().LoadLevel(0);
            }
        }
    }
}
