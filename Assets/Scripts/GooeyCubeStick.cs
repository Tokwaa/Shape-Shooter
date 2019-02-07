using UnityEngine;
using System.Collections;

public class GooeyCubeStick : MonoBehaviour {



	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag=="StickyCube")
		{
			collision.gameObject.GetComponent<Rigidbody>().isKinematic=true;
			collision.gameObject.GetComponent<Rigidbody>().useGravity=false;

		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
