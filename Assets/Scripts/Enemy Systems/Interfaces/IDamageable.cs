using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{

    /// <summary>
    /// Inflict damage to GameObject's health.
    /// </summary>
    /// <param name="amount"> Amount of damage taken </param>
    public void takeDamage(int amount);

    /// <summary>
    /// Replenish damage to GameObject's health.
    /// </summary>
    /// <param name="amount"> Amount of damage replenished </param>
    public void healDamage(int amount);

    /// <summary>
    /// Evaluate health total and update GameObject's state.
    /// </summary>
    public void EvaluateHealth();

    ///<summary>
    /// Destroy GameObject.
    /// </summary>
    public void Die();
}
