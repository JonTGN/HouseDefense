using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCube : EnemyClass
{

    protected override void Awake()
    {
        base.Awake();
        //StartCoroutine(Death());
    }

    public void damage(int amount)
    {
        takeDamage(amount);
    }

    IEnumerator Death()
    {
        Debug.Log($"{gameObject.name} will die in (5) seconds.");
        yield return new WaitForSeconds(5);
        EvaluateHealth();
        Die();
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    public void EvaluateHealth()
    {
        Debug.Log($"{gameObject.name}'s health is: {health}");
    }

}
