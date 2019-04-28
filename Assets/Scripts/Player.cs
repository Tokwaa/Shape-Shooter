using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    //Movement + camera declarations
    internal float movementSpeed = 5.0f, mouseSensitivity = 5f, jumpSpeed = 10, verticalRotation = 0f, Yrange = 70f;
    public float verticalVelocity;
    private int powerEffectiveness = 2;
	private CharacterController characterController;

	//BallShooting declarations
    [SerializeField]
	internal GameObject Canvas, AmmoHolder;
    [SerializeField]
    internal GameObject[] AmmoTypes;
    internal GameObject[][] ammoPool; 
    private GameObject tele1, tele2, gun;
    private float fireRate = 2f, rCount = 0, rCool, lastY;
    internal static int[] Ammo;
    internal int PowerLevel = 1;
    internal bool firing = false, teleB = false;
    internal int maxAmmo = 10, defaultAmmo = 0;

	//UI variables
	internal Text PowerLvlTxt, WeaponTxt, AmmoTxt;
	internal string CurrentWeapon = "Ball";

	//Weapon switching states
	private int WeaponNumber = 1;
    private Renderer gr;

	// Use this for initialization
	void Start ()
    {
        PowerLvlTxt = Canvas.transform.GetChild(1).gameObject.GetComponent<Text>();
        WeaponTxt = Canvas.transform.GetChild(2).gameObject.GetComponent<Text>();
        AmmoTxt = Canvas.transform.GetChild(3).gameObject.GetComponent<Text>();

        gun = gameObject.transform.GetChild(0).GetChild(0).gameObject;
		Cursor.lockState = CursorLockMode.Locked;
		characterController = GetComponent<CharacterController>();
        gr = gun.gameObject.GetComponent<Renderer>();
        ammoPool = new GameObject[5][];
        Ammo = new int[]
        {
            maxAmmo, maxAmmo, maxAmmo, maxAmmo, maxAmmo
        };
        for (int i = 0; i < Ammo.Length; i++)
        {
            ammoPool[i] = new GameObject[Ammo[i]];
            for (int j = 0; j < Ammo[i] - 1; j++)
            {
                ammoPool[i][j] = Instantiate(AmmoTypes[i], transform.position, Quaternion.identity, AmmoHolder.transform);
                ammoPool[i][j].SetActive(false);
            }
        }
        tele1 = Instantiate(AmmoTypes[4], transform.position, Quaternion.identity);
        tele2 = Instantiate(AmmoTypes[4], transform.position, Quaternion.identity);
        tele1.SetActive(false);
        tele2.SetActive(false);
        Ammo = new int[]
        {
           defaultAmmo, defaultAmmo, defaultAmmo, defaultAmmo, defaultAmmo
        };
        upDateUI();
    }

    // Update is called once per frame
    void Update () 
	{
		//Looking
		transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);//player rotates along the horizontal axis for local forward vector math
		verticalRotation = Mathf.Clamp(verticalRotation - Input.GetAxis("Mouse Y") * mouseSensitivity, -Yrange, Yrange); //camera rotates along the vertical axis clamped to +-70 degrees
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        if (!characterController.isGrounded)
        {
            if (verticalVelocity > lastY)
            {
                verticalVelocity = Mathf.MoveTowards(verticalVelocity, -jumpSpeed, 8 * Time.deltaTime); //going up
            }
            else
            {
                verticalVelocity = Mathf.MoveTowards(verticalVelocity, -jumpSpeed, 24 * Time.deltaTime); //coming down

            }
        }
        else if (Input.GetKey(KeyCode.Space))
		{
			verticalVelocity = jumpSpeed;
        }
        lastY = verticalVelocity;
        Vector3 speed = new Vector3 (Input.GetAxis("Horizontal") * movementSpeed / 2, verticalVelocity, Input.GetAxis("Vertical") * movementSpeed);
		speed = transform.rotation * speed;
		characterController.Move(speed * Time.deltaTime);

        //changing weapon via the alpha numbers (1-9 not numpad)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gr.material.color = Color.red;
            WeaponNumber = 0;
            CurrentWeapon = "Ball";
            powerEffectiveness = 2; upDateUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            gr.material.color = Color.grey;
            WeaponNumber = 1;
            CurrentWeapon = "Cube";
            powerEffectiveness = 2; upDateUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gr.material.color = Color.magenta;
            WeaponNumber = 2;
            CurrentWeapon = "Sticky";
            powerEffectiveness = 2; upDateUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            gr.material.color = Color.yellow;
            WeaponNumber = 3;
            CurrentWeapon = "Laser";
            powerEffectiveness = 50; upDateUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gr.material.color = Color.blue;
            WeaponNumber = 4;
            CurrentWeapon = "Teleporter Pair";
            powerEffectiveness = 5; upDateUI();
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
            PowerLvlTxt.text = "Power Lvl: " + PowerLevel; upDateUI();
        }

        //LeftMouseInput / SHOOTING
        if (Input.GetMouseButton(0))
        {
            allowFire(AmmoTypes[WeaponNumber]);
	    }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (rCool > 0 && rCount == 1)
            {
                foreach (Transform obj in AmmoHolder.transform)
                {
                    if (obj.gameObject.activeSelf)
                    {
                        obj.transform.position = Vector3.zero;
                        obj.gameObject.SetActive(false);
                        switch (obj.tag)
                        {
                            case "Ball":
                                Ammo[0]++;
                                break;
                            case "Cube":
                                Ammo[1]++;
                                break;
                            case "StickyCube":
                                Ammo[2]++;
                                break;
                            case "Laser":
                                Ammo[3]++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                if (rCount == 0)
                {
                    RaycastHit hit;
                    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red, Mathf.Infinity);
                    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
                    {
                        Debug.Log(hit.transform.name);
                        foreach (Transform obj in AmmoHolder.transform)
                        {
                            if (hit.transform == obj)
                            {
                                obj.transform.position = Vector3.zero;
                                obj.gameObject.SetActive(false);
                                switch (obj.tag)
                                {
                                    case "Ball":
                                        Ammo[0]++;
                                        break;
                                    case "Cube":
                                        Ammo[1]++;
                                        break;
                                    case "StickyCube":
                                        Ammo[2]++;
                                        break;
                                    case "Laser":
                                        Ammo[3]++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                    }
                }
                rCool = 0.35f;
                rCount++;
            }
            upDateUI();
        }
        if (rCool > 0)
        {
            rCool -= Time.deltaTime;
        }
        else
        {
            rCount = 0;
        }
    }

    private IEnumerator Fire(GameObject ammoType)
    {
        firing = true;
        if (ammoType == AmmoTypes[4])
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject == tele1 && tele2.activeSelf && Vector3.Distance(transform.position, hit.transform.position) <= 1)
                {
                    transform.position = tele2.transform.position;
                    Ammo[4]--;
                    upDateUI();
                }
                else if (hit.transform.gameObject == tele2 && tele1.activeSelf && Vector3.Distance(transform.position, hit.transform.position) <= 1)
                {
                    transform.position = tele1.transform.position;
                    Ammo[4]--;
                    upDateUI();
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
            foreach (GameObject p in ammoPool[WeaponNumber])
            {
                if (!p.activeSelf)
                {
                    p.SetActive(true);
                    p.transform.position = gun.transform.position + gun.transform.up;
                    p.GetComponent<Rigidbody>().velocity = (p.transform.position - gun.transform.position).normalized * (PowerLevel * powerEffectiveness);
                    if (p.tag.Contains("Laser"))
                    {
                        StartCoroutine(Laser(p));
                    }
                    break;
                }
            }
        }
        yield return new WaitForSeconds(fireRate);
        firing = false;
    }
    private IEnumerator Laser(GameObject l)
    {
        yield return new WaitForSeconds(10);
        l.transform.position = Vector3.zero;
        l.SetActive(false);
        Ammo[3]++;
    }

    private void allowFire(GameObject ammo)
    {
        if ((Ammo[WeaponNumber] > 0 || WeaponNumber == 4) && firing == false)
        {
            Ammo[WeaponNumber]--;
            upDateUI(); StartCoroutine(Fire(ammo));
        }
    }

    internal void upDateUI()
    {
        WeaponTxt.text = "Weapon Selected: " + WeaponNumber + " " + CurrentWeapon;
        PowerLvlTxt.text = "Power Lvl: " + PowerLevel;
        AmmoTxt.text = "Ammo: " + Ammo[0] + " " + Ammo[1] + " " + Ammo[2] + " " + Ammo[3] + " " + Ammo[4];
    }
}