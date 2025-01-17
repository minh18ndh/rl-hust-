using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
    public string jsonFilePath = "Data/questions.json";
    private QuestionAnswerList data;
    public InputQA[] inputs;

    void Start()
    {
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
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

    public List<QuestionAnswer> get5RandomQA()
    {
        List<QuestionAnswer> res = new List<QuestionAnswer>();
        int[] randomNumbers = GenerateUniqueRandomNumbers(0, 9, 5);
        foreach (int number in randomNumbers)
        {
            res.Add(data.questions[number]);
        }

        return res;
    }

    int[] GenerateUniqueRandomNumbers(int min, int max, int count)
    {
        List<int> numbers = new List<int>();
        for (int i = min; i <= max; i++)
        {
            numbers.Add(i);
        }

        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, numbers.Count);
            result[i] = numbers[randomIndex];
            numbers.RemoveAt(randomIndex);
        }

        return result;
    }
}
