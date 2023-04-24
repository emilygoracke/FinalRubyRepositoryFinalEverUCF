using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Winner : MonoBehaviour
{
    public void Setup(int score)

    {
        gameObject.SetActive(true);

    }

    public void WinBkL1()
    {

        SceneManager.LoadScene("Main.2");

    }
    public void WinBkL2()
    {
        SceneManager.LoadScene("Scene2");
    }
}
