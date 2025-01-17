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
    public string jsonFilePath = "Data/leaderboard.json";
    private PlayerRecordList data;
    public TextRecord[] records;

    void Start()
    {
        jsonFilePath = Path.Combine(Application.streamingAssetsPath, jsonFilePath);
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

    public bool isNewRecord(string time)
    {
        bool result = false;
        for(int i = 0;i < data.records.Count;i++)
        {
            PlayerRecord record = data.records[i];
            if(isTakeMoreTime(record.time, time))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public void UpdateData(string name, string time)
    {
        for (int i = 0; i < data.records.Count; i++)
        {
            PlayerRecord record = data.records[i];
            if (isTakeMoreTime(record.time, time))
            {
                record.time = time;
                record.name = name;
                break;
            }
        }

        SaveDataToJson(jsonFilePath, data);
    }

    private bool isTakeMoreTime(string time1, string time2)
    {
        int timeInMillis1 = ConvertTimeToMilliseconds(time1);
        int timeInMillis2 = ConvertTimeToMilliseconds(time2);

        return timeInMillis1 > timeInMillis2;
    }

    private int ConvertTimeToMilliseconds(string timeString)
    {
        var timeParts = timeString.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);
        int centiseconds = int.Parse(timeParts[2]);

        return (minutes * 60 * 1000) + (seconds * 1000) + (centiseconds * 10);
    }
}
