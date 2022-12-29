using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DiedCredits : MonoBehaviour
{
    

    [SerializeField]
    public string diedMessage = "You have perished.";

    public TextMeshProUGUI textbox;
    public GameObject RestartButton;
    public GameObject QuitButton;
    public GameObject FireplaceLights;

    private string msg = "";
    private int counter = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(TextTyping());
    }

    IEnumerator TextTyping()
    {
        msg += diedMessage[counter];
        counter++;

        textbox.text = msg;


        if (counter < diedMessage.Length - 1)
        {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(TextTyping());
        }
        else if (counter < diedMessage.Length)
        {
            yield return new WaitForSeconds(0.85f);
            StartCoroutine(TextTyping());
        }
        else
        {
            ButtonsVisible();
            FireplaceLights.SetActive(false);
        }
    }

    void ButtonsVisible()
    {
        RestartButton.SetActive(true);
        QuitButton.SetActive(true);
    }


    public void RestartGame()
    {
        Debug.Log("Scene Reloaded");
        SceneManager.LoadScene("AI Pathfinding");
    }

    public void MainMenu()
    {
        Debug.Log("Scene Reloaded");
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }



}
