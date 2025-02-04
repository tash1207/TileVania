using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] AudioClip levelExitSFX;

    bool isPlayingLevelExitSFX = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !isPlayingLevelExitSFX && FindObjectOfType<PlayerMovement>().IsAlive())
        {
            isPlayingLevelExitSFX = true;
            SoundFXManager.instance.PlaySoundFXClip(levelExitSFX, 0.6f);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        SpeedRun.instance.PauseSpeedRun();
        yield return new WaitForSecondsRealtime(levelExitSFX.length);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Player is exiting the last level.
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            PlayerPrefs.SetInt("Won", 1);
            PlayerPrefs.Save();
            SpeedRun.instance.Success();
            SetStartMenuState();
        }

        // Reset score and lives if leaving start menu scene.
        // Also, start any achievement-based tracking such as speed run timer.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<GameSession>().ResetLivesAndScore();
            SpeedRun.instance.StartTimer();
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SpeedRun.instance.ResumeSpeedRun();
        SceneManager.LoadScene(nextSceneIndex);
    }

    void SetStartMenuState()
    {
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.SetStartMenuState(GameSession.StartMenuState.Win);

        if (SpeedRun.instance.IsSuccess() || CoinRunManager.instance.IsSuccess())
        {
            gameSession.SetStartMenuState(GameSession.StartMenuState.RunSuccess);
        }
        else if (CoinRunManager.instance.IsCoinRun() && !CoinRunManager.instance.IsSuccess())
        {
            gameSession.SetStartMenuState(GameSession.StartMenuState.RunFail);
        }
    }
}
