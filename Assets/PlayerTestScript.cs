using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
        //Debug.Log("UpdateHealthLabel() has been called!");
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
    private int health;

    public AudioSource player_hurt;
    public CameraShake cameraShake;
    public float cameraShakeMagnitude;

    protected int Health 
    {
        get => health;
        set
        {
            health = value;

            
            // play audio of gettign hit here 
            //todo: make list of hurt sounds and set clip in audio source
            if (health != 100)
            {
                player_hurt.Play();
                StartCoroutine(cameraShake.Shake(0.1f, cameraShakeMagnitude));
            }
            
            
            HealthChanged?.Invoke(health);
            if(health == 0)
                SceneManager.LoadScene("DeathScene");
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

