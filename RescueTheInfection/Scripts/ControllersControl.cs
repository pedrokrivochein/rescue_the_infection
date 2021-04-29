using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllersControl : MonoBehaviour
{
    public Animator animator;
    public bool showBool = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("XButton")){
            Show();
        }    
    }
    
    public void Show(){
        showBool = !showBool;
        animator.SetBool("on", showBool);  
    }

}
