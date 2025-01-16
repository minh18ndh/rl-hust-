using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSlide : MonoBehaviour
{
    [SerializeField] private CanvasGroup foreground;

    public void slide()
    {
        transform.LeanMoveLocalX(-transform.position.x, 2f);
        foreground.LeanAlpha(0f, 1f);
        StartCoroutine("changeScene");
        //SceneManager.LoadScene("Level1");
    }
    
    public IEnumerator changeScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level1");
    }
}
