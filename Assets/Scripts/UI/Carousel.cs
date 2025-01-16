using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carousel : MonoBehaviour
{
    private int currentPage = 0;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void NextPage()
    {
        if (currentPage < 3)
        {
            currentPage++;
            rect.transform.LeanMoveLocalX(rect.transform.localPosition.x + -rect.rect.width / 4, 1f).setEase(LeanTweenType.easeOutCubic);

        } 
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            rect.transform.LeanMoveLocalX(rect.transform.localPosition.x + rect.rect.width / 4, 1f).setEase(LeanTweenType.easeOutCubic);
        }
    }
}
