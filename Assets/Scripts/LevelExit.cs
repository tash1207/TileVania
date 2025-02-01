using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] AudioClip levelExitSFX;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            SoundFXManager.instance.PlaySoundFXClip(levelExitSFX, Camera.main.transform, 0.6f);
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        SpeedRun.instance.PauseSpeedRun();
        yield return new WaitForSecondsRealtime(levelExitSFX.length);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            SpeedRun.instance.Success();
        }

        // Reset score and lives if leaving start menu scene.
        // Also, start any achievement-based tracking such as speed run timer.
        // TODO: Consider pausing speed run timer during next level load.
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            FindObjectOfType<GameSession>().ResetLivesAndScore();
            SpeedRun.instance.StartTimer();
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SpeedRun.instance.ResumeSpeedRun();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
