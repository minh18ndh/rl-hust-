using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int NumCheckpoints;
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

    private void Start()
    {
        tmScript = UIManager.GetComponent<TimerController>();
        isCheckpointPassed = false;
    }

    public void CheckpointReached()
    {
        NumCheckpoints--;
        NumCheckpoints = Mathf.Max(0, NumCheckpoints);

        // Pause the timer
        tmScript.SetPauseState(true);
        Debug.Log("Checkpoint reached!");

        // Freeze the game
        Time.timeScale = 0f;

        ShowQuestion("What is 2 + 2?", "4"); // Example question and answer
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
            CheckpointPassed();
        }
        else
        {
            rewards -= 5;
            CheckpointPassed();
        }
    }

    private void CheckpointPassed()
    {
        questionScreen.SetActive(false);

        // Resume the timer
        tmScript.SetPauseState(false);
        Debug.Log(rewards);

        // Continue the game
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
        // Pause the timer
        tmScript.SetPauseState(true);
        Debug.Log("Finish line reached!");

        // Freeze the game
        Time.timeScale = 0f;

        endGameText.text = FinalTime();
        endGameScreen.SetActive(true);
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
