using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitch : MonoBehaviour
{
    public int selectedItem = 0;
    public Animator playerAnimator;
    public GameObject escopetaBack;
    // Use this for initialization
    void Start()
    {
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        SelectItem();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedItem = selectedItem;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetButtonDown("LeftBumper"))
        {
            if (selectedItem <= 0)
            {
                selectedItem = transform.childCount - 1;
            }
            else
            {
                selectedItem--;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetButtonDown("RightBumper"))
        {
            if (selectedItem >= transform.childCount - 1)
            {
                selectedItem = 0;
            }
            else
            {
                selectedItem++;
            }
        }
        if (previousSelectedItem != selectedItem)
        {
            SelectItem();
        }
    }
    void SelectItem()
    {
        int i = 0;
        foreach (Transform item in transform)
        {
            if (i == selectedItem)
            {
                playerAnimator.SetBool("ChangeAnimation", true);
                DisableOtherItemsInChild(item.transform.gameObject.name);
                playerAnimator.SetBool("Pistol", item.transform.gameObject.GetComponent<Weapon>().weapon.pistol);
                string weaponType = item.transform.gameObject.GetComponent<Weapon>().type.ToString();
                GameObject.FindObjectOfType<AimScript>().currentWeapon = weaponType;
                if(weaponType == "Shotgun"){
					StartCoroutine(equipItem(item.transform.gameObject.name)); 
					playerAnimator.SetBool("Escopeta", true);
                    escopetaBack.SetActive(false);
				}else{
					escopetaBack.SetActive(true);
					playerAnimator.SetBool("Escopeta", false);
				}
            }
            i++;
        }
    }

    public IEnumerator equipItem(string name)
    {
        GameObject.FindObjectOfType<AimScript>().grabbingWeapon = true;
        GameObject.Find("GrabAnimation").GetComponent<Animator>().Play("Grab", 0, 0.0F);
        yield return new WaitForSeconds(0.3F);
        escopetaBack.SetActive(false);
        GameObject.Find("Hand").transform.Find(name).transform.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2F);
        GameObject.FindObjectOfType<AimScript>().grabbingWeapon = false;
    }
    void DisableOtherItemsInChild(string name)
    {
        foreach (Transform items in GameObject.Find("Hand").transform)
        {
            if (items.name != name)
            {
                items.gameObject.SetActive(false);
            }
            else
            {
                items.gameObject.SetActive(true);
            }
        }
    }
}
