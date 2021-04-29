using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Invoke("justLoading", 5.0F);
        Invoke("startCutscene", 0.0F);
        ////Debug.Log("StartScript");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void justLoading(){
        GameObject.Find("loadingScreen").SetActive(false);
    }
    public void startCutscene(){
        //GameObject.Find("Cutscene001Manager").GetComponent<Cutscene001Manager>().StartCutscene001();
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }
}
