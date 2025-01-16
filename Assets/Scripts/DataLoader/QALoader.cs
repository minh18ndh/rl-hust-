using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestionAnswer
{
    public string q;
    public string a;
}

[System.Serializable]
public class QuestionAnswerList
{
    public List<QuestionAnswer> questions;
}

public class QALoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/Data/questions.json";
    private QuestionAnswerList data;
    public InputQA[] inputs;

    void Start()
    {
        data = LoadDataFromJson(jsonFilePath);
        int i = 0;
        foreach (var input in inputs)
        {
            input.q.text = data.questions[i].q;
            input.a.text = data.questions[i].a;
            i++;
        }
    }

    QuestionAnswerList LoadDataFromJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<QuestionAnswerList>(json);
    }

    void SaveDataToJson(string path, QuestionAnswerList data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void UpdateData(int index)
    {
        data.questions[index].q = inputs[index].q.text;
        data.questions[index].a = inputs[index].a.text;
        SaveDataToJson(jsonFilePath, data);
    }
}
