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

    protected virtual void takeDamage(int amount)
    {
        Debug.Log($"{gameObject.name} has recieved {amount} damage!");
        health -= amount;
    }

    protected virtual void healDamage(int amount)
    {
        Debug.Log($"{gameObject.name} has healed {amount} damage!");
        health += amount;
    }

    protected virtual void Awake()
    {
        health = startingHealth;
    }

}
