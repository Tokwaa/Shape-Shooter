using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour {

	public float Distance =5.0f;
	public float fireRate =0.1f;
	public float shootSpeed =0.1f;
	public GameObject Player;
	public GameObject gunBarrel;
	public GameObject bullet;
	public bool allowFire = true;
	public bool playerInDistance=false;
	public RaycastHit hit;
	public GameObject Target;
	public GameObject Ball;
	public GameObject PTarget;
	public List<GameObject> Targets;
	public List<GameObject> Balls;
	public GameObject closest=null;

	public bool TurretTypePlayer=true;


    private void Start()
    {
        Player = GameObject.Find("Player");
        PTarget = Player;
    }

    void Update()
	{

		//cubes = GameObject.FindGameObjectsWithTag("StickyCube");
		//if player in range of turret
		if(Vector3.Distance(gameObject.transform.position,Player.transform.position)< Distance&&TurretTypePlayer==true)
		{
			playerInDistance=true;

			

			
		}
		//if player is not in range of turret
		else if(Vector3.Distance(gameObject.transform.position,Player.transform.position)> Distance)playerInDistance=false;

		//if playerinDistance is true
		if(playerInDistance)
		{
			//Turret looks at player
			transform.LookAt(Player.transform.position);
		//raycast direction of player
		Vector3 dir = Player.transform.position-transform.position;
			//Raycast towards player
		if(Physics.Raycast(transform.position,dir,out hit))
			
		{
				//if it sees the player 
				if(hit.collider.gameObject.tag=="Player")
				{
			    //if not shooting start shooting depending on turrets fireRate
			    if(allowFire) StartCoroutine(Shoot());
				}
			//}
		}
		}
		//if object turret
		if(TurretTypePlayer==false)

		{
			if(Target!=null)Target.GetComponent<Renderer>().material.color=Color.yellow;
			{
				PTarget=GameObject.FindGameObjectWithTag("Ball");
				if(Vector3.Distance(gameObject.transform.position,PTarget.transform.position)< Distance)
				{
					if(Target==null)
					{
						Target=PTarget;
					}
				}
				else PTarget=null;

			}
			
//			List<GameObject>Targets=new List<GameObject>();
//			List<GameObject>Balls=new List<GameObject>();
//			//Balls.AddRange(GameObject.FindGameObjectsWithTag("Ball"));
//			Balls.Add(GameObject.FindGameObjectWithTag("Ball"));
//			
//			foreach (GameObject Ball in Balls)
//			{
//				if(Vector3.Distance(gameObject.transform.position,Ball.transform.position)< Distance)
//				{
////					Targets.Add( );
////					Targets.Add(Ball);
//
//					//Target = Targets[1];
//				}
//				//Target.renderer.material.color = Color.yellow;
		}






//			
//	
//		}
	}



	IEnumerator Shoot()
	{
		allowFire=false;
		GameObject bulletClone = Instantiate(bullet,gunBarrel.transform.position,bullet.transform.rotation)as GameObject;
		//bulletClone.transform.LookAt(Input.mousePosition);
		Vector3 fwd =bulletClone.transform.forward;
		Vector3 newDirection = gunBarrel.transform.position - Player.transform.position ;
		//bulletClone.rigidbody.AddForce(newDirection*shootSpeed);
		bulletClone.GetComponent<Rigidbody>().AddForce(transform.forward*shootSpeed);

		print("BulletShot");
		yield return new WaitForSeconds (fireRate);
		allowFire = true;
	}







}
