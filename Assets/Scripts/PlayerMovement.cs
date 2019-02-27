using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	//Movement + camera declarations
    [SerializeField]
	internal float movementSpeed = 5.0f, mouseSensitivity = 5f, jumpSpeed = 2f, verticalRotation = 0f, Yrange = 70f, verticalVelocity;
    private int powerEffectiveness = 2;
	private CharacterController characterController;

	//BallShooting declarations
    [SerializeField]
	internal GameObject Canvas, ballGameobject, cubeGameobject, bouncyGameObject, gun;
    private GameObject LastFire;
    private float fireRate = 2f;
	private bool allowFire = true;
	internal int PhysicsObjects;
    internal int PowerLevel = 1;

	//UI variables
    [SerializeField]
	internal Text PowerLvlTXT, WeaponNumberText, PhysicsObjText;
	internal string CurrentWeapon = "BallShooter";

	//Weapon switching states
	private int WeaponNumber = 1;
    private Renderer gr;


	// Use this for initialization
	void Start ()
    {
		Cursor.lockState = CursorLockMode.Locked;
		characterController = GetComponent<CharacterController>();
		WeaponNumberText.text = "Weapon Selected: " + WeaponNumber + " " + CurrentWeapon;
		PowerLvlTXT.text = "Power Lvl: " + PowerLevel;
        gr = gun.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () 
	{
		//Looking
		transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);//player rotates along the horizontal axis for local forward vector math
		verticalRotation = Mathf.Clamp(verticalRotation - Input.GetAxis("Mouse Y") * mouseSensitivity, -Yrange, Yrange); //camera rotates along the vertical axis clamped to +-70 degrees
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // Movement
        if (!characterController.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
		else if(Input.GetKey(KeyCode.Space))
		{
			verticalVelocity = jumpSpeed;
		}
		
		Vector3 speed = new Vector3 (Input.GetAxis("Horizontal") * movementSpeed / 2, verticalVelocity, Input.GetAxis("Vertical") * movementSpeed);
		speed = transform.rotation * speed;
		characterController.Move(speed * Time.deltaTime);

        //changing weapon via the alpha numbers (1-9 not numpad)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gr.material.color = Color.red;
            WeaponNumber = 1;
            CurrentWeapon = "BallShooter";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gr.material.color = Color.magenta;
            WeaponNumber = 2;
            CurrentWeapon = "GooeyCubes";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gr.material.color = Color.yellow;
            WeaponNumber = 3;
            CurrentWeapon = "BouncyBalls";
        }
        if (WeaponNumberText.text != "Weapon Selected: " + WeaponNumber + ", " + CurrentWeapon)
        {
            WeaponNumberText.text = "Weapon Selected: " + WeaponNumber + ", " + CurrentWeapon;
        }

        //Changing power setting
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0) //scrolling up
            {
                PowerLevel++;
            }
            else
            {
                PowerLevel--;
            }
            PowerLevel = Mathf.Clamp(PowerLevel, 0, 15);
            PowerLvlTXT.text = "Power Lvl: " + PowerLevel;
            //Debug.Log(PowerLevel);
        }

        //LeftMouseInput / SHOOTING
        if (Input.GetMouseButton(0))
        {
            if (allowFire)
			{
                switch (WeaponNumber)
                {
                    case 1:
                        StartCoroutine(Fire(ballGameobject));
                        break;
                    case 2:
                        StartCoroutine(Fire(cubeGameobject));
                        break;
                    case 3:
                        StartCoroutine(Fire(bouncyGameObject));
                        break;
                    default:
                        Debug.Log("left mouse shoot fell through?");
                        break;
                }
			}
            //Debug.Log("LMB pressed");
	    }

		//RightMouse INPUT
	    if(Input.GetMouseButton(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hit))
			{
				if(hit.collider)
				{
					//Debug.Log("Raycast Hit: " + hit.transform.name);
				    if(hit.collider.gameObject.tag.Contains("Ball") || hit.collider.gameObject.tag.Contains("StickyCube"))
				    {
					    PhysicsObjects--;
					    PhysicsObjText.text = "Physics Objects: " + PhysicsObjects;
                        Destroy(hit.collider.gameObject);
				    }
                }
                /*else
                {
                    Debug.Log("Raycst missed");
                }*/
            }
            //Debug.Log("RMB pressed");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            var cubes = GameObject.FindGameObjectsWithTag("StickyCube");
            foreach (var ball in balls)
            {
                if (ball != LastFire || balls.Length + cubes.Length == 1)
                {
                    Destroy(ball);
                }
            }
            foreach (var cube in cubes)
            {
                if (cube != LastFire || balls.Length + cubes.Length == 1)
                {
                    Destroy(cube);
                }
            }
            PhysicsObjText.text = "Physics Objects: " + PhysicsObjects;
        }

    }
    private IEnumerator Fire(GameObject ammoType)
    {
        //Debug.Log("Fired: " + ammoType)
        allowFire = false;
        GameObject ammoClone = Instantiate(ammoType, gun.transform.position + gun.transform.up , Quaternion.identity);
        PhysicsObjects++;
        PhysicsObjText.text = "Physics Objects: " + PhysicsObjects;
        ammoClone.GetComponent<Rigidbody>().velocity = (ammoClone.transform.position - gun.transform.position).normalized * (PowerLevel * powerEffectiveness);
        Debug.Log(ammoClone.GetComponent<Rigidbody>().velocity);
        LastFire = ammoClone;
        yield return new WaitForSeconds(fireRate);
        allowFire = true;
    }
}