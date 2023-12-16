using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Function to start the game.
    public void Play()
    {
         // This will load the first level scene when the "Play" button is clicked.
    
        SceneManager.LoadScene("Level_1");
    }
  
   // Function to replay the game.
    public void replayGame()
    {
        // This will load the first level scene when the "Replay" button is clicked.
        SceneManager.LoadScene("Level_1");
    }

    // Function to Exit the game.
    public void Exit()
    {
        // This will Quit the application when the "Exit" button is clicked.
        Application.Quit();        
    }
    
    // Function to open the GitHub repository
     public void OpenGitHubRepo()
    {
         // Open the specified URL in the default web browser.
        Application.OpenURL("https://github.com/PrathamRaina/FINAL_VERSION_GAME");
    }

   
}

