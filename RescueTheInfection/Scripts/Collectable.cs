using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour {

	public bool OnTrigger = false;
	public PlayerC playerScript;
	public bool selected = false;
	public float timeForInventory = 0F;
	public GameObject handForItems;
	public GameObject thisItemInHand;
	public bool canCancel = false;
	public Image loadForItem;
	public Inventory inventoryScript;
	public new string name;
	public LayerMask objectsToGrab;
	public GameObject camera;
	public bool directToInventory = false;
	public GameObject CMCamera;
	public GameObject player;
	// Use this for initialization
	void Start () {
		OnTrigger = false;
		playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerC>();
		selected = false;
		timeForInventory = 0F;
		handForItems = GameObject.Find("HandForItems");
		if(!directToInventory){
			thisItemInHand = handForItems.transform.Find(gameObject.name).transform.gameObject;
			thisItemInHand.SetActive(false);
		}
		canCancel = false;
		//loadForItem = GameObject.Find("LoadForItem").GetComponent<Image>();
		inventoryScript = GameObject.Find("InventoryManager").GetComponent<Inventory>();
		loadForItem = GameObject.Find("Canvas").transform.GetChild(2).transform.gameObject.GetComponent<Image>();
		camera = GameObject.Find("MainCamera");
		CMCamera = GameObject.Find("CM_ThirdCamera");
		player = GameObject.Find("Ana");
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(Physics.Raycast(CMCamera.transform.position, GameObject.Find("MainCamera").transform.forward, out hit, 10.0F, objectsToGrab)){
			if(hit.collider.gameObject.name == this.gameObject.name){
				transform.GetChild(0).transform.gameObject.SetActive(true);
				OnTrigger = true;
			}else{
				if(OnTrigger){
					transform.GetChild(0).transform.gameObject.SetActive(false);
					OnTrigger = false;
				}
			}
		}else{
			if(OnTrigger){
				transform.GetChild(0).transform.gameObject.SetActive(false);
				OnTrigger = false;
			}
		}
		if(OnTrigger){
			if(!selected){
				playerScript.overItem = name;
				//playerScript.callOverItemChanged();
				selected = true;
			}
			if(Input.GetKeyUp(KeyCode.E)){
				if(!canCancel){
					if(!playerScript.itemInHand){
						if(!directToInventory){
							thisItemInHand.SetActive(true);
							playerScript.itemInHand = true;
							playerScript.pickUpCoroutine();
							if(gameObject.name.Contains("Maleta")){
								player.GetComponent<PlayerC>().health = 100;
							}
							Destroy(this.gameObject);
						}else{
							loadForItem.transform.gameObject.SetActive(false);
							inventoryScript.mainInventory.SetActive(true);
							inventoryScript.inventoryCamera.SetActive(true);
							inventoryScript.mainCamera.SetActive(false);
							GameObject item = Instantiate(Resources.Load<GameObject>("ItemPrefabsForInventory/" + gameObject.name)) as GameObject;
							item.transform.SetParent(inventoryScript.mainInventory.transform);
							//item.GetComponent<InventoryItem>().isSelected = true;
							inventoryScript.updateAmmo();
							Destroy(this.gameObject);
						}
					}
				}else{
					timeForInventory = 0F;
					if(loadForItem.transform.gameObject.activeSelf){
						loadForItem.fillAmount = 0.2F;
						loadForItem.transform.gameObject.SetActive(false);
					}
					canCancel = false;
				}
			}
			if(Input.GetKey(KeyCode.E)){
				timeForInventory += Time.deltaTime;
				if(canCancel){
					loadForItem.fillAmount = timeForInventory;
					if(!loadForItem.transform.gameObject.activeSelf){
						loadForItem.transform.gameObject.SetActive(true);
					}
				}
				if(timeForInventory >= 1.0F){
					loadForItem.transform.gameObject.SetActive(false);
					inventoryScript.mainInventory.SetActive(true);
					inventoryScript.inventoryCamera.SetActive(true);
					inventoryScript.mainCamera.SetActive(false);
					GameObject item = Instantiate(Resources.Load<GameObject>("ItemPrefabsForInventory/" + gameObject.name)) as GameObject;
					item.transform.SetParent(inventoryScript.mainInventory.transform);
					//item.GetComponent<InventoryItem>().isSelected = true;
					Destroy(this.gameObject);
				} else if(timeForInventory >= 0.2F && timeForInventory < 1.0F && !canCancel){
					canCancel = true;
					if(!loadForItem.transform.gameObject.activeSelf){
						loadForItem.transform.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			//OnTrigger = true;
		}
	}

	public void OnTriggerExit(Collider other){
		if(OnTrigger){
			//OnTrigger = false;
			//selected = false;
		}
	}

	public void OnDestroy(){
	}
}
