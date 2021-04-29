using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Cinemachine;

public class PlayerC : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    public CharacterController controller;
    Vector3 moveDirection = Vector3.zero;
    public Inventory inventory;
    public Vector3 oldPosition;
    public Animator animator;
    public bool isSprinting;
    public float speedFB, speedLR = 0.0F;
    public float health = 100;
    public bool pistol = false;
    public bool rifle = false;
    public string overItem;
    public Text actionText;
    public bool itemInHand = false;
    public Image healthBar;
    public bool rolling = false;
    public int inCutscene = 0;
    public int adjustCamera = 0;
    public bool inSlowMotion = false;
    public string rollingSide;
    public GameObject crossHair;
    public float currentSpeed;
    public Transform playerCam, character, centerPoint;
    private float mouseX, mouseY;
    public float mouseSensitivity = 10f;
    public float mouseYPosition = 1f;
    public float moveFB, moveLR;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public bool jumping = false;
    public bool inCombat = false;
    public float jumpAmount = 0.0F;
    public bool crouching = false;
    public bool compass = false;
    public bool inCover = false;
    public bool aiming = false;
    public bool onBorder = false;
    public GameObject endScreen;
    public float speedLimit = 1;
    public Vignette vignet;
    public Vector3 targetDirection;
    public Quaternion freeRotation;
    public float turnSpeed = 10f;
    private float turnSpeedMultiplier;
    public GameObject loadingScreen;
    public GameObject healthVignette;
    public GameObject mainC;
    public GameObject cmCam;
    public float aimingDistance;

    // Use this for initialization
    void Awake()
    {
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inventory = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        health = 100;
        pistol = false;
        rifle = false;
        actionText = GameObject.Find("ActionText").GetComponent<Text>();
        itemInHand = false;
        //healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        rolling = false;
        adjustCamera = 0;
        inCutscene = 0;
        inSlowMotion = false;
        crossHair = GameObject.Find("Crosshair");
        crossHair.SetActive(false);
        jumping = false;
        inCombat = false;
        crouching = false;
        compass = false;
        inCover = false;
        aiming = false;
        onBorder = false;
        speedLimit = 1;
        cmCam = GameObject.Find("CM_ThirdCamera");
        foreach (Rigidbody rg in GetComponentsInChildren<Rigidbody>())
        {
            rg.isKinematic = true;
        }
        /*GameObject.Find("LoadForItem").SetActive(false);
		GameObject.Find("GameManager").GetComponent<LocalizationManager>().LoadLocalizedText("en.json");
		GameObject.Find("Teste").GetComponent<LocalizedText>().key = "Eu tenho";
		////Debug.Log(GameObject.Find("Teste").GetComponent<LocalizedText>().key);*/
        healthVignette = GameObject.Find("HealthVignette");
        mainC = GameObject.Find("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Color alpha = healthVignette.GetComponent<Image>().color;
        alpha.a = (health - 100) / -100;
        healthVignette.GetComponent<Image>().color = alpha;
        if (health <= 0 && animator.enabled)
        {
            animator.enabled = false;
            foreach (Rigidbody rg in GetComponentsInChildren<Rigidbody>())
            {
                rg.isKinematic = false;
            }
            StartCoroutine(died());
            return;
        }
        if (animator.GetInteger("Action") != 0)
        {
            return;
        }
        if (inSlowMotion)
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
            {
                Time.timeScale = 1;
                inSlowMotion = false;
                //GameObject.Find("Canvas").transform.GetChild(2).transform.gameObject.SetActive(false);
                //GameObject.Find("Miguel").GetComponent<Boss1>().currentAction == Actions.cutscene;
            }
        }
        if (inCutscene != 0)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Running", false);
        }
        //healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health/100, 6 * Time.deltaTime);
        if (!inventory.inventoryOpened)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemInHand)
            {
                StartCoroutine(actionMain());
            }
            else if (Input.GetKeyDown(KeyCode.G) && itemInHand)
            {
                foreach (Transform item in GameObject.Find("HandForItems").transform)
                {
                    if (item.gameObject.activeSelf)
                    {
                        GameObject collectable = Instantiate(Resources.Load<GameObject>("CollectableItems/" + item.gameObject.name), GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
                        collectable.name = Resources.Load<GameObject>("CollectableItems/" + item.gameObject.name).name;
                        item.gameObject.SetActive(false);
                        itemInHand = false;
                        break;
                    }
                }
            }
            if (Input.GetMouseButton(1))
            {
                mouseX += Input.GetAxis("Mouse X");
                mouseY -= Input.GetAxis("Mouse Y");
            }
            if (Input.GetKey(KeyCode.Mouse1) || Input.GetAxis("Aim") >= 0.1F)
            {
                if (!animator.GetBool("Aiming") && !crouching && !animator.GetBool("Stealth"))
                {
                    Aim();
                }
            }
            else if (animator.GetBool("Aiming"))
            {
                StopAim();
            }
            if (animator.GetBool("Aiming"))
            {
                aimingDistance -= 100.0F * Time.deltaTime;
            }
            else
            {
                aimingDistance += 100.0F * Time.deltaTime;
            }
            aimingDistance = Mathf.Clamp(aimingDistance, 30.0F, 50.0F);
            cmCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = aimingDistance;
        }
        //if (oldPosition != transform.position) {
        if (!rolling && !animator.GetBool("Aiming") && !inCover && GameObject.FindObjectOfType<CameraControl>().inCombat)
        {
            if (animator.GetBool("Sprint"))
            {
                if (Input.GetKey(KeyCode.Space) || Input.GetButton("AButton"))
                {
                    StartCoroutine(roll());
                }
                //}
            }
        }
        oldPosition = transform.position;
        mouseY = Mathf.Clamp(mouseY, -60f, 60f);
        moveFB = Input.GetAxis("Vertical");
        moveLR = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(0.0F, 0.0F, 0.0F);
        movement = new Vector3(0.0F, jumpAmount, moveFB * speedFB * 2.5F * Time.deltaTime);
        Vector3 inCoverMovement = new Vector3(-moveLR, 0.0F, 0.0F);
        movement = character.rotation * movement;
        movement.y -= 150.0F * Time.deltaTime;
        turnSpeedMultiplier = 1f;
        if (!controller.isGrounded)
        {
            controller.Move(new Vector3(0.0F, -150.0F * Time.deltaTime, 0.0F));
        }
        if (onBorder && animator.GetInteger("WalkingAnimation") == 0)
        {
            animator.SetBool("Walking", false);
        }
    }

    public void Aim()
    {
        GameObject.Find("CM_ThirdCamera").GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 150.0F;
        animator.SetBool("Aiming", true);
        cmCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = aimingDistance;
        //cmCam.GetComponent<CinemachineFreeLook>().LookAt = GameObject.Find("AimPivot").transform.GetChild(1);
        crossHair.SetActive(true);
        aiming = true;
        if (GameObject.Find("Escopeta"))
        {
            GameObject.Find("Escopeta").transform.localPosition = new Vector3(15.98F, -39.02F, 8.99F);
            GameObject.Find("Escopeta").transform.localRotation = Quaternion.Euler(-191.421F, -210.566F, -31.51001F);
        }
    }

    public void StopAim()
    {
        GameObject.Find("CM_ThirdCamera").GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 250.0F;
        animator.SetBool("Aiming", false);
        if (GameObject.FindObjectOfType<CameraControl>().inCombat)
        {
            cmCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = aimingDistance;
        }
        else
        {
            cmCam.GetComponent<CinemachineFreeLook>().m_Lens.FieldOfView = aimingDistance;
        }
        crossHair.SetActive(false);
        aiming = false;
        if (GameObject.Find("Escopeta"))
        {
            GameObject.Find("Escopeta").transform.localPosition = new Vector3(14.69F, -41.82F, 8.11F);
            GameObject.Find("Escopeta").transform.localRotation = Quaternion.Euler(-187.137F, -213.006F, -60.849F);
        }
    }
    public IEnumerator takeDamage(float damage)
    {
        ////Debug.Log ("TakenDamage");
        health -= damage;
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(0.3F);
        animator.SetBool("Hit", false);
    }
    public IEnumerator overItemChanged()
    {
        //actionText.gameObject.GetComponent<LocalizedText>().key = string.Format("{0}\n Aperte 'E' para pegar\n Pressione 'E' para coletar", overItem);
        yield return new WaitForSeconds(2.0F);
        actionText.text = "";
    }
    public void callOverItemChanged()
    {
        StartCoroutine(overItemChanged());
    }
    public void callTakeDamage(float damage)
    {
        StartCoroutine(takeDamage(damage));
    }
    public IEnumerator actionMain()
    {
        animator.SetBool("ActionMain", true);
        yield return new WaitForSeconds(2.6F);
        animator.SetBool("ActionMain", false);
        foreach (Transform item in GameObject.Find("HandForItems").transform)
        {
            if (item.transform.gameObject.activeSelf)
            {
                ////Debug.Log (item.name);
                //Instantiate(Resources.Load<GameObject>("ItemPrefabsForDropping/" + item.name), item.transform.position, item.transform.rotation);
                item.transform.gameObject.SetActive(false);
                break;
            }
        }
        itemInHand = false;
    }
    public void pickUpCoroutine()
    {
        StartCoroutine(pickUp());
    }
    public IEnumerator pickUp()
    {
        animator.SetBool("PickingUp", true);
        yield return new WaitForSeconds(0.4F);
        animator.SetBool("PickingUp", false);
    }
    public IEnumerator roll()
    {
        rolling = true;
        animator.SetInteger("Roll", 1);
        yield return new WaitForSeconds(0.9F);
        animator.SetInteger("Roll", 0);
        rolling = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindObjectOfType<Cutscene002Manager>().secondMapTrigger != null)
        {
            if (other.transform.gameObject.name == GameObject.FindObjectOfType<Cutscene002Manager>().secondMapTrigger.name)
            {
                GameObject.FindObjectOfType<Cutscene002Manager>().startSecondMapTriggerIEnumerator();
                inCutscene = 2;
                Destroy(GameObject.FindObjectOfType<Cutscene002Manager>().secondMapTrigger);
            }
        }
        if (GameObject.FindObjectOfType<Cutscene002Manager>().firstMapTrigger != null)
        {
            if (other.transform.gameObject.name == GameObject.FindObjectOfType<Cutscene002Manager>().firstMapTrigger.name)
            {
                GameObject.FindObjectOfType<Cutscene002Manager>().startFirstMapTriggerIEnumerator();
                inCutscene = 2;
                //GameObject.Find ("Objective").GetComponent<TextMeshProUGUI> ().text = "";
                Destroy(GameObject.FindObjectOfType<Cutscene002Manager>().firstMapTrigger);
            }
        }
        if (other.transform.gameObject.name == "SawTrigger")
        {
            //if(inCutscene == 11){
            //GameObject.FindObjectOfType<Cutscene002Manager>().gotSaw();
            //}
        }
        if (other.transform.gameObject.name == "TriggerStartRun")
        {
            //if(inCutscene == 11){
            //GameObject.FindObjectOfType<Cutscene002Manager>().gotSawCutscene();
            //}
        }
    }

    public IEnumerator jump()
    {
        jumping = true;
        animator.SetBool("Jump", true);
        jumpAmount = 2.0F;
        yield return new WaitForSeconds(0.78F);
        jumping = false;
        animator.SetBool("Jump", false);
        jumpAmount = 0.0F;
    }

    public IEnumerator dead()
    {
        animator.SetInteger("Dead", 1);
        yield return new WaitForSeconds(4.0F);
    }

    public IEnumerator sprintSaw()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2.5F, 3.4F));
        //GameObject.FindObjectOfType<Cutscene002Manager>().gotSaw();
    }

    public IEnumerator endScreeen()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(2.5F, 3.4F));
        endScreen.SetActive(true);
    }

    public IEnumerator died()
    {
        yield return new WaitForSeconds(3.0F);
        loadingScreen.SetActive(true);
        yield return null;
        yield return new WaitForSeconds(1.0F);
        foreach (GameObject zombie in GameObject.FindGameObjectsWithTag("NormalZombie"))
        {
            if (Vector3.Distance(transform.position, zombie.transform.position) > 30.0F)
            {
                yield return null;
            }
            if (zombie.transform.position != zombie.GetComponent<EnemyNormalZombie>().startPosition)
            {
                zombie.transform.position = zombie.GetComponent<EnemyNormalZombie>().startPosition;
            }
        }
        transform.position = GetComponent<Waypoint>().oldPosition;
        animator.enabled = true;
        foreach (Rigidbody rg in GetComponentsInChildren<Rigidbody>())
        {
            rg.isKinematic = true;
        }
        health = 100;
        yield return new WaitForSeconds(5.0F);
        //loadingScreen.SetActive(false);
    }
}