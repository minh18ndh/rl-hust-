using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timer;   // Timer value in seconds
    private bool isPaused;

    void Update()
    {
        if (!isPaused) // Only update the timer if not paused
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int centiseconds = Mathf.FloorToInt((timer * 100) % 100);

            timerText.text = $"{minutes:00}:{seconds:00}:{centiseconds:00}";
        }
    }

    public void SetPauseState(bool pause)
    {
        isPaused = pause;
    }
}
