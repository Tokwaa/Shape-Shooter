using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    private Vector3 StartPos;
    private bool active = false, lA = false;
    private Transform ct;
    [SerializeField]
    GameObject Linked;
    GameObject hitMe;

    private void Start()
    {
        ct = transform.GetChild(0);
        StartPos = ct.position;
        GetComponent<Renderer>().material.color = Color.blue;

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
        if (!lA)
        {
            active = false;
            GetComponent<Renderer>().material.color = Color.blue;
            Linked.GetComponent<Door>().updateSource(gameObject, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        hitMe = other.gameObject;
        if (hitMe.tag.Contains("Laser"))
        {
            StartCoroutine(LaserActivated());
        }
        active = true;
        GetComponent<Renderer>().material.color = other.gameObject.GetComponent<Renderer>().material.color;
        if (Linked.name.Contains("Door"))
        {
            Linked.GetComponent<Door>().updateSource(gameObject, true);
        }
    }
    private IEnumerator LaserActivated()
    {
        lA = true;
        yield return new WaitForSeconds(10);
        active = false;
        GetComponent<Renderer>().material.color = Color.blue;
        Linked.GetComponent<Door>().updateSource(gameObject, false);
        lA = false;

    }
}
