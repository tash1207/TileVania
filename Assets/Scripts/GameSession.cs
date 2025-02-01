using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int startingPlayerLives = 3;
    [SerializeField] int startingTotalScore = 0;

    int playerLives;
    int totalScore;

    [SerializeField] TextMeshProUGUI numLivesText;
    [SerializeField] TextMeshProUGUI numScoreText;

    float levelLoadDelay = 1.3f;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        ResetLivesAndScore();
    }

    public void ResetLivesAndScore()
    {
        playerLives = startingPlayerLives;
        totalScore = startingTotalScore;

        numLivesText.text = playerLives.ToString();
        numScoreText.text = totalScore.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        totalScore += pointsToAdd;
        numScoreText.text = totalScore.ToString();  
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1 && !SpeedRun.instance.IsSpeedRun())
        {
            TakeLife();
        }
        else
        {
            StartCoroutine(ResetGameSession());
        }
    }

    void TakeLife()
    {
        playerLives--;
        numLivesText.text = playerLives.ToString();
        StartCoroutine(LoadCurrentLevel());
    }

    IEnumerator LoadCurrentLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator ResetGameSession()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
