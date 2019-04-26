using UnityEngine;
using System.Collections;

public class GooeyCubeStick : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic=true;
        rb.useGravity=false;
	}

}
