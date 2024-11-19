using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowTutorial : MonoBehaviour
{
    public void ShowTut()
    {
        SceneManager.LoadSceneAsync(7);
    }
}
