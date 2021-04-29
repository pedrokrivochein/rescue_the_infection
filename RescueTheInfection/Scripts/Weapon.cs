using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Weapon : MonoBehaviour
{

    public Weapons weapon;
    public GameObject cameraForShooting;
    public Animator playerAnimator;
    public bool shooting = false;
    public int bulletsLoaded;
    public int bulletsNotLoaded;
    public int maxBulletsToHold;
    public bool reloading = false;
    public GameObject inventory;
    public enum WeaponType { Pistol, Rifle, Shotgun };
    public WeaponType type;
    public GameObject muzzleFlash;
    public Inventory inventoryScript;
    public AudioManager audioManager;
    public bool compass = false;
    public LayerMask notRaycasted;
    public LayerMask notRaycastedZombie;
    public GameObject blood;
    public GameObject bulletHole;
    public GameObject hitDecal;
    public GameObject[] bloodDecals;
    public GameObject dirt;
    public GameObject rock;
    public GameObject player;
    public Animator animator;
    public float shotgunCooldown = 0.0F;
    // Use this for initialization
    void Start()
    {
        //cameraForShooting = GameObject.Find("CM_ThirdCamera");
        playerAnimator = player.GetComponent<Animator>();
        shooting = false;
        reloading = false;
        inventory = GameObject.Find("Inventory");
        inventoryScript = GameObject.Find("InventoryManager").GetComponent<Inventory>();
        audioManager = GetComponent<AudioManager>();
        player = GameObject.Find("Ana");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = transform.root.gameObject;
            return;
        }
        if (player.GetComponent<PlayerC>().health <= 0)
        {
            return;
        }
        if (compass)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                player.GetComponent<PlayerC>().compass = true;
            }
            else
            {
                player.GetComponent<PlayerC>().compass = false;
            }
            return;
        }
        else
        {
            player.GetComponent<PlayerC>().compass = false;
        }
        if (type == WeaponType.Pistol)
        {
            bulletsNotLoaded = inventoryScript.pistolAmmo;
        }
        else if (type == WeaponType.Rifle)
        {
            bulletsNotLoaded = inventoryScript.rifleAmmo;
        }
        else if (type == WeaponType.Shotgun)
        {
            bulletsNotLoaded = inventoryScript.shotgunAmmo;
            if (shotgunCooldown < 0.7F)
            {
                shotgunCooldown += Time.deltaTime;
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Reload"))
        {
            if (!shooting)
            {
                StartCoroutine(reload());
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetAxis("Fire1") >= 0.1)
        {
            if (playerAnimator.GetInteger("Dead") == 1)
            {
                return;
            }
            if (bulletsLoaded <= 0)
            {
                GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = "0/0";
                //audioManager.Play("OutOfAmmo");
            }
            if (!shooting && bulletsLoaded > 0 && playerAnimator.GetBool("Aiming") && !playerAnimator.transform.gameObject.GetComponent<CoverSystem>().inCover && playerAnimator.GetInteger("Aiming") != 3)
            {
                StartCoroutine(shoot());
                RaycastHit hit;
                if (type == WeaponType.Pistol)
                {
                    muzzleFlash.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(recoil(0.05F));
                    Ray ray = new Ray(cameraForShooting.transform.position, GameObject.Find("MainCamera").transform.forward);
                    if (Physics.Raycast(ray, out hit, 200F, notRaycasted))
                    {
                        causeDamage(hit, ray);
                    }
                    return;
                }
                else if (type == WeaponType.Shotgun)
                {
                    muzzleFlash.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(recoil(0.1F));
                    shotgunCooldown = 0.0F;
                    animator.Play("Shoot", 0, -0.3F);
                    for (int i = 0; i < 12; i++)
                    {
                        Vector3 randomDirection = GameObject.Find("MainCamera").transform.forward + new Vector3(Random.Range(-0.2F, 0.2F), Random.Range(-0.2F, 0.2F), 0.1F);
                        Ray ray = new Ray(cameraForShooting.transform.position, GameObject.Find("MainCamera").transform.forward + randomDirection);
                        if (Physics.Raycast(ray, out hit, 20.0F, notRaycasted))
                        {
                            causeDamage(hit, ray);
                            Debug.DrawRay(cameraForShooting.transform.position, GameObject.Find("MainCamera").transform.forward + GameObject.Find("MainCamera").transform.up * Random.Range(-0.1F, 1.0F), Color.blue);
                        }
                    }
                }
            }
        }
    }

    public IEnumerator recoil(float range)
    {
        GameObject cam = GameObject.Find("CM_ThirdCamera");
        AimScript re = transform.root.gameObject.GetComponent<AimScript>();
        float saveValue = re.recoilAmount;//cam.GetComponent<CinemachineFreeLook>().m_YAxis.Value;
        //cam.GetComponent<CinemachineFreeLook>().m_YAxis.Value -= range/2;
        re.recoilAmount -= range;
        while (re.recoilAmount < 0.0F)
        {
            //cam.GetComponent<CinemachineFreeLook>().m_YAxis.Value += 0.005F;
            re.recoilAmount += 0.005F;
            yield return new WaitForSeconds(0.01F);
        }
        re.recoilAmount = 0.0F;
    }

    public void causeDamage(RaycastHit hit, Ray ray)
    {
        if (hit.collider.gameObject.transform.root.transform.gameObject.GetComponent<EnemyHealthManager>() != null)
        {
            if (Vector3.Distance(transform.position, hit.point) <= 1.0F)
            {
                transform.root.gameObject.GetComponent<AimScript>().aimingOn = false;
                playerAnimator.Play("PushPistol", 0);
                StartCoroutine(closePunch(hit, ray));
                return;
            }
            if (Vector3.Angle(hit.collider.gameObject.transform.position - playerAnimator.gameObject.transform.position, playerAnimator.gameObject.transform.forward) < 180.0F)
            {
                hit.collider.gameObject.transform.root.transform.gameObject.GetComponent<EnemyHealthManager>().takeDamage(hit.collider.gameObject.name, weapon.damage, hit, ray);
                GameObject go = Instantiate(blood, hit.point, transform.root.rotation, hit.collider.transform.parent) as GameObject;
                go.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
                GameObject goDecal = Instantiate(hitDecal, hit.point + new Vector3(0.0F, 0.0F, 0.002F), transform.root.rotation, hit.collider.transform) as GameObject;
                goDecal.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
                goDecal.transform.LookAt(transform.parent.position);
                Destroy(goDecal, 10.0F);
                Debug.Log("HIT");
                //Destroy(go, 2.0F);
                ////Debug.Log("Blood");
                //StartCoroutine(ragdollHit(hit));
                if (Physics.Raycast(cameraForShooting.transform.position, GameObject.Find("MainCamera").transform.forward, out hit, 10.0F, notRaycastedZombie))
                {
                    GameObject decalBlood = Instantiate(bloodDecals[UnityEngine.Random.Range(0, bloodDecals.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
                    decalBlood.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    Destroy(decalBlood, 30.0F);
                }
            }
            return;
        }
        if (hit.collider.gameObject.name.ToLower().Contains("terrain"))
        {
            GameObject go = Instantiate(dirt, hit.point, transform.root.rotation, hit.collider.transform.parent) as GameObject;
            go.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Destroy(go, 2.0F);
        }
        else if (hit.collider.name.ToLower().Contains("rock"))
        {
            GameObject go = Instantiate(rock, hit.point, transform.root.rotation, hit.collider.transform.parent) as GameObject;
            go.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Destroy(go, 2.0F);
        }
    }
    public IEnumerator closePunch(RaycastHit hit, Ray ray)
    {
        yield return new WaitForSeconds(0.5F);
        hit.collider.gameObject.transform.root.transform.gameObject.GetComponent<EnemyHealthManager>().takeDamage(hit.collider.gameObject.name, 100, hit, ray);
        GameObject go = Instantiate(blood, hit.point, transform.root.rotation, hit.collider.transform.parent) as GameObject;
        go.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
        GameObject goDecal = Instantiate(hitDecal, hit.point + new Vector3(0.0F, 0.0F, 0.002F), transform.root.rotation, hit.collider.transform) as GameObject;
        goDecal.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -hit.normal);
        goDecal.transform.LookAt(transform.parent.position);
        Destroy(goDecal, 10.0F);
        Debug.Log("HIT");
        //Destroy(go, 2.0F);
        ////Debug.Log("Blood");
        //StartCoroutine(ragdollHit(hit));
        /*if (Physics.Raycast(cameraForShooting.transform.position, GameObject.Find("MainCamera").transform.forward, out hit, 10.0F, notRaycastedZombie))
        {
            GameObject decalBlood = Instantiate(bloodDecals[UnityEngine.Random.Range(0, bloodDecals.Length - 1)], hit.point + (hit.normal * 0.025f), Quaternion.identity, hit.collider.gameObject.transform) as GameObject;
            decalBlood.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            Destroy(decalBlood, 30.0F);
        }*/
        yield return new WaitForSeconds(0.8F);
        transform.root.gameObject.GetComponent<AimScript>().aimingOn = true;
    }

    public IEnumerator shoot()
    {
        if (bulletsLoaded > 0)
        {
            shooting = true;
            playerAnimator.SetInteger("Shoot", 1);
            GameObject camera = GameObject.Find("Camera");
            //camera.transform.rotation = Quaternion.Euler(camera.transform.rotation.x -4, camera.transform.rotation.y, camera.transform.rotation.z);
            //muzzleFlash.GetComponent<ParticleSystem>().Play();
            //audioManager.Play("PistolShoot");
            removeBullets();
            yield return new WaitForSeconds(0.25F);
            bulletsLoaded -= 1;
            shooting = false;
            playerAnimator.SetInteger("Shoot", 0);
            GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = bulletsLoaded + "/0";
        }
        else
        {
            //audioManager.Play("OutOfAmmo");
            yield return null;
        }
    }

    public IEnumerator reload()
    {
        reloading = true;
        playerAnimator.Play("Reloading");
        GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = "Reloading.";
        yield return new WaitForSeconds(0.33F);
        GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = "Reloading..";
        yield return new WaitForSeconds(0.33F);
        GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = "Reloading...";
        //audioManager.Play("Reload");
        yield return new WaitForSeconds(0.33F);
        GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = "";
        if (bulletsLoaded != maxBulletsToHold)
        {
            if ((bulletsLoaded + bulletsNotLoaded) > maxBulletsToHold)
            {
                bulletsLoaded = maxBulletsToHold;
                bulletsNotLoaded -= maxBulletsToHold;
            }
            else if ((bulletsLoaded + bulletsNotLoaded) == maxBulletsToHold)
            {
                bulletsLoaded = maxBulletsToHold;
                bulletsNotLoaded = 0;
            }
            else
            {
                bulletsLoaded += bulletsNotLoaded;
                bulletsNotLoaded = 0;
            }
        }
        GameObject.Find("ReloadingText").GetComponent<TextMeshProUGUI>().text = bulletsLoaded + "/0";
        reloading = false;
    }

    public void removeBullets()
    {
        foreach (Transform item in inventory.transform)
        {
            if (type == WeaponType.Pistol)
            {
                if (item.transform.gameObject.name.StartsWith("PistolAmmo"))
                {
                    if (item.transform.gameObject.GetComponent<InventoryItem>().amount > 0)
                    {
                        item.transform.gameObject.GetComponent<InventoryItem>().amount -= 1;
                        inventoryScript.pistolAmmo -= 1;
                        if (item.transform.gameObject.GetComponent<InventoryItem>().amount <= 0)
                        {
                            Destroy(item.transform.gameObject);
                        }
                    }
                }
            }
            else if (type == WeaponType.Rifle)
            {
                if (item.transform.gameObject.name.StartsWith("RifleAmmo"))
                {
                    if (item.transform.gameObject.GetComponent<InventoryItem>().amount > 0)
                    {
                        item.transform.gameObject.GetComponent<InventoryItem>().amount -= 1;
                        inventoryScript.rifleAmmo -= 1;
                        if (item.transform.gameObject.GetComponent<InventoryItem>().amount <= 0)
                        {
                            Destroy(item.transform.gameObject);
                        }
                    }
                }
            }
            else if (type == WeaponType.Shotgun)
            {
                if (item.transform.gameObject.name.StartsWith("ShotgunAmmo"))
                {
                    if (item.transform.gameObject.GetComponent<InventoryItem>().amount > 0)
                    {
                        item.transform.gameObject.GetComponent<InventoryItem>().amount -= 1;
                        inventoryScript.shotgunAmmo -= 1;
                        if (item.transform.gameObject.GetComponent<InventoryItem>().amount <= 0)
                        {
                            Destroy(item.transform.gameObject);
                        }
                    }
                }
            }
        }
    }

    public IEnumerator ragdollHit(RaycastHit hit)
    {
        if (!hit.collider.gameObject.name.Contains("Head"))
        {
            if (hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyNormalZombie>() != null && hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyNormalZombie>().health >= 1.0F)
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                hit.collider.gameObject.transform.root.GetComponent<Animator>().enabled = false;
                yield return new WaitForSeconds(0.08F);
                hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
                hit.collider.gameObject.transform.root.GetComponent<Animator>().enabled = true;
            }
        }
    }
}
