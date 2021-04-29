using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasToSeeCutscene002 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") && !GameObject.FindObjectOfType<Cutscene002Manager>().did){
            GameObject.FindObjectOfType<Cutscene002Manager>().gotSaw();
        }
    }
}
