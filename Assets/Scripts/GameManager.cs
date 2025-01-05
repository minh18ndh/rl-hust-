using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int NumCheckpoints;
    private float rewards;
    public bool isCheckpointPassed;
    private float finalTime;

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

    private void Start()
    {
        tmScript = UIManager.GetComponent<TimerController>();
        isCheckpointPassed = false;

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
            rewards += 5;
            Debug.Log("Correct answer! Rewards: " + rewards);
        }
        else
        {
            rewards -= 5;
            Debug.Log("Wrong answer! Rewards: " + rewards);
        }

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
    }

    private void EndGame()
    {
        tmScript.SetPauseState(true);
        Time.timeScale = 0f;

        endGameText.text = FinalTime();
        endGameScreen.SetActive(true);

        Debug.Log("Finish line reached!");
    }

    private string FinalTime()
    {
        finalTime = tmScript.timer - rewards;

        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        int centiseconds = Mathf.FloorToInt((finalTime * 100) % 100);

        return $"Your final time is {minutes:00}:{seconds:00}:{centiseconds:00}";
    }
}
