using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedRun : MonoBehaviour
{
    public static SpeedRun instance;

    [SerializeField] AudioClip toggleModeSFX;
    [SerializeField] AudioClip tickTockSFX;
    [SerializeField] GameObject timerUI;
    [SerializeField] TextMeshProUGUI timeLeftText;

    bool isSpeedRun = false;
    bool speedRunSucceeded = false;

    bool timerOn = false;
    float speedRunTime = 42f;
    float tickTockTime = 3f;
    float timeLeft;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ToggleSpeedRun() {
        SoundFXManager.instance.PlaySoundFXClip(toggleModeSFX, 1f);
        isSpeedRun = !isSpeedRun;

        if (isSpeedRun)
        {
            StartSceneManager.instance.ToggleSpeedRunUIOn();
            timeLeftText.text = ":" + string.Format("{0:00}", speedRunTime);
            EnableTimerUI();
        }
        else
        {
            StartSceneManager.instance.ToggleSpeedRunUIOff();
            DisableTimerUI();
        }

        FindObjectOfType<GameSession>().ResetLivesAndScore();
    }

    public void EnableTimerUI()
    {
        timerUI.SetActive(true);
    }

    public void DisableTimerUI()
    {
        timerUI.SetActive(false);
    }

    void Update()
    {
        if (isSpeedRun && timerOn)
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
                FindObjectOfType<PlayerMovement>().Death();
                isSpeedRun = false;
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        timeLeftText.text = ":" + string.Format("{0:00}", Mathf.Ceil(currentTime));

        if (currentTime < tickTockTime && currentTime > 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(tickTockSFX, 1f);
            tickTockTime -= 1f;
        }
    }

    public bool IsSpeedRun()
    {
        return isSpeedRun;
    }

    public void SetSpeedRun(bool value)
    {
        isSpeedRun = value;
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
        if (isSpeedRun && !speedRunSucceeded)
        {
            timerOn = true;
        }
    }

    public void StartTimer()
    {
        if (isSpeedRun && !timerOn)
        {
            timerOn = true;
            timeLeft = speedRunTime;
            timeLeftText.color = Color.white;
        }
    }

    public void Success()
    {
        if (isSpeedRun)
        {
            timerOn = false;
            speedRunSucceeded = true;
            isSpeedRun = false;
            timerUI.SetActive(false);
        }
    }

    public bool IsSuccess()
    {
        return speedRunSucceeded;
    }
}
