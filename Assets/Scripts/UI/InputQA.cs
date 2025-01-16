using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputQA : MonoBehaviour
{
    public TMP_InputField q, a;

    public void updateQuestion(QuestionAnswer qa)
    {
        q.text = qa.q;
        a.text = qa.a;
    }
}
