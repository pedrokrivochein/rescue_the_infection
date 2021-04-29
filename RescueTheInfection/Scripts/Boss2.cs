using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2 : MonoBehaviour
{

    public float speed = 1.0F;
    public enum Actions { Following, Idle, Attacking, Defending, Hit, ThrowingAxe, GrabbingAxe, BoxAttacking };
    public float health = 2000F;
    public Animator animator;
    public Vector3 oldPosition;
    public Actions currentAction;
    public GameObject player;
    public bool canAttack = false;
    public bool running = false;
    public bool canSpecialAttack = false;
    public bool canDash = false;
    public int damageInt = 0;
    public bool damaging = false;
    public int attackNumber = 0;
    public bool canGetDamage = false;
    public NavMeshAgent navMeshAgent;
    public Material bloodOnDamage;
    public GameObject machado;
    public Vector3 machadoStartPosition;
    public Transform machadoParent;
    public Vector3 keepMachadoLastPosition;
    public Quaternion machadoStartRotation;
    public float machadoTime = 0.0F;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAction = Actions.Following;
        canAttack = true;
        speed = 1.0F;
        running = false;
        canSpecialAttack = true;
        InvokeRepeating("canChange", 0.0F, 1.0F);
        health = 2000F;
        canDash = false;
        damageInt = 0;
        damaging = false;
        canGetDamage = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
        machadoParent = machado.transform.parent;
        machadoStartPosition = machado.transform.localPosition;
        machadoStartRotation = machado.transform.localRotation;
        keepMachadoLastPosition = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            animator.SetInteger("Dead", 1);
            return;
        }
        bloodOnDamage.SetFloat("_Dirtiness", ((health - 1800) / -1800) / 2);
        oldPosition = transform.position;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (currentAction == Actions.Following)
        {
            if (distance < 3)
            {
                animator.SetBool("Running", false);
                animator.SetBool("Walking", false);
                StartCoroutine(attack(UnityEngine.Random.Range(1, 3)));
                return;
            }
            else if (distance < 6)
            {
                if (canAttack)
                {
                    if (distance > 6 && canSpecialAttack)
                    {
                        if (attackNumber == 2)
                        {
                            StartCoroutine(specialAttack(3));
                        }
                    }
                    else
                    {
                        running = true;
                    }
                }
                else
                {
                    if (health < 1000)
                    {
                        running = true;
                    }
                    else
                    {
                        if (canDash)
                        {
                            StartCoroutine(dash());
                        }
                    }
                }
            }
            else if (distance < 11)
            {
                if (canAttack)
                {
                    if (distance < 10 && canSpecialAttack)
                    {
                        bool random = (Random.value > 0.75F);
                        if (random)
                        {
                            animator.SetBool("Running", true);
                            animator.SetBool("Walking", false);
                            StartCoroutine(throwBox());
                            return;
                        }
                        StartCoroutine(specialAttack(2));
                    }
                    else
                    {
                        running = true;
                    }
                }
                else
                {
                    if (health < 1000)
                    {
                        running = true;
                    }
                    else
                    {
                        if (canDash)
                        {
                            StartCoroutine(dash());
                        }
                    }
                }
            }
            if (!GetComponent<CharacterController>().isGrounded)
            {
                transform.position -= new Vector3(0.0F, 2.0F * Time.deltaTime, 0.0F);
            }
            navMeshAgent.SetDestination(player.transform.position);
            /*transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
			Vector3 dir = player.transform.position + new Vector3(0.0F, 1.0F, 0.0F) - transform.position + new Vector3(0.0F, 1.0F, 0.0F);
			Quaternion lookRotation = Quaternion.LookRotation(dir);
			Vector3 rotation = lookRotation.eulerAngles;
			transform.rotation = Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z);*/
        }
        else if (currentAction == Actions.Attacking)
        {
            Vector3 dir = player.transform.position + new Vector3(0.0F, 1.0F, 0.0F) - transform.position + new Vector3(0.0F, 1.0F, 0.0F);
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z), 6.0F * Time.deltaTime);
            return;
        }
        else if (currentAction == Actions.BoxAttacking)
        {
            if (holdingBox)
            {
                Vector3 dir = player.transform.position + new Vector3(0.0F, 1.0F, 0.0F) - transform.position + new Vector3(0.0F, 1.0F, 0.0F);
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = lookRotation.eulerAngles;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z), 6.0F * Time.deltaTime);
                return;
            }
            return;
        }
        else if (currentAction == Actions.ThrowingAxe)
        {
            if (machadoTime < 2.0F)
            {
                machado.transform.GetChild(0).transform.gameObject.GetComponent<Animator>().SetBool("ThrowingAxe", true);
                machado.transform.position += machado.transform.forward * 10.0F * Time.deltaTime;
                machado.transform.LookAt(player.transform.position + new Vector3(0.0F, 1.5F, 0.0F));
                machadoTime += Time.deltaTime;
                if (Vector3.Distance(machado.transform.position, player.transform.position + new Vector3(0.0F, 1.5F, 0.0F)) < 0.2F)
                {
                    player.GetComponent<PlayerC>().callTakeDamage(Random.Range(40.0F, 60.0F));
                    machadoTime = 3.0F;
                }
            }
            else
            {
                machado.GetComponent<AxeThrowingBoss2Script>().canStuck = true;
                //machado.transform.GetChild(0).transform.gameObject.GetComponent<Animator>().SetBool("ThrowingAxe", false);
                machado.GetComponent<Rigidbody>().isKinematic = false;
                currentAction = Actions.GrabbingAxe;
            }
        }
        else if (currentAction == Actions.GrabbingAxe)
        {
            if (keepMachadoLastPosition == Vector3.zero)
            {
                return;
            }
            navMeshAgent.SetDestination(keepMachadoLastPosition);
            running = true;
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
            float distanceTM = Vector3.Distance(transform.position, keepMachadoLastPosition);
            if (distanceTM < 3.0F)
            {
                closeEnoughToGrab = true;
                if (grabWeight < 1.0F && distanceTM > 0.4F)
                {
                    grabWeight += 2.0F * Time.deltaTime;
                }
                if (distanceTM < 1.0F)
                {
                    machado.transform.SetParent(machadoParent);
                    machado.transform.localPosition = Vector3.Lerp(machado.transform.localPosition, machadoStartPosition, 4.0F * Time.deltaTime);
                    machado.transform.localRotation = Quaternion.Lerp(machado.transform.localRotation, machadoStartRotation, 4.0F * Time.deltaTime);
                    if (distanceTM < 0.4F)
                    {
                        if (grabWeight > 0.0F)
                        {
                            grabWeight -= 2.0F * Time.deltaTime;
                        }
                        else
                        {
                            grabWeight = 0.0F;
                            closeEnoughToGrab = false;
                            canAttack = true;
                            machadoTime = 0.0F;
                            keepMachadoLastPosition = Vector3.zero;
                            StartCoroutine(canSpecialAttackCoroutine());
                            currentAction = Actions.Following;
                        }
                    }
                }
            }
        }
        if (running)
        {
            animator.SetBool("Running", true);
            animator.SetBool("Walking", false);
            speed = 2.0F;
        }
        else
        {
            animator.SetBool("Walking", true);
            animator.SetBool("Running", false);
            speed = 1.0F;
        }
        if (notHoldingBox)
        {
            GameObject gBox = GameObject.Find("GrabableBox");
            if (Vector3.Distance(gBox.transform.position, player.transform.position + new Vector3(0.0F, 1.5F, 0.0F)) < 3.0F)
            {
                gBox.transform.position += gBox.transform.forward * 3.0F * Time.deltaTime;
                if (Vector3.Distance(gBox.transform.position, player.transform.position + new Vector3(0.0F, 1.5F, 0.0F)) < 1.0F)
                {
                    player.GetComponent<PlayerC>().callTakeDamage(Random.Range(40.0F, 50.0F));
                    notHoldingBox = false;
                    gBox.transform.position = GameObject.Find("GrabableBoxSpawn").transform.position;
                    gBox.transform.rotation = GameObject.Find("GrabableBoxSpawn").transform.rotation;
                    gBox.GetComponent<Rigidbody>().isKinematic = true;
                    StartCoroutine(canSpecialAttackCoroutine());
                }
                return;
            }
            gBox.transform.LookAt(player.transform.position + new Vector3(0.0F, 3.0F, 0.0F));
            gBox.transform.position += gBox.transform.forward * distance * Time.deltaTime;
            gBox.GetComponent<Rigidbody>().AddForce(gBox.transform.forward * distance);
        }
    }

    public IEnumerator canSpecialAttackCoroutine()
    {
        yield return new WaitForSeconds(6.0F);
        canSpecialAttack = true;
    }
    void OnAnimatorMove()
    {
        if (animator)
        {
            navMeshAgent.speed = (animator.deltaPosition / Time.deltaTime).magnitude * UnityEngine.Random.Range(0.8F, 1.0F);
        }
    }

    public IEnumerator attack(int attackNumber)
    {
        animator.SetInteger("Attack", attackNumber);
        canGetDamage = false;
        canAttack = false;
        currentAction = Actions.Attacking;
        yield return new WaitForSeconds(2.0F);
        canGetDamage = true;
        if(Vector3.Distance(transform.position, player.transform.position) < 2.5F){
            player.GetComponent<PlayerC>().callTakeDamage(Random.Range(10.0F, 20.0F));
        }
        if (attackNumber == 1)
        {
            yield return new WaitForSeconds(3.16F - 2.0F);
        }
        else if (attackNumber == 2)
        {
            yield return new WaitForSeconds(2.5F - 2.0F);
        }
        animator.SetInteger("Attack", 0);
        canAttack = true;
        currentAction = Actions.Following;
    }

    bool holdingBox = false;
    bool notHoldingBox = false;
    public IEnumerator throwBox()
    {
        currentAction = Actions.BoxAttacking;
        canSpecialAttack = false;
        navMeshAgent.SetDestination(GameObject.Find("GrabableBoxPosition").transform.position);
        while (Vector3.Distance(transform.position, GameObject.Find("GrabableBoxPosition").transform.position) > 0.4F)
        {
            yield return new WaitForSeconds(0.1F);
        }
        GameObject gBox = GameObject.Find("GrabableBox");
        Vector3 dir = gBox.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(transform.rotation.x, rotation.y, transform.rotation.z);
        animator.SetInteger("SpecialAttack", 4);
        yield return new WaitForSeconds(0.2F);
        gBox.transform.SetParent(animator.GetBoneTransform(HumanBodyBones.RightHand));
        gBox.transform.localPosition = new Vector3(-0.013F, 0.01F, 0.3F);
        gBox.transform.localRotation = Quaternion.Euler(13.2F, -11.1F, 56F);
        gBox.transform.localScale = new Vector3(0.4682839F, 0.4682839F, 0.4682839F);
        holdingBox = true;
        yield return new WaitForSeconds(1.95F);
        animator.SetInteger("SpecialAttack", 0);
        gBox.transform.SetParent(transform.parent);
        gBox.GetComponent<Rigidbody>().isKinematic = false;
        notHoldingBox = true;
        //gBox.GetComponent<Rigidbody>().AddForce(gBox.transform.forward * 10.0F * Time.deltaTime);
        currentAction = Actions.Following;
    }
    bool closeEnoughToGrab = false;
    float grabWeight = 0.0F;
    void OnAnimatorIK()
    {
        if (closeEnoughToGrab)
        {
            animator.SetLookAtPosition(machado.transform.position);
            animator.SetLookAtWeight(grabWeight, grabWeight - 0.2F, grabWeight);
            animator.SetIKPosition(AvatarIKGoal.RightHand, machado.transform.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, grabWeight);
        }
    }

    public IEnumerator specialAttack(int specialAttackNumber)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + new Vector3(0.0F, 1.0F, 0.0F), player.transform.position + new Vector3(0.0F, 1.0F, 0.0F), out hit))
        {
            transform.LookAt(hit.transform.position);
        }
        canGetDamage = false;
        animator.SetInteger("SpecialAttack", specialAttackNumber);
        currentAction = Actions.Attacking;
        canAttack = false;
        canSpecialAttack = false;
        canGetDamage = true;
        if (specialAttackNumber == 1)
        {
            yield return new WaitForSeconds(4.6F);
        }
        else if (specialAttackNumber == 2)
        {
            yield return new WaitForSeconds(1.6F);
            currentAction = Actions.ThrowingAxe;
            machado.transform.SetParent(transform.root.parent);
        }
        else if (specialAttackNumber == 3)
        {
            yield return new WaitForSeconds(2.1F);
        }
        animator.SetInteger("SpecialAttack", 0);
        //canAttack = true;
        //currentAction = Actions.Following;
    }

    public IEnumerator dash()
    {
        animator.SetInteger("Dash", 1);
        canAttack = false;
        canDash = false;
        currentAction = Actions.Attacking;
        yield return new WaitForSeconds(4.6F);
        animator.SetInteger("Dash", 0);
        canAttack = true;
        currentAction = Actions.Following;
    }

    public void canChange()
    {
        attackNumber = UnityEngine.Random.Range(1, 4);
    }
}
