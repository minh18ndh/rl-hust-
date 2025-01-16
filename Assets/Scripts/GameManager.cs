using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int NumCheckpoints;
    private float rewards;
    [HideInInspector] public bool isCheckpointPassed;
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
    [SerializeField] private TextMeshProUGUI aiText;
    [SerializeField] private TextMeshProUGUI correctAnsText;
    [SerializeField] private TextMeshProUGUI currentCheckpointText;


    [SerializeField] private LeaderboardLoader leaderboardLoader;
    [SerializeField] private GameObject savePointWidget;
    [SerializeField] private TMP_InputField nameText;

    private List<string> questions;
    private List<string> answers;

    [SerializeField] private QALoader qaLoader;

    private int currentCheckpointIndex = 0;
    private int correctAnswerNum = 0;
    private bool bot1Finished;
    private bool bot2Finished;

    private void Start()
    {
        tmScript = UIManager.GetComponent<TimerController>();
        isCheckpointPassed = false;
        bot1Finished = false;
        bot2Finished = false;

        List<QuestionAnswer> qa = qaLoader.get5RandomQA();
        questions = new List<string>();
        answers = new List<string>();
        for (int i = 0; i < qa.Count; i++)
        {
            questions.Add(qa[i].q);
            answers.Add(qa[i].a);
        }
    }

    public void CheckpointReached()
    {
        if (currentCheckpointIndex < questions.Count)
        {
            ShowQuestion(questions[currentCheckpointIndex], answers[currentCheckpointIndex]);
            currentCheckpointIndex++;
            currentCheckpointText.text = "Checkpoints: " + currentCheckpointIndex.ToString() + "/5";
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
            tmScript.addTime(-5);
            correctAnswerNum++;
            Debug.Log("Correct answer! Rewards: " + rewards);
        }
        else
        {
            rewards -= 2;
            tmScript.addTime(5);
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

        //endGameText.text = Bot1FinalTime() + Bot2FinalTime() + PlayerFinalTime();
        endGameText.text = "Your Final Time: " + PlayerFinalTime();
        aiText.text = "AI Times:\n" + Bot1FinalTime() + "\n" + Bot2FinalTime();
        correctAnsText.text = "Correct Answers: " + correctAnswerNum.ToString() + "/5";
        endGameScreen.SetActive(true);

        Debug.Log("Finish line reached!");
    }

    public void onHighScoreSceneButtonClick()
    {
        if (leaderboardLoader.isNewRecord(convertFinalTime()))
        {
            savePointWidget.SetActive(true);
            endGameScreen.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene("Main-Menu");
        }
    }

    public void onSavePointButtonClick()
    {
        leaderboardLoader.UpdateData(nameText.text, convertFinalTime());
        SceneManager.LoadScene("Main-Menu");
    }

    private string Bot1FinalTime()
    {
        int bot1Minutes = Mathf.FloorToInt(bot1FinalTime / 60);
        int bot1Seconds = Mathf.FloorToInt(bot1FinalTime % 60);
        int bot1Centiseconds = Mathf.FloorToInt((bot1FinalTime * 100) % 100);

        return $"AI 1: {bot1Minutes:00}:{bot1Seconds:00}:{bot1Centiseconds:00}";
    }

    private string Bot2FinalTime()
    {
        int bot2Minutes = Mathf.FloorToInt(bot2FinalTime / 60);
        int bot2Seconds = Mathf.FloorToInt(bot2FinalTime % 60);
        int bot2Centiseconds = Mathf.FloorToInt((bot2FinalTime * 100) % 100);
        
        return $"AI 2: {bot2Minutes:00}:{bot2Seconds:00}:{bot2Centiseconds:00}";
    }

    public string PlayerFinalTime()
    {
        playerFinalTime = tmScript.timer - rewards;

        int playerMinutes = Mathf.FloorToInt(playerFinalTime / 60);
        int playerSeconds = Mathf.FloorToInt(playerFinalTime % 60);
        int playerCentiseconds = Mathf.FloorToInt((playerFinalTime * 100) % 100);

        return $"\n\nYour final time is {playerMinutes:00}:{playerSeconds:00}:{playerCentiseconds:00}";
    }

    public string convertFinalTime()
    {
        playerFinalTime = tmScript.timer - rewards;

        int playerMinutes = Mathf.FloorToInt(playerFinalTime / 60);
        int playerSeconds = Mathf.FloorToInt(playerFinalTime % 60);
        int playerCentiseconds = Mathf.FloorToInt((playerFinalTime * 100) % 100);

        return $"{playerMinutes:00}:{playerSeconds:00}:{playerCentiseconds:00}";
    }
}
