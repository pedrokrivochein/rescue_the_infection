using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters;

[System.Serializable]
public class Item{
	public string name;
	public string description;
	public Sprite sprite;
	public int id;
	public string spriteName;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Item(string nm, string dscrpt, string sprtnm, int iid){
		name = nm;
		description = dscrpt;
		id = iid;
		sprite = Resources.Load<Sprite> ("ItemImages/" + sprtnm);
		spriteName = sprtnm;
		prefab = Resources.Load<GameObject> ("ItemPrefabs/" + sprtnm);
	}
	public Item(){
	}
}
