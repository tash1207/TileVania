using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    public static StartSceneManager instance;

    [SerializeField] GameObject titleUI;
    [SerializeField] TextMeshProUGUI titleText;

    [Header("Runs")]
    [SerializeField] GameObject runModesIcons;
    [SerializeField] SpriteRenderer clockImage;
    [SerializeField] SpriteRenderer coinImage;
    [SerializeField] SpriteRenderer enemyImage;

    float clockAlpha = 0.4f;
    float coinAlpha = 0.3f;
    float enemyAlpha = 0.35f;

    [Header("Options")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider soundFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    string gameTitleText = "TileVania";
    string gameOverText = "Game Over";
    string winText = "You Win!";
    string achievementText = "Success!";
    string achievementFailText = "Failed";

    float titleTextDelay = 4f;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        SetTitleText();
        titleUI.SetActive(true);

        if (PlayerPrefs.GetInt("Won", 0) != 0)
        {
            runModesIcons.SetActive(true);
        }

        ResetRuns();
        SetVolumeLevels();
    }

    void Update()
    {
    #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C))
        {
            runModesIcons.SetActive(true);
        }
    #endif
    }

    void SetTitleText()
    {
        GameSession.StartMenuState startMenuState = FindObjectOfType<GameSession>().GetStartMenuState();
        switch (startMenuState)
        {
            case GameSession.StartMenuState.Start:
                titleText.text = gameTitleText;
                return;
            case GameSession.StartMenuState.Win:
                titleText.text = winText;
                break;
            case GameSession.StartMenuState.GameOver:
                titleText.text = gameOverText;
                break;
            case GameSession.StartMenuState.RunSuccess:
                titleText.text = achievementText;
                break;
            case GameSession.StartMenuState.RunFail:
                titleText.text = achievementFailText;
                break;
        }
        Invoke("ShowGameTitleText", titleTextDelay);
    }

    void ShowGameTitleText()
    {
        FindObjectOfType<GameSession>().SetStartMenuState(GameSession.StartMenuState.Start);
        titleText.text = gameTitleText;
    }

    void SetVolumeLevels()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        soundFXVolumeSlider.value = PlayerPrefs.GetFloat("soundFXVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
    }

    public void ToggleSpeedRunUIOn()
    {
        DisableCoinRun();
        DisableEnemyRun();
        titleUI.SetActive(false);
        var tempColor = clockImage.color;
        tempColor.a = 1f;
        clockImage.color = tempColor;
    }

    public void ToggleSpeedRunUIOff()
    {
        titleUI.SetActive(true);
        var tempColor = clockImage.color;
        tempColor.a = clockAlpha;
        clockImage.color = tempColor;
    }

    public void ToggleCoinRunUIOn()
    {
        DisableSpeedRun();
        DisableEnemyRun();
        titleUI.SetActive(false);
        var tempColor = coinImage.color;
        tempColor.a = 1f;
        coinImage.color = tempColor;
    }

    public void ToggleCoinRunUIOff()
    {
        titleUI.SetActive(true);
        var tempColor = coinImage.color;
        tempColor.a = coinAlpha;
        coinImage.color = tempColor;
    }

    public void ToggleEnemyRunUIOn()
    {
        DisableSpeedRun();
        DisableCoinRun();
        titleUI.SetActive(false);
        var tempColor = enemyImage.color;
        tempColor.a = 1f;
        enemyImage.color = tempColor;
    }

    public void ToggleEnemyRunUIOff()
    {
        titleUI.SetActive(true);
        var tempColor = enemyImage.color;
        tempColor.a = enemyAlpha;
        enemyImage.color = tempColor;
    }

    void ResetRuns()
    {
        DisableSpeedRun();
        DisableCoinRun();
        DisableEnemyRun();
    }

    // TODO: Consider adding a run type enum.
    void DisableSpeedRun()
    {
        ToggleSpeedRunUIOff();
        SpeedRun.instance.DisableTimerUI();
        SpeedRun.instance.SetSpeedRun(false);
    }

    void DisableCoinRun()
    {
        ToggleCoinRunUIOff();
        CoinRunManager.instance.DisableCoinsCollectedUI();
        CoinRunManager.instance.SetCoinRun(false);
    }

    void DisableEnemyRun()
    {
        ToggleEnemyRunUIOff();
        EnemyRunManager.instance.DisableEnemiesKilledUI();
        EnemyRunManager.instance.SetEnemyRun(false);
    }
}
