using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Animator anim;

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
    [SerializeField]
    private float attackLength;
    [SerializeField]
    private float timeBetweenAttacks;
    private bool alreadyAttacked;


    // States
    [SerializeField]
    private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRnage;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // check if player in sight/attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRnage = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRnage) Patroling();
        if (playerInSightRange && !playerInAttackRnage) ChasePlayer();
        if (playerInAttackRnage && playerInSightRange) AttackPlayer();
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
        anim.SetBool("is_running", true);
        agent.SetDestination(player.position);

        // fix bug where if you move slightly during attack ai will pause until <timeinbetweenattacks> var is complete
        alreadyAttacked = false;

        // Cancel current invokes to prevent later issues
        CancelInvoke();
    }

    private void AttackPlayer()
    {
        if (!alreadyAttacked)
        {
            ResetAnim();
            anim.SetBool("is_attacking", true);
            Debug.Log("setting attack to true");
        }
        

        // make sure enemy doesn't move
        agent.SetDestination(transform.position);

        // If you don't subtract y by 1.45 zombie will lay perp to plr. This is bc zombie is smaller than plr. 
        // Will need to adjust this value if we make the zombie smaller most likely.
        var targetPos = new Vector3(player.position.x, player.position.y - 1.45f, player.position.z);
        transform.LookAt(targetPos);

        if (!alreadyAttacked)
        {
            Debug.Log("attack!");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackLength);
        }
    }

    private void ResetAttack()
    {
        ResetAnim();
        anim.SetBool("is_cooldown", true);
        Debug.Log("in cooldown...");

        Invoke(nameof(FinishAttack), timeBetweenAttacks);
    }

    private void FinishAttack()
    {
        Debug.Log("wait time over");

        alreadyAttacked = false;
    }

    // idk if you already added this
    public void AITakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Invoke(nameof(KillEnemy), 0.5f);
    }

    private void KillEnemy()
    {
        anim.SetBool("is_dead", true);
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
