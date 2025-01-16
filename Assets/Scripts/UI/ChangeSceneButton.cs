using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private GameObject bot1;
    [SerializeField] private GameObject bot2;

    public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void loadMainMenuScene()
    {
        bot1.SetActive(false);
        bot2.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main-Menu");
    }
}
