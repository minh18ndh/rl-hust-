using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerRecord
{
    public string name;
    public string time;
}

[System.Serializable]
public class PlayerRecordList
{
    public List<PlayerRecord> records;
}

public class LeaderboardLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/Data/leaderboard.json";
    private PlayerRecordList data;
    public TextRecord[] records;

    void Start()
    {
        data = LoadDataFromJson(jsonFilePath);
        int i = 0;
        foreach (var record in records)
        {
            record.playerName.text = data.records[i].name;
            record.time.text = data.records[i].time;
            i++;
        }
    }

    PlayerRecordList LoadDataFromJson(string path)
    {
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<PlayerRecordList>(json);
    }

    void SaveDataToJson(string path, PlayerRecordList data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public void UpdateData(string name, DateTime time)
    {
        SaveDataToJson(jsonFilePath, data);
    }
}
