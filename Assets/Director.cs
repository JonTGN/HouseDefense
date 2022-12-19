using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Director : MonoBehaviour
{
    [SerializeField]
    public int Round = 1;

    [SerializeField]
    public int WaveSize = 0;

    [SerializeField]
    public float WaveStrength = 0;

    [SerializeField]
    public float WaveModifier = 0f;

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

        //if (PopulateWave)
        //    //SPAWN FULL WAVE
        //    SpawnWave();

        SetupNewRound();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(new Vector3(0,0,0) + new Vector3(0,0,Mathf.Ceil(spawnLength/2)), new Vector3(spawnWidth,1,spawnLength));
    }

    private void SetupNewRound()
    {
        // Calculate the wave modifier by evaluating scores on ammo, health, and time to complete previous round.
        // Temporary value is 1.
        WaveModifier = 1;

        // Calculate the wave strength with function * wave modifier
        // Temporary wave strength calculation is: (2 + (2 x Round))
        WaveStrength = Mathf.Ceil((2 + (2 * Round)) * WaveModifier);

        // Increment round counter
        Round += 1;

        StartRound();
    }

    private void StartRound()
    {
        SpawnWave();
    }

    public void RemoveEnemy(EnemyClass instance)
    {
        Enemies.Remove(instance);
        Destroy(instance.gameObject);
        Debug.Log($"{gameObject.name} has died and been removed.");
        WaveSize -= 1;

        if(WaveSize == 0)
        {
            Debug.Log("ROUND OVER");
            SetupNewRound();
        }    
    }

    private void SpawnWave()
    {
        while (WaveStrength > 0)
        {
            SpawnEnemy();
            WaveSize += 1;
            WaveStrength -= 1;
        }
    }

    public void SpawnEnemy()
    {
        //if (Enemies.Count >= WaveSize)
        //    return;

        GameObject spawnedEnemy = Instantiate(EnemyPrefab, new Vector3(Random.Range(0, spawnWidth) - (spawnWidth / 2f), 1.75f, Random.Range(0, spawnLength) - (spawnLength / 2f)), Quaternion.identity);
        Enemies.Add(spawnedEnemy.GetComponent<DeathCube>());
    }

    public void DamageEnemy()
    {
        Debug.Log("Obsolete function");
    }

    public void DespawnEnemy()
    {
        Debug.Log("Obsolete function");
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
