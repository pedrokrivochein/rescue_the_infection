using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSelf : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
	Transform startParent;
	public int amount = 1;
	GameObject itemGhost;
	bool rigthMouse;
	bool leftMouse;
	public GameObject ghostPrefab;
	public bool stackable;
	// Use this for initialization
	void Start () {
		startParent = transform.parent;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Mouse0)) {
			leftMouse = true;
			rigthMouse = false;
		} else if (Input.GetKey (KeyCode.Mouse1)) {
			leftMouse = false;
			rigthMouse = true;
		}
		if (TagItemCheckForStacking("Apple") || TagItemCheckForStacking("Orange") || TagItemCheckForStacking("Wood")){
			stackable = true;
		} else {
			stackable = false;
		}
	}
	public void OnBeginDrag(PointerEventData eventData){
		itemGhost = Instantiate (ghostPrefab) as GameObject;
		ghostPrefab.GetComponent<Image> ().sprite = transform.gameObject.GetComponent<Image> ().sprite;
		itemGhost.transform.position = eventData.position;
		itemGhost.transform.SetParent (transform.parent.parent.parent);
		itemGhost.transform.GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData eventData){
		itemGhost.transform.position = eventData.position;
	}
	public void OnEndDrag(PointerEventData eventData){
		Destroy (itemGhost);
		transform.SetParent (startParent);
		transform.localPosition = Vector2.zero;
		transform.gameObject.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GetSlotInfo ();
	}
	void GetSlotInfo(){
		GameObject[] slots = GameObject.FindGameObjectsWithTag ("Slot");
		foreach (var slot in slots) {
			if (slot.GetComponent<Slots> ().isSelected == true) {
				if (slot.transform.childCount > 1) {
					ItemSelf otherItem = slot.transform.GetChild (1).GetComponent<ItemSelf> ();
					if (otherItem.transform.gameObject.GetComponent<Image> ().sprite == transform.gameObject.GetComponent<Image> ().sprite && stackable) {
						if (otherItem.startParent != startParent) {
							if (rigthMouse && amount > 1) {
								otherItem.amount += 1;
								amount -= 1;
							} else if (rigthMouse && amount <= 1) {
								otherItem.amount += amount;
								Destroy (gameObject);
							}
							if (leftMouse) {
								otherItem.amount += amount;
								Destroy (gameObject);
							}
						}
					} else {
						if (slot.transform.GetChild (1).gameObject.GetComponent<Image>().sprite != transform.gameObject.GetComponent<Image> ().sprite) {
							Transform otherItemParent = otherItem.startParent;
							otherItem.startParent = startParent;
							startParent = otherItemParent;
							otherItem.transform.SetParent (otherItem.startParent);
							otherItem.transform.localPosition = Vector2.zero;
						}
					}
				} else {
					if (rigthMouse && amount > 1) {
						GameObject item = Instantiate (ghostPrefab) as GameObject;
						item.transform.SetParent (slot.transform);
						item.transform.localPosition = Vector2.zero;
						item.transform.gameObject.GetComponent<ItemSelf> ().amount = 1;
						amount -= 1;
						break;
					} else if (rigthMouse && amount <= 1){
						startParent = slot.transform;
					} else if (leftMouse) {
						startParent = slot.transform;
					}
				}
		}
		if (this.gameObject != null) {
			transform.SetParent (startParent);
			transform.localPosition = Vector2.zero;
		}
	}
	}
	bool TagItemCheckForStacking(string name){
		if (transform.gameObject.GetComponent<Image> ().sprite.name == name) {
			return true;
		} else {
			return false;
		}
	}
}