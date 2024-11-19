using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
class PlayLevel1 : MonoBehaviour
{
    public void PlayLvl1()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
