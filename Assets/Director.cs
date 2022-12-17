using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField]
    DeathCube Enemy;

    [SerializeField]
    PlayerTestScript Player;

    [SerializeField]
    public GameObject EnemyPrefab;

    private void Awake()
    {
        Enemy = GameObject.Find("Test Enemy").GetComponent<DeathCube>();
        Player = GameObject.Find("Test Player").GetComponent<PlayerTestScript>();
    }

    public void SpawnEnemy()
    {
        if (Enemy != null)
            return;

        GameObject spawnedEnemy = Instantiate(EnemyPrefab, Player.transform.position + new Vector3(0, 0.75f, 8), Quaternion.identity);
        Enemy = spawnedEnemy.GetComponent<DeathCube>();         
    }

    public void DamageEnemy()
    {
        if (Enemy is null)
            return;

        if (Enemy.CheckHealth() <= 10)
        {
            Enemy.Damage(10);
            Enemy = null;
        } else
        {
            Enemy.Damage(10);
        }
    }

    public void DespawnEnemy()
    {
        if (Enemy is null)
            return;

        Enemy.Die();
        Enemy = null;

    }

    public void HealPlayer()
    {
        Player.Heal(10);
    }

    public void DamagePlayer()
    {
        Player.Damage(10);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
