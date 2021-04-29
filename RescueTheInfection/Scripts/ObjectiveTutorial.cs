using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveTutorial : MonoBehaviour
{
    public GameObject ot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setObjectiveOrTutorial(string text){
        StartCoroutine(setObjOrTut(text));
    }
    
    public IEnumerator setObjOrTut(string text){
        ot.SetActive(true);
        ot.transform.GetChild(1).transform.gameObject.GetComponent<TextMeshProUGUI>().text = text;
        ot.GetComponent<Animator>().Play("ObjectiveTutorial", 0, 0.0F);
        yield return new WaitForSeconds(5.0F);
        ot.SetActive(false);
    }
}
