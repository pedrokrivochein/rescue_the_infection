using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene001Manager : MonoBehaviour {
	public GameObject audioManager;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void StartCutscene001(){
		GetComponent<PlayableDirector>().Play();
		GameObject.FindObjectOfType<Waypoint>().inCutscene = true;
		StartCoroutine(Cutscene001());
	}

	public IEnumerator Cutscene001(){
		audioManager.GetComponent<AudioManager>().Play("WaterBoat");
		yield return new WaitForSeconds(1.0F);
		//Marcelo - Chegamos! Voce fica aqui e eu vou fazer o reconhecimento da area.
		audioManager.GetComponent<AudioManager>().Play("Chegamos! Voce fica aqui e eu vou fazer o reconhecimento da area.");
		yield return new WaitForSeconds(7.0F);
		audioManager.GetComponent<AudioManager>().Stop("WaterBoat");
		//Ana - Ta na escuta?
		yield return new WaitForSeconds(3.0F);
		audioManager.GetComponent<AudioManager>().Play("Ta na escuta?");
		yield return new WaitForSeconds(1.4F);
		//Marcelo - Positivo Cambio!.
		audioManager.GetComponent<AudioManager>().Play("Positivo Cambio!");
		//GameObject.FindGameObjectOfType<GameManagerScript>().inCutscene = false;
		yield return new WaitForSeconds(5.0F);
		GameObject.Find("ObjectiveTutorialManager").GetComponent<ObjectiveTutorial>().setObjectiveOrTutorial("Press X to see controls");
		yield return new WaitForSeconds(7.0F);
		GameObject.Find("ObjectiveTutorialManager").GetComponent<ObjectiveTutorial>().setObjectiveOrTutorial("Go to the Cabin");
		GameObject.FindObjectOfType<Waypoint>().inCutscene = false;
		GameObject.FindObjectOfType<ControllersControl>().Show();
		//Ana - Deixa eu checar as coordenadas
	}
}
