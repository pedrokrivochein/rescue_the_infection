using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ArrowSelected : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool select = false;
    public void OnPointerEnter(PointerEventData eventData){
        GetComponent<Animator>().SetBool("Highlighted", true);
    }
    public void OnPointerExit(PointerEventData eventData){
        GetComponent<Animator>().SetBool("Highlighted", false);
    }
    public void selected(){
        if(!select){
		    transform.GetChild(0).gameObject.GetComponent<Animator>().Play("PressedSelected",0 , 0.0F);
            GetComponent<Animator>().Play("Selected", 0, 0.0F);
            select = true;
        }else{
            unSelected();
        }
	}

	public void unSelected(){
		transform.GetChild(0).gameObject.GetComponent<Animator>().Play("PressedUnselected", 0, 0.0F);
        //transform.parent.gameObject.GetComponent<Animator>().Play("Highlighted", 0, 0.0F);
        GetComponent<Animator>().SetBool("Highlighted", false);
        GetComponent<Animator>().SetBool("Normal", true);
        //GetComponent<Animator>().Play("Normal", 0, 0.0F);
        select = false;
	}
    public void continueButton(){
        Time.timeScale = 1;
		GameObject.Find("PauseMenu").SetActive(false);
		////Debug.Log("A");
    }
    public void exitToMenu(){
        SceneManager.LoadScene(0);
    }
}
