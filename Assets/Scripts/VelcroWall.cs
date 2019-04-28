 using UnityEngine;
using System.Collections;

public class VelcroWall : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{
        var rb = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.tag.Contains("StickyCube"))
		{
		   rb.isKinematic=true;
           rb.useGravity=false;
		}
	}
}
