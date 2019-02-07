using UnityEngine;
using System.Collections;

public class CollisionChanger : MonoBehaviour {



	void OnCollisionEnter(Collision collision)
	{

		gameObject.layer=9;
	}
}
