using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public enum StartMenuState
    {
        Start, // When game is just started. This is the default state.
        Win, // When player wins main game.
        GameOver, // When player loses main game.
        RunSuccess, // When player successfully completes a run.
        RunFail, // When player fails a run.
    }

    [SerializeField] int startingPlayerLives = 3;
    [SerializeField] int startingTotalScore = 0;

    int playerLives;
    int totalScore;

    [SerializeField] TextMeshProUGUI numLivesText;
    [SerializeField] TextMeshProUGUI numScoreText;

    float levelLoadDelay = 1.3f;
    StartMenuState startMenuState = StartMenuState.Start;

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
        playerLives = SpeedRun.instance.IsSpeedRun() ? 1 : startingPlayerLives;
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
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            if (SpeedRun.instance.IsSpeedRun() ||
                CoinRunManager.instance.IsCoinRun() ||
                EnemyRunManager.instance.IsEnemyRun())
            {
                SetStartMenuState(StartMenuState.RunFail);
            }
            else
            {
                SetStartMenuState(StartMenuState.GameOver);
            }
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
        ResetAllRuns();
        ResetLivesAndScore();
        SceneManager.LoadScene(0);
    }

    void ResetAllRuns()
    {
        SpeedRun.instance.ResetSpeedRun();
        CoinRunManager.instance.ResetCoinRun();
        EnemyRunManager.instance.ResetEnemyRun();
    }

    public StartMenuState GetStartMenuState()
    {
        return startMenuState;
    }

    public void SetStartMenuState(StartMenuState state)
    {
        startMenuState = state;
    }
}
