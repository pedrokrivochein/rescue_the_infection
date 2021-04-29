using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookFloat : MonoBehaviour{
    public GameObject player;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Ana");
        GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale = new Vector3(0.07590146F, 0.01F, 0.07590146F);
        InvokeRepeating("UpdateLookFloat", 1.0F, 1.0F);
    }

    // Update is called once per frame
    void Update(){
    }

    public void UpdateLookFloat(){
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("NormalZombie");
        foreach(GameObject zombie in zombies){
            if(Vector3.Distance(transform.position, zombie.transform.position) < 20.0F){
                if(zombie.GetComponent<EnemyNormalZombie>().angle <= 90){
                    if(GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y < 0.030F){
                        GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale = new Vector3(0.07590146F, GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y + 0.5F * Time.deltaTime, 0.07590146F);
                    }else{
                        GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale = new Vector3(0.07590146F, GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y - 0.5F * Time.deltaTime, 0.07590146F);
                    }
                }
                if(zombie.GetComponent<EnemyNormalZombie>().angle <= 60){
                    if(GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y < 0.07590146F){
                        GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale = new Vector3(0.07590146F, GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y + 0.5F * Time.deltaTime, 0.07590146F);
                    }
                }
                /*if(zombie.GetComponent<EnemyNormalZombie>().angle >= 120){
                    if(GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y > 0.0F){
                        GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale = new Vector3(0.07590146F, GameObject.Find("LookFloat").GetComponent<RectTransform>().localScale.y - 1.0F * Time.deltaTime, 0.07590146F);
                    }
                }*/
            }
        }
    }
}
