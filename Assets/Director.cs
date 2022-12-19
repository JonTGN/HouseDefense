using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField]
    public int WaveSize = 0;

    [SerializeField]
    public int WaveStrength = 0;

    [SerializeField]
    List<EnemyClass> Enemies = new List<EnemyClass>();

    [SerializeField]
    PlayerTestScript Player;

    [SerializeField]
    public bool PopulateWave;


    [SerializeField]
    public GameObject EnemyPrefab;

    [SerializeField]
    public int spawnWidth;

    [SerializeField]
    public int spawnLength;

    private void Awake()
    {
        //Enemy = GameObject.Find("Test Enemy").GetComponent<DeathCube>();
        Player = GameObject.Find("Test Player").GetComponent<PlayerTestScript>();

        if (PopulateWave)
            //SPAWN FULL WAVE
            SpawnWave();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(new Vector3(0,0,0) + new Vector3(0,0,spawnLength/2), new Vector3(spawnWidth,1,spawnLength));
    }

    public void RemoveEnemy(EnemyClass instance)
    {
        Enemies.Remove(instance);
        Destroy(instance.gameObject);
        Debug.Log($"{gameObject.name} has died and been removed.");

    }

    private void SpawnWave()
    {
        while(WaveStrength > 0)
        {
            SpawnEnemy();
            WaveStrength -= 1;
        }
    }

    public void SpawnEnemy()
    {
        if (Enemies.Count >= WaveSize)
            return;

        GameObject spawnedEnemy = Instantiate(EnemyPrefab, new Vector3(Random.Range(0, spawnWidth), 1.75f, Random.Range(0, spawnLength)), Quaternion.identity);
        Enemies.Add(spawnedEnemy.GetComponent<DeathCube>());
    }

    public void DamageEnemy()
    {
        //if (Enemy == null)
        //    return;

        //if (Enemy.CheckHealth() <= 10)
        //{
        //    Enemy.Damage(10);
        //    Enemy = null;
        //} else
        //{
        //    Enemy.Damage(10);
        //}
    }

    public void DespawnEnemy()
    {
        //if (Enemy == null)
        //    return;

        //Enemy.Die();
        //Enemy = null;

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
