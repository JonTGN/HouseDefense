using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestScript : Player
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Player : MonoBehaviour
{
    [SerializeField]
    protected string enemyName;

    [SerializeField]
    protected int startingHealth;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected int health;

    public void EvaluateHealth()
    {
        Debug.Log($"{gameObject.name}'s health is: {health}");
    }

}

