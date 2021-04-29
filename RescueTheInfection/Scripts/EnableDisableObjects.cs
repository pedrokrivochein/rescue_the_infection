using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableObjects : MonoBehaviour
{
    public GameObject[] objects;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("enabledisableObject", 0.0F, 30.0F);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enabledisableObject(){
        foreach(GameObject objectHere in objects){
            if(Vector3.Distance(transform.position, objectHere.transform.position) < 100.0F){
                if(objectHere.activeSelf){
                    return;
                }else{
                   objectHere.SetActive(true);
                }
            }else{
                if(!objectHere.activeSelf){
                    return;
                }else{
                   objectHere.SetActive(false);
                }
            }
        }
    }
}
