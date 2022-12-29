using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyClass : MonoBehaviour
{

    [SerializeField]
    protected string enemyName;

    [SerializeField]
    protected int startingHealth;

    [SerializeField]
    protected int health;

    [SerializeField]
    protected Director LevelManager;

    private EnemyBehavior enemyBehavior;
    private NavMeshAgent agent;


    #region Abstract Methods

    protected virtual void Awake()
    {
        health = startingHealth;
        LevelManager = GameObject.Find("LevelManager").GetComponent<Director>();
        enemyBehavior = this.GetComponent<EnemyBehavior>();
        agent = this.GetComponent<NavMeshAgent>();
    }

    #endregion

    #region Methods

    public void Damage(int amount)
    {
        if (health == 0)
            return;

        //Debug.Log($"{gameObject.name} has recieved {amount} damage! Its new heath is: {health}");
        health = Mathf.Max(0, health - amount);

        EvaluateHealth();
    }

    public void Heal(int amount)
    {
        Debug.Log($"{gameObject.name} has healed {amount} damage!");
        health = Mathf.Min(startingHealth, health + amount);
    }

    public int CheckHealth()
    {
        return health;
    }

    void EvaluateHealth()
    {
        if(health <= 0)
            Die();
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        gameObject.GetComponent<Collider>().enabled = false;
        enemyBehavior.anim.SetBool("is_dead", true);

        // stop dead body from attacking you :P
        enemyBehavior.alreadyAttacked = true;
        
        agent.speed = 0;
        //Destroy(gameObject);
        NotifyOfDeath();
    }

    private void NotifyOfDeath()
    {
        LevelManager.RemoveEnemy(this);
    }

    #endregion

    #region IEnumerators

    IEnumerator Death()
    {
        Debug.Log($"{gameObject.name} will die in (5) seconds.");
        yield return new WaitForSeconds(5);
        Die();
    }

    #endregion


}
