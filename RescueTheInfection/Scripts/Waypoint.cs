using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Waypoint : MonoBehaviour {

	public GameObject canvas;
	public GameObject marker;
	public GameObject target;
	public Camera cam;
	public GameObject wp;
	GameObject gameManager;
	public bool inCutscene = false;
	public Vector3 oldPosition;
	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Camera").transform.GetChild(0).transform.gameObject.GetComponent<Camera>();
		wp = Instantiate(marker);
		wp.GetComponent<RectTransform>().SetParent(canvas.transform);
		wp.GetComponent<RawImage>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(inCutscene){
			wp.GetComponent<RawImage>().enabled = false;
			return;
		}else{
			float check = Vector3.Dot((target.transform.position - cam.transform.position).normalized, cam.transform.forward);
			if(check <= 0.0F || GameObject.FindObjectOfType<AwayMeters>().meters < 10.0F){
				wp.GetComponent<RawImage>().enabled = false;
				wp.transform.GetChild(0).transform.gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
			}else if(GameObject.FindObjectOfType<AwayMeters>().meters > 10.0F){
				wp.GetComponent<RawImage>().enabled = true;
				wp.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + target.transform.lossyScale.y, target.transform.position.z));
				wp.transform.GetChild(0).transform.gameObject.GetComponent<TextMeshProUGUI>().enabled = true;
			}
		}
	}
}
