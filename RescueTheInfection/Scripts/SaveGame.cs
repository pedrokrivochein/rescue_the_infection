using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveGame : MonoBehaviour {

	private Vector3 PlayerPos;
	private Quaternion PlayerRot;

	private bool PressSave;
	private float PlayerVida;

	public GameObject inventoryGameObject;

	public List<string> inventoryName = new List<string>();
	public List<Vector3> inventoryPosition = new List<Vector3>();
	public List<Quaternion> inventoryRotation = new List<Quaternion>();

	public GameObject inventoryCamera;
	public GameObject mainCamera;
	public GameObject loadingScreen;

	// Use this for initialization
	void Start () {
		//inventoryGameObject = GameObject.Find("InventoryManager").GetComponent<Inventory>().mainInventory;
		//inventoryCamera = GameObject.Find("InventoryManager").GetComponent<Inventory>().inventoryCamera;
		//mainCamera = GameObject.Find("InventoryManager").GetComponent<Inventory>().mainCamera;
		//loadingScreen = GameObject.Find("LoadingScreen");
		loadingScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)) {
			//Save();
		}
		if (Input.GetKeyDown(KeyCode.P)) {
			//Load ();
		}

		/*float distance = Vector3.Distance (GameObject.Find ("Ana").transform.position, transform.position);//GetComponent<CameraController> ().m_target.transform.position, transform.position);

		if (distance < 10) {
			////Debug.Log("AreaSave");
			if (PressSave){
				////Debug.Log ("Salvando......");
				Save ();
			}
		}*/

	}
	/*public void Save(){
		BinaryFormatter BF = new BinaryFormatter ();
		FileStream file;
		PlayData data = new PlayData ();

		PlayerPos = GameObject.Find ("Ana").transform.position;
		PlayerRot = GameObject.Find ("Ana").transform.rotation;
		//PlayerVida = GameObject.Find ("Player").hp;

		file = File.Create(Application.persistentDataPath + "/PlayersSave.dat");

		data.PlayerPosX = PlayerPos.x;
		data.PlayerPosY = PlayerPos.y;
		data.PlayerPosZ = PlayerPos.z;

		data.PlayerRotX = PlayerRot.x;
		data.PlayerRotY = PlayerRot.y;
		data.PlayerRotZ = PlayerRot.z;
		
		data.PlayerVidaL = PlayerVida;

		data.itemName.Clear();
		data.itemLocalPosX.Clear();
		data.itemLocalPosY.Clear();
		data.itemLocalPosZ.Clear();
		data.itemLocalRotX.Clear();
		data.itemLocalRotY.Clear();
		data.itemLocalRotZ.Clear();
		data.itemPosX.Clear();
		data.itemPosY.Clear();
		data.itemPosZ.Clear();
		data.itemRotX.Clear();
		data.itemRotY.Clear();
		data.itemRotZ.Clear();
		foreach(Transform item in inventoryGameObject.transform){
			if(!item.gameObject.name.StartsWith("Inventory")){
				data.itemName.Add(item.gameObject.name);
				data.itemLocalPosX.Add(item.localPosition.x);
				data.itemLocalPosY.Add(item.localPosition.y);
				data.itemLocalPosZ.Add(item.localPosition.z);
				data.itemLocalRotX.Add(item.localRotation.x);
				data.itemLocalRotY.Add(item.localRotation.y);
				data.itemLocalRotZ.Add(item.localRotation.z);
				data.itemPosX.Add(item.position.x);
				data.itemPosY.Add(item.position.y);
				data.itemPosZ.Add(item.position.z);
				data.itemRotX.Add(item.rotation.x);
				data.itemRotY.Add(item.rotation.y);
				data.itemRotZ.Add(item.rotation.z);
			}
		}
		BF.Serialize (file, data);
		file.Close ();
		////Debug.Log ("Save Concluido!");
	}
	public void Load (){
		if (File.Exists (Application.persistentDataPath + "/PlayersSave.dat")) {
			loadingScreen.SetActive(true);
			BinaryFormatter BF = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + "/PlayersSave.dat", FileMode.Open);
			PlayData data = (PlayData)BF.Deserialize(file);
			file.Close ();

			PlayerPos.x = data.PlayerPosX;
			PlayerPos.y = data.PlayerPosY;
			PlayerPos.z = data.PlayerPosZ;

			PlayerRot.x = data.PlayerRotX;
			PlayerRot.y = data.PlayerRotY;
			PlayerRot.z = data.PlayerRotZ;

			//PlayerVida = data.PlayerVidaL;
			
			GameObject.Find ("Ana").transform.position = PlayerPos;
			GameObject.Find ("Ana").transform.rotation = PlayerRot;
			//GameObject.Find ("Player").hp = PlayerVida;
			//inventoryGameObject.SetActive(true);
			mainCamera.SetActive(false);
			//inventoryCamera.SetActive(true);
			/*for(int i = 0; i < data.itemName.Count; i++){
				GameObject go = Instantiate(Resources.Load("ItemPrefabsForInventoryLoad/" + data.itemName[i].Replace("(Clone)", ""))) as GameObject;
				go.transform.SetParent(inventoryGameObject.transform);
				go.GetComponent<InventoryItem>().done();
				go.GetComponent<InventoryItem>().isOver = false;
				go.GetComponent<InventoryItem>().isSelected = false;
				go.transform.localPosition = new Vector3(data.itemLocalPosX[i], data.itemLocalPosY[i], data.itemLocalPosZ[i]);
				go.transform.localRotation = new Quaternion(data.itemLocalRotX[i], data.itemLocalRotY[i], data.itemLocalRotZ[i], go.transform.localRotation.w);
			}
			inventoryGameObject.SetActive(false);
			mainCamera.SetActive(true);
			inventoryCamera.SetActive(false);
			loadingScreen.SetActive(false);
			////Debug.Log ("Load Concluido!");
		}
	}*/

}

[System.Serializable]
class PlayData
{
	public float PlayerPosX;
	public float PlayerPosY;
	public float PlayerPosZ;


	public float PlayerRotX;
	public float PlayerRotY;
	public float PlayerRotZ;

	public float PlayerVidaL;

	public List<string> itemName = new List<string>();
	public List<float> itemLocalPosX = new List<float>();
	public List<float> itemLocalPosY = new List<float>();
	public List<float> itemLocalPosZ = new List<float>();
	public List<float> itemLocalRotX = new List<float>();
	public List<float> itemLocalRotY = new List<float>();
	public List<float> itemLocalRotZ = new List<float>();
	public List<float> itemPosX = new List<float>();
	public List<float> itemPosY = new List<float>();
	public List<float> itemPosZ = new List<float>();
	public List<float> itemRotX = new List<float>();
	public List<float> itemRotY = new List<float>();
	public List<float> itemRotZ = new List<float>();
}

