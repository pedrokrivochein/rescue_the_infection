using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBlockageBoss1 : MonoBehaviour
{
    public Animator animator;
    public Transform chest;
    public Transform head;
    public Transform rightHand;
    public float strengh = 0.0F;
    public float maxStrengh = 0.8F;
    public bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        head = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(GameObject.Find("Ana").transform.position, transform.position) < 3.0F && !attacking){
            StartCoroutine(attack());
        }
    }
    void LateUpdate(){
        //chest.transform.LookAt(GameObject.Find("Ana").transform.position + new Vector3(0.0F, 1.3F, 0.0F));
        //head.transform.LookAt(GameObject.Find("Ana").transform.position + new Vector3(0.0F, 1.5F, 0.0F));
    }
    /*void OnAnimatorIK(){
        animator.SetIKPosition(AvatarIKGoal.RightHand, GameObject.Find("Ana").transform.position + new Vector3(0.0F, 1.5F, 0.0F));
        animator.SetIKPosition(AvatarIKGoal.LeftHand, GameObject.Find("Ana").transform.position + new Vector3(0.0F, 1.5F, 0.0F));
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, strengh);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, strengh);
    }*/

    public IEnumerator attack(){
        attacking = true;
        animator.SetInteger("AttackAnimation", 2);
        yield return new WaitForSeconds(4.0F);
        animator.SetInteger("AttackAnimation", 0);
        attacking = false;
    }
}
