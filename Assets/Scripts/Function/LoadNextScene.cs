using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public void ButtonNextLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void ButtonExitGame()
    {
        Application.Quit();
    }
}
