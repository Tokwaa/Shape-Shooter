using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	
	
	//Movement + camera declarations
	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	public float jumpSpeed = 2.0f;
	float verticalRotation=0;
	public float Yrange =70f;
	float verticalVelocity =0;
	CharacterController characterController;

	//BallShooting declarations
	public GameObject Canvas;
	public GameObject ballGameobject;
	public GameObject cubeGameobject;
	public GameObject bouncyGameObject;
	public  GameObject Target;
	public GameObject TargetAid;
	public GameObject gun;
	public float shootSpeed;
	public float fireRate;
	public bool allowFire = true;
	public bool limiter=true;
	public GameObject[] balls;
	public GameObject[] cubes; 
	public int Energy = 100;
	public int PhysicsObjects;
	public int PowerLevel=1;
	public int RealPowerLevel=1;

	//UI variables
	public Text EnergyText;
	public Text PowerLvlTXT;
	public Text WeaponNumberText;
	public Text PhysicsObjText;
	public string CurrentWeapon = "BallShooter";

	//Weapon switching states
	public int WeaponNumber = 1;

	// Use this for initialization
	void Start () {
		Canvas.gameObject.SetActive(true);
		gun.gameObject.GetComponent<Renderer>().material.color= Color.red;
		Cursor.lockState = CursorLockMode.Locked;
		characterController = this.GetComponent<CharacterController>();
		EnergyText.text = "Energy: "+Energy;
		WeaponNumber = 1;
		WeaponNumberText.text="Weapon Selected: "+WeaponNumber+" "+CurrentWeapon;
		PowerLevel=1;
		PowerLvlTXT.text = "Power Lvl: "+PowerLevel;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Looking
		float rotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotation,0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -Yrange,Yrange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation,0,0);
		
		// Movement
		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
		
		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		
		if(characterController.isGrounded && Input.GetKey(KeyCode.Space))
		{
			verticalVelocity = jumpSpeed;
		}
		
		Vector3 speed = new Vector3 (sideSpeed,verticalVelocity, forwardSpeed);
		speed = transform.rotation*speed;
		characterController.Move(speed*Time.deltaTime);
		//changing weapon via the alpha numbers
		if(Input.GetKeyDown(KeyCode.Alpha1))NormalBalls();

		if(Input.GetKeyDown(KeyCode.Alpha2))GooeyCubes();

		if(Input.GetKeyDown(KeyCode.Alpha3))BouncyBalls();

		//Changing power setting
		if(Input.GetAxis("Mouse ScrollWheel")>0)
		{

			PowerLevel+=1;
			PowerLevel=Mathf.Clamp(PowerLevel,1,10);
			PowerLvlTXT.text = "Power Lvl: "+PowerLevel;



		}
		if(Input.GetAxis("Mouse ScrollWheel")<0)
		{

			PowerLevel-=1;
			PowerLevel=Mathf.Clamp(PowerLevel,1,10);
			PowerLvlTXT.text = "Power Lvl: "+PowerLevel;
			
		}



		

		
			


		//LeftMouseInput / SHOOTING
				if(Input.GetMouseButton(0))
				{
            Cursor.lockState = CursorLockMode.Locked;
            if (Energy>0||limiter==false)
			{
					if(allowFire)
				{
					if(WeaponNumber==1)StartCoroutine(FireBall());
					if(WeaponNumber==2)StartCoroutine(FireCube());
					if(WeaponNumber==3)StartCoroutine(FireBouncyBall());
					
				}
//					if(allowFire)StartCoroutine(FireGun());
			}
					print("LMB pressed");
					
					
					
					
					

					//Ray ray = Camera.main.ViewportPointToRay (new Vector3(0.5f,0.5f,0));
					//RaycastHit hit;
				}

		//RightMouse INPUT
				if(Input.GetMouseButton(1))
		{
            Cursor.lockState = CursorLockMode.Locked;
            print ("rightMouseHit");
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider)
				{
					print ("HIT"+hit.transform.name);
					if(hit.collider.gameObject.tag=="Ball"||hit.collider.gameObject.tag=="StickyCube")
				{
					print ("ballHit");
					Energy+=10;
					EnergyText.text = "Energy: "+Energy;
						PhysicsObjects--;
						PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;

					Destroy(hit.collider.gameObject);
				}
				}
			}
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			balls = GameObject.FindGameObjectsWithTag("Ball");
			for(int i = 0; i < balls.Length; i++)
			{
				Destroy(balls[i]);
				Energy+=10;
				EnergyText.text = "Energy: "+Energy;
				PhysicsObjects--;
				PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;
			}
			cubes = GameObject.FindGameObjectsWithTag("StickyCube");
			for(int i = 0; i < cubes.Length; i++)
			{
				Destroy(cubes[i]);
				Energy+=10;
				EnergyText.text = "Energy: "+Energy;
				PhysicsObjects--;
				PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;

			}
		}

	}
	//Selected Weapons

	void NormalBalls()
	{
		gun.gameObject.GetComponent<Renderer>().material.color= Color.red;
		WeaponNumber=1;
		CurrentWeapon="BallShooter";
		WeaponNumberText.text="Weapon Selected: "+WeaponNumber+" "+CurrentWeapon;
	}
	void GooeyCubes()
	{
		gun.gameObject.GetComponent<Renderer>().material.color= Color.magenta;
		WeaponNumber=2;
		CurrentWeapon="Gooey CUBES";
		WeaponNumberText.text="Weapon Selected: "+WeaponNumber+" "+CurrentWeapon;

	}
	void BouncyBalls()
	{
		gun.gameObject.GetComponent<Renderer>().material.color= Color.yellow;
		WeaponNumber=3;
		CurrentWeapon="BouncyBalls";
		WeaponNumberText.text="Weapon Selected: "+WeaponNumber+" "+CurrentWeapon;

	}



	IEnumerator FireBall()
	{
		allowFire=false;
		Energy-=10;
		EnergyText.text = "Energy: "+Energy;
		GameObject ballClone = Instantiate(ballGameobject,Target.transform.position,ballGameobject.transform.rotation)as GameObject;
		ballClone.transform.LookAt(Input.mousePosition);
		Vector3 newDirection = TargetAid.transform.position - Target.transform.position ;
		ballClone.GetComponent<Rigidbody>().AddForce(newDirection*shootSpeed*PowerLevel/10);
		PhysicsObjects++;
		PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;
		print("ballShot pressed");

		yield return new WaitForSeconds (fireRate);
		allowFire = true;
	}
	IEnumerator FireCube()
	{
		allowFire=false;
		Energy-=10;
		EnergyText.text = "Energy: "+Energy;
		GameObject cubeClone = Instantiate(cubeGameobject,Target.transform.position,cubeGameobject.transform.rotation)as GameObject;
		cubeClone.transform.LookAt(Input.mousePosition);
		Vector3 newDirection = TargetAid.transform.position - Target.transform.position ;
		cubeClone.GetComponent<Rigidbody>().AddForce(newDirection*shootSpeed);
		cubeClone.transform.rotation=Quaternion.identity;
		PhysicsObjects++;
		PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;
		print("cubeShot pressed");
		
		yield return new WaitForSeconds (fireRate);
		allowFire = true;
	}
	IEnumerator FireBouncyBall()
	{
		allowFire=false;
		Energy-=10;
		EnergyText.text = "Energy: "+Energy;
		GameObject bouncyBallClone = Instantiate(bouncyGameObject,Target.transform.position,bouncyGameObject.transform.rotation)as GameObject;
		bouncyBallClone.transform.LookAt(Input.mousePosition);
		Vector3 newDirection = TargetAid.transform.position - Target.transform.position ;
		bouncyBallClone.GetComponent<Rigidbody>().AddForce(newDirection*shootSpeed);
		PhysicsObjects++;
		PhysicsObjText.text = "PhysicsObjects: "+PhysicsObjects;
		print("ballShot pressed");
		
		yield return new WaitForSeconds (fireRate);
		allowFire = true;
	}
}