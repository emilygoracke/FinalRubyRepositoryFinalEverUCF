using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Opening : MonoBehaviour
{
      public void Setup(int score)

    {
        gameObject.SetActive(true);

    }

    public void Play()
    {
        SceneManager.LoadScene("Main.2");
    }
}
