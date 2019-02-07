using UnityEngine;
using System.Collections;

public class Bulllet : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
	{

		Destroy(gameObject);
	}
}
