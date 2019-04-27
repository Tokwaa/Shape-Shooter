using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    int[] value;
    [SerializeField]
    GameObject player;
    bool used = false;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(Random.value*360, Random.value * 360, Random.value * 360));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name.Contains("Player") && !used)
        {
            used = true;
            for (int i = 0; i < PlayerMovement.Ammo.Length; i++)
            {
                PlayerMovement.Ammo[i] += value[i];
            }
            Destroy(gameObject);
            player.GetComponent<PlayerMovement>().upDateUI();
        }
    }
}
