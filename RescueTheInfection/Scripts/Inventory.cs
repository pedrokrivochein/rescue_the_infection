using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	public GameObject slot;
	public int slotAmount = 30;
	public GameObject slotPrefab;
	public List<GameObject> slots = new List<GameObject>();
	ItemDatabase itemDatabase;
	public GameObject itemPrefab;
	public bool inventoryOpened = false;
	public GameObject mainInventory;
	Vector3 startPosition;
	public GameObject mainCamera;
	public GameObject inventoryCamera;
	public int pistolAmmo = 0;
	public int rifleAmmo = 0;
	public int shotgunAmmo = 0;
	// Use this for initialization
	void Start () {
		mainInventory = GameObject.Find ("Inventory");
		/*for (int i = 0; i < slotAmount; i++) {
			slots.Add (Instantiate (slotPrefab));
			slots [i].transform.SetParent (slot.transform);
		}*/
		/*foreach (Transform slot in GameObject.Find("Hotbar").transform) {
			slots.Add (slot.gameObject);
			////Debug.Log (slot.gameObject.name);
		}*/
		itemDatabase = GameObject.FindGameObjectWithTag ("InventoryManager").GetComponent<ItemDatabase> ();
		//mainInventory.SetActive (false);
		mainCamera = GameObject.Find("MainCamera");
		inventoryCamera = GameObject.Find("InventoryCamera");
		inventoryCamera.SetActive(false);
		//mainInventory.SetActive (false);
		foreach(Transform item in mainInventory.transform){
			if(item.gameObject.name.StartsWith("PistolAmmo")){
				pistolAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}else if(item.gameObject.name.StartsWith("RifleAmmo")){
				rifleAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}else if(item.gameObject.name.StartsWith("ShotgunAmmo")){
				shotgunAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			InsertItem (0);
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			if (!inventoryOpened) {
				inventoryOpened = true;
				mainInventory.SetActive (true);
				inventoryCamera.SetActive(true);
				mainCamera.SetActive(false);
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			} else {
				inventoryOpened = false;
				mainInventory.SetActive (false);
				inventoryCamera.SetActive(false);
				mainCamera.SetActive(true);
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}
	
	public void InsertItem(int id){
		bool forCompleted = false;
		if (itemDatabase == null) {
			itemDatabase = GameObject.FindGameObjectWithTag ("InventoryManager").GetComponent<ItemDatabase> ();
		}
		Item itemToAdd = itemDatabase.GetItemById (id);
		for (int i = 0; i < slots.Count; i++) {
			if (slots [i].transform.childCount > 1) {
				if (slots [i].transform.GetChild (1).GetComponent<Image> ().sprite.name == itemToAdd.spriteName) {
					if (slots [i].transform.GetChild (1).GetComponent<ItemSelf> ().stackable) {
						slots [i].transform.GetChild (1).GetComponent<ItemSelf> ().amount += 1;
						forCompleted = true;
						break;
					}
				}
			}
		}
		if (!forCompleted) {
			for (int i = 0; i < slots.Count; i++) {
				if (slots [i].transform.childCount <= 1) {
					GameObject itemAdding = Instantiate (itemPrefab);
					itemAdding.GetComponent<Image> ().sprite = itemToAdd.sprite;
					itemAdding.transform.SetParent (slots [i].transform);
					itemAdding.transform.localPosition = Vector2.zero;
					break;
				}/* else if (slots [i].transform.GetChild (1).GetComponent<Image> ().sprite.name == itemToAdd.spriteName) {
					slots [i].transform.GetChild (1).GetComponent<ItemSelf> ().amount += 1;
					break;
				}*/
			}
		}
	}
	public void updateAmmo(){
		foreach(Transform item in mainInventory.transform){
			if(item.gameObject.name.StartsWith("PistolAmmo")){
				pistolAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}else if(item.gameObject.name.StartsWith("RifleAmmo")){
				rifleAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}else if(item.gameObject.name.StartsWith("ShotgunAmmo")){
				shotgunAmmo += item.gameObject.GetComponent<InventoryItem>().amount;
			}
		}
	}
}
