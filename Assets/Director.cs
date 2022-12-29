using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    [SerializeField]
    public int Round = 1;

    [SerializeField]
    public int finalRound = 1;

    [SerializeField]
    public int WaveSize = 0;

    [SerializeField]
    public float WaveStrength = 0;

    [SerializeField]
    public float WaveModifier = 0f;

    [SerializeField]
    List<EnemyClass> Enemies = new List<EnemyClass>();

    [SerializeField]
    GameObject[] SpawnPoints;

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

    [SerializeField]
    private float bodyTimer;

    [SerializeField]
    private PlayerInteraction playerInteraction;
    bool hasAlreadyStarted;

    private void Awake()
    {
        //Enemy = GameObject.Find("Test Enemy").GetComponent<DeathCube>();
        Player = GameObject.Find("Test Player").GetComponent<PlayerTestScript>();

        //if (PopulateWave)
        //    //SPAWN FULL WAVE
        //    SpawnWave();

        SpawnPoints = GameObject.FindGameObjectsWithTag("Spawn");

        // setup new round when fireplace is activated
        //SetupNewRound();

    }

    void Update()
    {
        if (playerInteraction.alreadyLitFireplace && !hasAlreadyStarted)
        {
            SetupNewRound();
            hasAlreadyStarted = true;
        }
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
        WaveStrength = Mathf.Ceil((4 + (4 * Round)) * WaveModifier);

        // Increment round counter
        Round += 1;

        if (Round == finalRound)
            //test
            EndGame();
        else
            StartRound();
    }

    private void StartRound()
    {
        SpawnWave();
    }

    private void EndGame()
    {
       SceneManager.LoadScene("WinScene");
    }

    public void RemoveEnemy(EnemyClass instance)
    {
        Enemies.Remove(instance);
        
        StartCoroutine(DestroyEnemyGameObjectIEnum(instance, bodyTimer));

        WaveSize -= 1;

        if(WaveSize == 0)
        {
            Debug.Log("ROUND OVER");
            SetupNewRound();
        }    
    }

    IEnumerator DestroyEnemyGameObjectIEnum(EnemyClass instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Destroy(instance.gameObject);
        Debug.Log($"{gameObject.name} has been removed.");
    }

    private void SpawnWave()
    {
        while (WaveStrength > 0)
        {
            if(SpawnPoints.Length == 0)
            {
                SpawnEnemyAtDefault();
            } else
            {
                SpawnEnemyAtSpawnPoint();
            }

            WaveSize += 1;
            WaveStrength -= 1;
        }
    }

    public void SpawnEnemyAtSpawnPoint()
    {
        GameObject spawnedEnemy = Instantiate(EnemyPrefab, SpawnPoints[Mathf.FloorToInt(Random.Range(0, SpawnPoints.Length))].transform.position, Quaternion.identity);
        Enemies.Add(spawnedEnemy.GetComponent<DeathCube>());
    }

    public void SpawnEnemyAtDefault()
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

    public void DamagePlayer(int amount)
    {
        Player.Damage(amount);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
