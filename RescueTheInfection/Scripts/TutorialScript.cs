using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject[] tutorialObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void startTutorial(){
        
    }

    int tutorialInt;
    public IEnumerator tutorial(){
        tutorialInt = 0;
        while(tutorialInt < tutorialObjects.Length){
            tutorialObjects[tutorialInt].GetComponent<Animator>().Play("Start", 0, 0.0F);
            tutorialInt++;
            yield return new WaitForSeconds(3.0F);
        }
    }
}
