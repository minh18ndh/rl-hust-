using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int NumCheckpoints;
    private float rewards;
    public bool isCheckpointPassed;
    private float playerFinalTime;
    private float bot1FinalTime;
    private float bot2FinalTime;

    [SerializeField] private GameObject UIManager;
    private TimerController tmScript;

    [SerializeField] private GameObject questionScreen;
    [SerializeField] private TextMeshProUGUI questionText;
    private string correctAnswer;
    [SerializeField] private TMP_InputField answerInput;

    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private TextMeshProUGUI endGameText;

    [SerializeField] private List<string> questions;
    [SerializeField] private List<string> answers;

    private int currentCheckpointIndex = 0;
    private bool bot1Finished;
    private bool bot2Finished;

    private void Start()
    {
        tmScript = UIManager.GetComponent<TimerController>();
        isCheckpointPassed = false;
        bot1Finished = false;
        bot2Finished = false;

        if (questions.Count != answers.Count)
        {
            Debug.LogError("Questions and answers lists must have the same length.");
        }
    }

    public void CheckpointReached()
    {
        if (currentCheckpointIndex < questions.Count)
        {
            ShowQuestion(questions[currentCheckpointIndex], answers[currentCheckpointIndex]);
            currentCheckpointIndex++;
        }
        else
        {
            Debug.LogWarning("No more questions available for this checkpoint.");
        }

        NumCheckpoints--;
        NumCheckpoints = Mathf.Max(0, NumCheckpoints);
        tmScript.SetPauseState(true);
        Time.timeScale = 0f;

        Debug.Log("Checkpoint reached!");
    }

    private void ShowQuestion(string question, string answer)
    {
        correctAnswer = answer;
        questionText.text = question;
        questionScreen.SetActive(true);
    }

    public void SubmitAnswer()
    {
        string userAnswer = answerInput.text.Trim();

        if (userAnswer.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            rewards += 2;
            Debug.Log("Correct answer! Rewards: " + rewards);
        }
        else
        {
            rewards -= 2;
            Debug.Log("Wrong answer! Rewards: " + rewards);
        }

        answerInput.text = string.Empty;
        CheckpointPassed();
    }

    private void CheckpointPassed()
    {
        questionScreen.SetActive(false);
        tmScript.SetPauseState(false);
        Time.timeScale = 1f;

        isCheckpointPassed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && NumCheckpoints == 0)
        {
            EndGame();
        }

        if (other.CompareTag("Bot_v1") && !bot1Finished)
        {
            bot1FinalTime = tmScript.timer;
            Debug.Log("Bot_v1 time: " + bot1FinalTime);
            bot1Finished = true;
        }

        if (other.CompareTag("Bot_v2") && !bot2Finished)
        {
            bot2FinalTime = tmScript.timer;
            Debug.Log("Bot_v2 time: " + bot2FinalTime);
            bot2Finished = true;
        }
    }

    private void EndGame()
    {
        tmScript.SetPauseState(true);
        Time.timeScale = 0f;

        endGameText.text = Bot1FinalTime() + Bot2FinalTime() + PlayerFinalTime();
        endGameScreen.SetActive(true);

        Debug.Log("Finish line reached!");
    }

    private string Bot1FinalTime()
    {
        int bot1Minutes = Mathf.FloorToInt(bot1FinalTime / 60);
        int bot1Seconds = Mathf.FloorToInt(bot1FinalTime % 60);
        int bot1Centiseconds = Mathf.FloorToInt((bot1FinalTime * 100) % 100);

        return $"Bot_v1 time is {bot1Minutes:00}:{bot1Seconds:00}:{bot1Centiseconds:00}";
    }

    private string Bot2FinalTime()
    {
        int bot2Minutes = Mathf.FloorToInt(bot2FinalTime / 60);
        int bot2Seconds = Mathf.FloorToInt(bot2FinalTime % 60);
        int bot2Centiseconds = Mathf.FloorToInt((bot2FinalTime * 100) % 100);

        return $"\n\nBot_v2 time is {bot2Minutes:00}:{bot2Seconds:00}:{bot2Centiseconds:00}";
    }

    private string PlayerFinalTime()
    {
        playerFinalTime = tmScript.timer - rewards;

        int playerMinutes = Mathf.FloorToInt(playerFinalTime / 60);
        int playerSeconds = Mathf.FloorToInt(playerFinalTime % 60);
        int playerCentiseconds = Mathf.FloorToInt((playerFinalTime * 100) % 100);

        return $"\n\nYour final time is {playerMinutes:00}:{playerSeconds:00}:{playerCentiseconds:00}";
    }
}
