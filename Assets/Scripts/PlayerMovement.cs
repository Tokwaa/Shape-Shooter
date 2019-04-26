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
	internal GameObject Canvas, ballProj, stickyProj, laserProj, TeleportProj, cubeProj, gun;
    private GameObject LastFire, tele1, tele2;
    private float fireRate = 2f;
    private float[] Ammo;
	internal int PhysicsObjects;
    internal int PowerLevel = 1;
    internal bool firing = false, teleB = false;


	//UI variables
    [SerializeField]
	internal Text PowerLvlTxt, WeaponTxt, AmmoTxt;
	internal string CurrentWeapon = "BallShooter";

	//Weapon switching states
	private int WeaponNumber = 1;
    private Renderer gr;

	// Use this for initialization
	void Start ()
    {
		Cursor.lockState = CursorLockMode.Locked;
		characterController = GetComponent<CharacterController>();
        WeaponTxt.text = "Weapon Selected: " + WeaponNumber + " " + CurrentWeapon;
        PowerLvlTxt.text = "Power Lvl: " + PowerLevel;
        gr = gun.gameObject.GetComponent<Renderer>();
        Ammo = new float[]
        {
            10, 10, 10, 10, Mathf.Infinity
        }; AmmoTxt.text = "Ammo: " + Ammo[0] + " " + Ammo[1] + " " + Ammo[2] + " " + Ammo[3];
        tele1 = Instantiate(TeleportProj, transform.position, Quaternion.identity);
        tele2 = Instantiate(TeleportProj, transform.position, Quaternion.identity);
        tele1.SetActive(false);
        tele2.SetActive(false);
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
            gr.material.color = Color.grey;
            WeaponNumber = 1;
            CurrentWeapon = "Ball";
            powerEffectiveness = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gr.material.color = Color.magenta;
            WeaponNumber = 2;
            CurrentWeapon = "Cube";
            powerEffectiveness = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gr.material.color = Color.red;
            WeaponNumber = 3;
            CurrentWeapon = "Sticky";
            powerEffectiveness = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gr.material.color = Color.blue;
            WeaponNumber = 4;
            CurrentWeapon = "Laser";
            powerEffectiveness = 50;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gr.material.color = Color.grey;
            WeaponNumber = 5;
            CurrentWeapon = "Teleporter Pair";
            powerEffectiveness = 5;
        }
        if (WeaponTxt.text != "Weapon Selected: " + WeaponNumber + ", " + CurrentWeapon)
        {
            WeaponTxt.text = "Weapon Selected: " + WeaponNumber + ", " + CurrentWeapon;
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
            PowerLevel = Mathf.Clamp(PowerLevel, 1, 15);
            PowerLvlTxt.text = "Power Lvl: " + PowerLevel;
        }

        //LeftMouseInput / SHOOTING
        if (Input.GetMouseButton(0))
        {
            if (allowFire())
			{
                switch (WeaponNumber)
                {
                    case 1:
                        StartCoroutine(Fire(ballProj));
                        break;
                    case 2:
                        StartCoroutine(Fire(cubeProj));
                        break;
                    case 3:
                        StartCoroutine(Fire(stickyProj));
                        break;
                    case 4:
                        StartCoroutine(Fire(laserProj));
                        break;
                    case 5:
                        StartCoroutine(Fire(TeleportProj));
                        break;
                    default:
                        break;
                }
			}
	    }
	    if(Input.GetMouseButton(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hit))
			{
				if(hit.collider)
				{
				    if(hit.collider.gameObject.tag.Contains("Ball") || hit.collider.gameObject.tag.Contains("StickyCube"))
				    {
                        Destroy(hit.collider.gameObject);
				    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            var balls = GameObject.FindGameObjectsWithTag("Ball");
            var cubes = GameObject.FindGameObjectsWithTag("StickyCube");
            var lasers = GameObject.FindGameObjectsWithTag("Laser");
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
        }

    }
    private IEnumerator Fire(GameObject ammoType)
    {
        firing = true;
        if (ammoType == TeleportProj)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == tele1 && tele2.activeSelf && Vector3.Distance(transform.position, hit.transform.position) <= 1)
                {
                    transform.position = tele2.transform.position;
                }
                else if (hit.transform.gameObject == tele2 && tele1.activeSelf && Vector3.Distance(transform.position, hit.transform.position) <= 1)
                {
                    transform.position = tele1.transform.position;
                }
                else if (teleB == false)
                {
                    tele1.GetComponent<Rigidbody>().isKinematic = false;
                    tele1.transform.position = hit.point;
                    if (!tele1.activeSelf)
                    {
                        tele1.SetActive(true);
                    }
                    teleB = true;
                }
                else
                {
                    tele2.GetComponent<Rigidbody>().isKinematic = false;
                    tele2.transform.position = hit.point;
                    if (!tele2.activeSelf)
                    {
                        tele2.SetActive(true);
                    }
                    teleB = false;

                }
            }
        }
        else
        {
            GameObject ammoClone = Instantiate(ammoType, gun.transform.position + gun.transform.up, Quaternion.identity);
            ammoClone.GetComponent<Rigidbody>().velocity = (ammoClone.transform.position - gun.transform.position).normalized * (PowerLevel * powerEffectiveness);
            LastFire = ammoClone;
        }
        yield return new WaitForSeconds(fireRate);
        firing = false;
    }
    private bool allowFire()
    {
        if (Ammo[WeaponNumber-1] > 0 && firing == false)
        {
            Ammo[WeaponNumber-1]--;
            AmmoTxt.text = "Ammo: " + Ammo[0] + " " + Ammo[1] + " " + Ammo[2] + " " + Ammo[3];
            return true;
        }
        else
        {
            return false;
        }
    }
}