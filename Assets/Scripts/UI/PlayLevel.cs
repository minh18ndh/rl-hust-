using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class PlayLevel : MonoBehaviour
{

    public void goToSelectLvl()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void goToTut()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayLvl1()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void PlayLvl2()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
