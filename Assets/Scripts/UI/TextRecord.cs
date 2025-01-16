using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextRecord : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI time;
    public void updateRecord(PlayerRecord record)
    {
        playerName.text = record.name;
        time.text = record.time;
    }
}
