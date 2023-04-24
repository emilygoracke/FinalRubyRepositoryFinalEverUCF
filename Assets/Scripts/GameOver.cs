using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public void Setup(int score)

    {
        gameObject.SetActive(true);

    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Scene2");
    }

    public void MainMenuButton()
    {

        SceneManager.LoadScene("Main.2");


    }
    
}