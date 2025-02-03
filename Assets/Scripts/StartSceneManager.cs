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

    float clockAlpha = 0.4f;
    float coinAlpha = 0.3f;

    [Header("Options")]
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider soundFXVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;

    string gameTitleText = "TileVania";
    string gameOverText = "Game Over";
    string winText = "You Win!";
    string achievementText = "Success!";

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // TODO: Add logic for displaying the game over, you win,
        // and achievement achieved title text.
        titleUI.SetActive(true);
        titleText.text = SuccessfullyCompletedRun() ? achievementText : gameTitleText;

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

    void SetVolumeLevels()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 1f);
        soundFXVolumeSlider.value = PlayerPrefs.GetFloat("soundFXVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
    }

    bool SuccessfullyCompletedRun()
    {
        return SpeedRun.instance.IsSuccess() || CoinRunManager.instance.IsSuccess();
    }

    public void ToggleSpeedRunUIOn()
    {
        DisableCoinRun();
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

    void ResetRuns()
    {
        DisableSpeedRun();
        DisableCoinRun();
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
}
