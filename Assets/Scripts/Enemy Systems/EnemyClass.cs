using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{

    [SerializeField]
    protected string enemyName;

    [SerializeField]
    protected int startingHealth;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected int health;

    [SerializeField]
    protected Director LevelManager;

    #region Abstract Methods

    protected virtual void Awake()
    {
        health = startingHealth;
        LevelManager = GameObject.Find("LevelManager").GetComponent<Director>();
    }

    #endregion

    #region Methods

    public void Damage(int amount)
    {
        Debug.Log($"{gameObject.name} has recieved {amount} damage!");
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
