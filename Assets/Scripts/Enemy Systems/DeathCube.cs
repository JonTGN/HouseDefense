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

    public void EvaluateHealth()
    {
        Debug.Log($"{gameObject.name}'s health is: {health}");
    }

}
