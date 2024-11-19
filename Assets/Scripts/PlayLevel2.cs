using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayLevel2 : MonoBehaviour
{
    public void PlayLvl2()
    {
        SceneManager.LoadSceneAsync(3);
    }
}
