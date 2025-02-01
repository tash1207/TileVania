using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedRun : MonoBehaviour
{
    public static SpeedRun instance;

    [SerializeField] AudioClip toggleModeSFX;
    [SerializeField] GameObject timerUI;
    [SerializeField] TextMeshProUGUI timeLeftText;

    bool isSpeedRun = false;
    bool timerOn = false;
    bool speedRunRanOutOfTime = false;
    bool speedRunSucceeded = false;
    float speedRunTime = 42f;
    float timeLeft;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ToggleSpeedRun() {
        SoundFXManager.instance.PlaySoundFXClip(toggleModeSFX, Camera.main.transform, 1f);
        isSpeedRun = !isSpeedRun;
        if (isSpeedRun)
        {
            timerUI.SetActive(true);
            StartSceneManager.instance.ToggleSpeedRunUIOn();
        }
        else
        {
            timerUI.SetActive(false);
            StartSceneManager.instance.ToggleSpeedRunUIOff();
        }

        speedRunRanOutOfTime = false;
    }

    void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerOn = false;
                speedRunRanOutOfTime = true;
                FindObjectOfType<PlayerMovement>().Death();
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        timeLeftText.text = ":" + string.Format("{0:00}", currentTime);
    }

    public bool IsSpeedRun()
    {
        return isSpeedRun;
    }

    public void PauseSpeedRun()
    {
        if (isSpeedRun)
        {
            timerOn = false;
        }
    }

    public void ResumeSpeedRun()
    {
        if (isSpeedRun && !speedRunRanOutOfTime && !speedRunSucceeded)
        {
            timerOn = true;
        }
    }

    public void StartTimer()
    {
        if (isSpeedRun && !timerOn && !speedRunRanOutOfTime)
        {
            timerOn = true;
            timeLeft = speedRunTime;
            timeLeftText.color = Color.white;
        }
    }

    public void Success()
    {
        timerOn = false;
        speedRunSucceeded = true;
        timerUI.SetActive(false);
    }

    public bool IsSuccess()
    {
        return speedRunSucceeded;
    }
}
