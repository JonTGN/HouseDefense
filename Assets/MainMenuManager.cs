using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public void StartButton()
    {
        Debug.Log("Scene Reloaded");
        SceneManager.LoadScene("House");
    }
    public void HowToPlayButton()
    {

    }
    public void OptionsButton()
    {

    }
    public void CreditsButton()
    {


    }
    public void QuitButton()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
