using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideWidget : MonoBehaviour
{
    [SerializeField] private GameObject openMenuButton;
    public void openWidget()
    {
        openMenuButton.SetActive(false);
        transform.localPosition = new Vector3(0f, 0f, 0f);
        Time.timeScale = 0f;
    }

    public void closeWidget() {
        openMenuButton.SetActive(true);
        transform.localPosition = new Vector3(-500f, 0f, 0f);
        Time.timeScale = 1f;
    }
}
