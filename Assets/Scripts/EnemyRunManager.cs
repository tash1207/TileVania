using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyRunManager : MonoBehaviour
{
    public static EnemyRunManager instance;

    int totalEnemies = 7;
    int enemiesKilled = 0;

    [SerializeField] AudioClip toggleModeSFX;
    [SerializeField] GameObject enemyCountUI;
    [SerializeField] TextMeshProUGUI enemiesKilledText;

    bool isEnemyRun = false;
    bool enemyRunSucceeded = false;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ToggleEnemyRun() {
        SoundFXManager.instance.PlaySoundFXClip(toggleModeSFX, 1f);
        isEnemyRun = !isEnemyRun;
        
        if (isEnemyRun)
        {
            StartSceneManager.instance.ToggleEnemyRunUIOn();
            enemiesKilled = 0;
            enemiesKilledText.text = enemiesKilled + "/" + totalEnemies;
            EnableEnemiesKilledUI();
        }
        else
        {
            StartSceneManager.instance.ToggleEnemyRunUIOff();
            DisableEnemiesKilledUI();
        }

        FindObjectOfType<GameSession>().ResetLivesAndScore();
    }

    public void EnableEnemiesKilledUI()
    {
        enemyCountUI.SetActive(true);
    }

    public void DisableEnemiesKilledUI()
    {
        enemyCountUI.SetActive(false);
    }

    public bool IsEnemyRun()
    {
        return isEnemyRun;
    }

    public void SetEnemyRun(bool value)
    {
        isEnemyRun = value;
        enemyRunSucceeded = false;
    }

    public void ResetEnemyRun()
    {
        isEnemyRun = false;
        enemyRunSucceeded = false;
        enemiesKilled = 0;
    }

    public void KillEnemy()
    {
        if (isEnemyRun)
        {
            enemiesKilled++;
            enemiesKilledText.text = enemiesKilled + "/" + totalEnemies;
            
            if (enemiesKilled == totalEnemies) {
                enemyRunSucceeded = true;
            }
        }
    }

    public bool IsSuccess()
    {
        return enemyRunSucceeded;
    }
}
