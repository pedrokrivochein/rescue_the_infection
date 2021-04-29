using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class InventoryItem : MonoBehaviour {
	public bool isSelected = false;
	public bool isOver = false;
	public Vector3 oldPosition;
	public Quaternion oldRotation;
	public bool canDrop = false;
	public Inventory inventoryScript;
	public bool selectedForActions = false;
	public int amount = 1;
	// Use this for initialization
	void Start () {
		isOver = true;
		canDrop = false;
		inventoryScript = GameObject.Find("InventoryManager").GetComponent<Inventory>();
		//isSelected = true;
		selectedForActions = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!inventoryScript.inventoryOpened){
			return;
		}
		Ray ray = inventoryScript.inventoryCamera.GetComponent<Camera>().ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 2F)) {
			if (hit.collider.gameObject == this.gameObject) {
				isOver = true;
			} else {
				isOver = false;
			}
		}
		if (!isSelected) {
			if(isOver){
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					RaycastHit hitt;
					if(GameObject.Find("Canvas").transform.childCount == 5){
						if(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject.name.StartsWith("InventoryActions")){
							if(Physics.Raycast(ray, out hitt, 2F)){
								if(!hit.collider.CompareTag("InventoryActions")){
									Destroy(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject, 0.15F);
								}
							}
						}
					}
					oldPosition = transform.position;
					oldRotation = transform.localRotation;
					transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -5.95F);
					isSelected = true;
					/*Color a = new Color (GetComponent<Renderer>().material.color.r, GetComponent<Renderer>().material.color.g, GetComponent<Renderer>().material.color.b, 0.5F);
					GetComponent<Renderer>().material.color = a;*/
				}else if(Input.GetKeyDown(KeyCode.Mouse1)){
					RaycastHit hitt;
					if(GameObject.Find("Canvas").transform.childCount >= 4){
						if(GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).transform.gameObject.name.StartsWith("InventoryActions")){
							if(Physics.Raycast(ray, out hitt, 2F)){
								if(!hit.collider.CompareTag("InventoryActions")){
									Destroy(GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).transform.gameObject, 0.15F);
								}
							}
						}
					}
					GameObject inventoryActions = Instantiate(Resources.Load<GameObject>("ItemPrefabsForInventory/InventoryActions")); 
					inventoryActions.transform.SetParent(GameObject.Find("Canvas").transform);
					inventoryActions.transform.position = new Vector3(Input.mousePosition.x + 20, Input.mousePosition.y + 10);
					foreach(Transform item in GameObject.Find("Inventory").transform){
						if(item.GetComponent<InventoryItem>() != null){
							item.GetComponent<InventoryItem>().selectedForActions = false;
						}
					}
					selectedForActions = true;
				}
			}else{
				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					RaycastHit hitt;
					if(GameObject.Find("Canvas").transform.childCount == 5){
						if(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject.name.StartsWith("InventoryActions")){
							if(Physics.Raycast(ray, out hitt, 2F)){
								if(!hit.collider.CompareTag("InventoryActions")){
									Destroy(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject, 0.15F);
								}
							}
						}
					}
				}else if(Input.GetKeyDown(KeyCode.Mouse1)){
					RaycastHit hitt;
					if(GameObject.Find("Canvas").transform.childCount == 5){
						if(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject.name.StartsWith("InventoryActions")){
							if(Physics.Raycast(ray, out hitt, 2F)){
								if(!hit.collider.CompareTag("InventoryActions")){
									Destroy(GameObject.Find("Canvas").transform.GetChild(4).transform.gameObject, 0.15F);
								}
							}
						}
					}
				}
			}
		} else {
			Vector3 mousePosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 1.15F);
			Vector3 objPosition = inventoryScript.inventoryCamera.GetComponent<Camera>().ScreenToWorldPoint (mousePosition);
			transform.position = objPosition;
			////Debug.Log ("Foi");
			RaycastHit hitt;
			if (Input.GetKey (KeyCode.D)) {
				transform.localRotation = new Quaternion (transform.localRotation.x, transform.localRotation.y, transform.localRotation.z - 0.03F, transform.localRotation.w);
			} else if (Input.GetKey (KeyCode.A)) {
				transform.localRotation = new Quaternion (transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 0.03F, transform.localRotation.w);
			}
			foreach (Transform item in transform) {
				if (item.gameObject.name.StartsWith("GameObject")) {
					Debug.DrawRay(item.transform.position, item.transform.forward);
					if (Physics.Raycast (item.transform.position, item.transform.forward, out hitt, 10.0F)) {
						if (hitt.collider.gameObject.name != "InventoryBase") {
							transform.Find ("Ignore").GetComponent<Renderer> ().material.color = Color.red;
							return;
						}
					}else{
						transform.Find ("Ignore").GetComponent<Renderer> ().material.color = Color.red;
						return;
					}
				}
			}
			transform.Find ("Ignore").GetComponent<Renderer> ().material.color = Color.green;
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				isSelected = false;
				transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -5.619999F);
				/*Color a = new Color (GetComponent<Renderer> ().material.color.r, GetComponent<Renderer> ().material.color.g, GetComponent<Renderer> ().material.color.b, 1F);
				GetComponent<Renderer> ().material.color = a;*/
			}
		}
	}
	public void done(){
		isSelected = false;
		transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -5.619999F);
	}
}
