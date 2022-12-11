using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTestScript : Player
{

    public TextMeshProUGUI playerHealth;

    // Start is called before the first frame update
    protected override void Awake()
    {
        HealthChanged += UpdateHealthLabel;
        base.Awake();
    }

    void UpdateHealthLabel(int healthTotal )
    {
        playerHealth.text = $"{healthTotal}";
        Debug.Log("UpdateHealthLabel() has been called!");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}

public class Player : MonoBehaviour
{
    public Action<int> HealthChanged;

    [SerializeField]
    protected string enemyName;

    [SerializeField]
    protected int startingHealth;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    private int health;

    protected int Health 
    {
        get => health;
        set
        {
            health = value;
            HealthChanged?.Invoke(health);
        }

    }

    protected virtual void Awake()
    {
        Health = startingHealth;
    }

    public void Damage(int damage)
    {
        Health = Mathf.Max(0, health - damage);
    }
    public void Heal(int damage)
    {
        Health = Mathf.Min(startingHealth, health + damage);
    }

    public void EvaluateHealth()
    {
        Debug.Log($"{gameObject.name}'s health is: {health}");
    }

}

