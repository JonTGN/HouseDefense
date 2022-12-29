using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Transform player;

    public Animator anim;

    [SerializeField]
    private LayerMask whatIsGround, whatIsPlayer;

    [SerializeField]
    private float health;

    // Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField]
    private float walkPointRange;

    // Attacking
    [SerializeField] int damage;
    [SerializeField]
    private float attackLength;
    [SerializeField]
    private float timeBetweenAttacks;
    public bool alreadyAttacked;


    // States
    [SerializeField]
    private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRnage;


    [SerializeField]
    private Director levelDirector;

    public AudioClip[] zombie_ambience;
    private AudioClip currentClip;
    public AudioSource source;
    public float minWaitTime, maxWaitTime;
    float waitTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("PlayerCapsule").transform;
        levelDirector = GameObject.Find("LevelManager").GetComponent<Director>();
    }

    private void Update()
    {
        // check if player in sight/attack range
        Vector3 endpt = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRnage = Physics.CheckCapsule(transform.position, endpt, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRnage) Patroling();
        if (playerInSightRange && !playerInAttackRnage && !alreadyAttacked) ChasePlayer();
        if (playerInAttackRnage && playerInSightRange) AttackPlayer();

        // sound mngr
        if (!source.isPlaying)
        {
            if (waitTime < 0f)
            {
                currentClip = zombie_ambience[Random.Range(0, zombie_ambience.Length)];
                source.clip = currentClip;
                source.Play();
                waitTime = Random.Range(minWaitTime, maxWaitTime);
            }

            else 
                waitTime -= Time.deltaTime;
        }
    }

    // probably will never use, nice to have though
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calc random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        ResetAnim();
        anim.SetBool("is_walking", true);
        agent.SetDestination(player.position);

        // fix bug where if you move slightly during attack ai will pause until <timeinbetweenattacks> var is complete
        //alreadyAttacked = false;

        // Cancel current invokes to prevent later issues
        CancelInvoke();
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            ResetAnim();
            anim.SetBool("is_attacking", true);
            //Debug.Log("setting attack to true");
        }

        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        //agent.isStopped = true; // Clean this up


        //// If you don't subtract y by 1.45 zombie will lay perp to plr. This is bc zombie is smaller than plr. 
        //// Will need to adjust this value if we make the zombie smaller most likely.
        //var targetPos = new Vector3(player.position.x, player.position.y - 1.45f, players.position.z);
        //transform.LookAt(targetPos); // Change to RotateTowards()

        //var targetDirection = player.position - transform.position;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection) * new Quaternion(0f, 1f, 0f, 1f);

        if (!alreadyAttacked)
        {
            //Debug.Log("attack!");
            levelDirector.DamagePlayer(damage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackLength);
        }
    }

    private void ResetAttack()
    {
        ResetAnim();
        anim.SetBool("is_cooldown", true);
        //Debug.Log("in cooldown...");

        Invoke(nameof(FinishAttack), timeBetweenAttacks);
    }

    private void FinishAttack()
    {
        //Debug.Log("wait time over");

        alreadyAttacked = false;
    }

    [System.Obsolete]
    public void AITakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetBool("is_dead", true);
            Invoke(nameof(KillEnemy), 5f);
        }
    }

    private void KillEnemy()
    {
        Destroy(gameObject);
    }

    private void ResetAnim()
    {
        anim.SetBool("is_walking", false);
        anim.SetBool("is_running", false);
        anim.SetBool("is_attacking", false);
        anim.SetBool("is_cooldown", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
