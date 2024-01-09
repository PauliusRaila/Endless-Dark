using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Invector;

public class EnemyNPC : MonoBehaviour {

    public float startHealth = 50;
    private float health;
    private Rigidbody rb;
    
   // public float wanderRange = 20f;
    public float attackRadius = 2f;
    public float lookRadius = 8f;
    public int chanceToSpawn;
    NavMeshAgent agent;
    NavMeshObstacle navObstacle;
    private Vector3 knockDestination;
    //GameObject healtCanvas;
    public Transform nearestTarget = null;
    GameObject[] enemyTargets;
    private NavMeshHit navHit;
   // public Image healthBar;

    //Behaviour states
    public enum EnemyState { isChasing, isAttacking, isIdle, isDead }
    public EnemyState currentState;

    Animator anim;

    // Use this for initialization
    void OnEnable () {
        InitializeReferences();        
    }

    private void Start()
    {
        enemyTargets = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        

        this.transform.parent = null;
    }

    private void FixedUpdate()
    {
        
        updateTarget();      
    }

    private void OnDrawGizmosSelected()
   {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, attackRadius);
      
    }

    private float checkDistanceToTarget() {
        float distanceToEnemy = Vector3.Distance(nearestTarget.transform.position, transform.position);
        return distanceToEnemy;
    }

    private void InitializeReferences()
    {
        navObstacle = GetComponent<NavMeshObstacle>();
        agent = GetComponent<NavMeshAgent>();
        
        anim = GetComponent<Animator>();
        //healtCanvas = transform.Find("healthCanvas").gameObject;
        health = startHealth;
        setCurrentState(EnemyState.isIdle);

        //checkRate = Random.RandomRange(0.2f, 0.5f);
    }

    private void DisableThis()
    {
        this.enabled = false;
    }


    public void updateTarget()
    {     
        float shortestDistance = Mathf.Infinity;
        ///  checkDistanceToTarget

        if (nearestTarget == null)
        {
            foreach (GameObject target in enemyTargets)
            {
                float distanceToEnemy = Vector3.Distance(target.transform.position, transform.position);

                if (distanceToEnemy <= shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestTarget = target.transform;
                    //MoveToTarget(nearestTarget);
                }
            }
        }
        else
        {
          
                if (checkDistanceToTarget() <= lookRadius && checkDistanceToTarget() > attackRadius)
                {
                    setCurrentState(EnemyState.isChasing);
                    MoveToTarget(nearestTarget);
                }
                else if (checkDistanceToTarget() <= attackRadius)
                {

                    setCurrentState(EnemyState.isAttacking);

                }
                else
                {
                    setCurrentState(EnemyState.isIdle);
                }
            
            
          
          

        } 
          
    }

    private void knockback() {
       Vector3 direction = transform.position - nearestTarget.position;
       knockDestination = direction * 1f;
       rb.detectCollisions = false;
       anim.SetTrigger("gethit");
       rb.AddForce(knockDestination);

    }

    private void setCurrentState(EnemyState state)
    {
        currentState = state;

        switch (currentState) {
            case EnemyState.isAttacking:
                anim.SetBool("idle", false);
                anim.SetBool("chasing", false);
                if(agent.hasPath)
                    agent.isStopped = true;

                if(!GetComponent<vHealthController>().isDead)
                anim.SetTrigger("attack");

                break;
            case EnemyState.isChasing:               
                anim.SetBool("idle", false);
                agent.isStopped = false;
                anim.SetBool("chasing", true);
                break;
            case EnemyState.isIdle:
                anim.SetBool("chasing", false);
                anim.SetBool("idle", true);

                if(agent.hasPath)
                    agent.isStopped = true;
                break;

            case EnemyState.isDead:
                anim.SetBool("chasing", false);
                anim.SetBool("idle", false);
                agent.isStopped = true;

                anim.SetTrigger("dead");
                break;
        }
    }

    private void MoveToTarget(Transform nearestTarget)
    {
       // float distance = Vector3.Distance(nearestTarget.transform.position, transform.position);

        if (agent.enabled == false) {
            navObstacle.enabled = false;
            agent.enabled = true;
           
        }
        
        agent.SetDestination(nearestTarget.position);                 
    }



    public void triggerDamage() {
        Debug.Log("triggerDamage");
        if (agent.isActiveAndEnabled) {
            agent.isStopped = true;
            agent.enabled = false;
            //agent.
        }
        knockback();

        if (GetComponent<vHealthController>().currentHealth <= 0)
        {
          
            anim.SetTrigger("dead");
            //Destroy(this);
        }           
    }
}
