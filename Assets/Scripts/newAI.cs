using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Invector;

public class newAI : MonoBehaviour {

    private vHealthController healthController;
    private NavMeshAgent agent;
    private NavMeshHit navHit;
    private Transform nearestTarget = null;
    private Vector3 wanderTarget;
    private GameObject[] enemyTargets;
    private Animator anim;
    private Rigidbody rb;




    private IEnumerator coroutine;

    public enum EnemyState { Chasing, Wandering, Combat, Idle, Dead , Retreating}
    public EnemyState currentState = EnemyState.Idle;
    private EnemyState oldState;

    public GameObject deadBodyPrefab;

    public float attackRadius = 2f;
    public float lookRadius = 8f;
    public float attackCooldown = 4f;
    public int chanceToSpawn;

    private float timeStamp;
    private float shortestDistance = Mathf.Infinity;

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

    }

    void OnEnable () {       
        healthController = GetComponent<vHealthController>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        enemyTargets = GameObject.FindGameObjectsWithTag("Player");

        RandomWanderTarget(transform.position, lookRadius, out wanderTarget);
        coroutine = checkBehaviour(0.5f);
        StartCoroutine(coroutine);
    }

    private void FixedUpdate()
    {
        if (currentState == EnemyState.Chasing)
            agent.SetDestination(nearestTarget.position);

        if (currentState == EnemyState.Combat) {
            transform.LookAt(nearestTarget);
        }
           

    }

    private IEnumerator checkBehaviour(float waitTime)
    {
        while (currentState != EnemyState.Dead)
        {
        

            foreach (GameObject target in enemyTargets)
            {
                float distanceToEnemy = Vector3.Distance(target.transform.position, transform.position);


                
                if (distanceToEnemy > lookRadius && currentState != EnemyState.Wandering)
                {
                    if (RandomWanderTarget(transform.position, lookRadius, out wanderTarget))
                    {
                        oldState = currentState;
                        currentState = EnemyState.Wandering;
                        UpdateAnimations();

                        agent.stoppingDistance = 0.5f;
                        agent.SetDestination(wanderTarget);
                        

                    }
                }
                else if (currentState == EnemyState.Wandering && Vector3.Distance(wanderTarget, transform.position) <= 0.5f)
                {
                    oldState = currentState;
                    currentState = EnemyState.Idle;
                    UpdateAnimations();
                    agent.ResetPath();
                   
                }


                if (distanceToEnemy <= lookRadius && distanceToEnemy > attackRadius)
                {
                    oldState = currentState;
                    currentState = EnemyState.Chasing;
                    UpdateAnimations();

                    shortestDistance = distanceToEnemy;
                    nearestTarget = target.transform;

                    Debug.Log("Moving to player.");
                    
                }

                if (currentState == EnemyState.Retreating && timeStamp <= Time.time) {
                    attackRadius = 1.5f;                
                }
                    

                if (distanceToEnemy <= attackRadius && timeStamp <= Time.time)
                {
                    
                    oldState = currentState;
                    currentState = EnemyState.Combat;
                    UpdateAnimations();

                        transform.LookAt(nearestTarget);
                        anim.SetTrigger("attack");
                        timeStamp = Time.time + attackCooldown;
                           
                  
                }
                else if(distanceToEnemy <= attackRadius && timeStamp > Time.time)
                {
                        attackRadius = 4f;
                        oldState = currentState;
                        currentState = EnemyState.Retreating;
                        agent.Stop();
                        Vector3 toPlayer = target.transform.position - transform.position;                    
                        Vector3 targetPosition = toPlayer.normalized * -3f;
                        agent.destination = targetPosition;
                        agent.transform.LookAt(target.transform);
                        agent.updateRotation = false;
                        agent.Resume();
                        UpdateAnimations();
                   
                }

            }

            yield return new WaitForSeconds(waitTime);
        }
    
    }

    bool RandomWanderTarget(Vector3 centre, float range, out Vector3 result)
    {
        Vector3 randomPoint = centre + Random.insideUnitSphere * lookRadius;
        if (NavMesh.SamplePosition(randomPoint, out navHit, 1.0f, 1))
        {
            result = navHit.position;
            return true;
        }
        else
        {
            result = centre;
            return false;
        }
    }

    private void UpdateAnimations() {

        if (oldState == currentState)
            return;

        anim.SetBool("retreating", false);
        anim.SetBool("moving", false);
        anim.SetBool("idle", false);
        anim.SetBool("dead", false);

        switch (currentState) {
            

            case EnemyState.Chasing:                
                anim.SetBool("moving", true);
                break;
            case EnemyState.Wandering:
                anim.SetBool("moving", true);
                break;
            case EnemyState.Idle:
                anim.SetBool("idle", true);
                break;
            case EnemyState.Dead:
                anim.SetBool("idle", true);
                break;
            case EnemyState.Retreating:
                anim.SetBool("retreating", true);
                break;
        }
    }


    public void triggerDamage() {
        float currentHealth = GetComponent<vHealthController>().currentHealth;
        if (currentHealth <= 0) {
            currentState = EnemyState.Dead;
            agent.enabled = false;

            Instantiate(deadBodyPrefab, this.transform.position, this.transform.rotation);
            this.gameObject.SetActive(false);
            //UpdateAnimations();

        }
        else{
            agent.ResetPath();
            anim.SetTrigger("takedamage");
            Vector3 moveDirection = transform.position - nearestTarget.transform.position;         

            agent.updateRotation = false;
            agent.velocity = moveDirection.normalized * 20;         
            anim.SetBool("moving", true);
        }

    }


}
