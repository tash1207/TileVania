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
            timeLeftText.text = ":" + string.Format("{0:00}", speedRunTime);
            timerUI.SetActive(true);
            StartSceneManager.instance.ToggleSpeedRunUIOn();
        }
        else
        {
            timerUI.SetActive(false);
            StartSceneManager.instance.ToggleSpeedRunUIOff();
        }
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
