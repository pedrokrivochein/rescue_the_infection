using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDatabase : MonoBehaviour {
	public List<Item> items = new List<Item>();
	// Use this for initialization
	void Start () {
		ManageNewItems ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void ManageNewItems(){
		items.Add (new Item("Wood", "Wood", "Wood", 100));
	}
	public Item GetItemById(int id){
		foreach (Item itm in items) {
			if (itm.id == id) {
				return itm;
			}
		}
		return null;
	}
	public int GetIdBySprite(Sprite sprite){
		foreach (Item itm in items) {
			if (itm.spriteName == sprite.name) {
				return itm.id;
			}
		}
		return 0;
	}
	public int GetIdByName(String name){
		foreach (Item itm in items) {
			if (itm.name == name) {
				return itm.id;
			}
		}
		return 0;
	}
	public string GetNameById(int id){
		foreach (Item itm in items) {
			if (itm.id == id) {
				return itm.spriteName;
			}
		}
		return null;
	}
}
