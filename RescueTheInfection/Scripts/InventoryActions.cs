using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Use(){
		foreach(Transform item in GameObject.Find("Inventory").transform){
			if(item.GetComponent<InventoryItem>() != null){
				if(item.GetComponent<InventoryItem>().selectedForActions){
					if(item.name.Contains("Maleta")){
						GameObject.Find("Ana").GetComponent<PlayerC>().health = 100;
					}
					Destroy(item.gameObject);
					break;
				}
			}
		}
		Destroy(gameObject);
	}
	public void Drop(){
		foreach(Transform item in GameObject.Find("Inventory").transform){
			if(item.GetComponent<InventoryItem>() != null){
				if(item.GetComponent<InventoryItem>().selectedForActions){
					foreach(var word in item.name.Split('(')){
						GameObject collectable = Instantiate(Resources.Load<GameObject>("CollectableItems/" + word), GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity) as GameObject;
						collectable.name = Resources.Load<GameObject>("CollectableItems/" + word).name;
						Destroy(item.gameObject);
						break;
					}
					break;
				}
			}
		}
		Destroy(gameObject);	
	}
}
