using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinRunManager : MonoBehaviour
{
    public static CoinRunManager instance;

    int totalCoins = 13;
    int coinsCollected = 0;

    [SerializeField] AudioClip toggleModeSFX;
    [SerializeField] GameObject coinCountUI;
    [SerializeField] TextMeshProUGUI coinsCollectedText;

    bool isCoinRun = false;
    bool coinRunSucceeded = false;

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ToggleCoinRun() {
        SoundFXManager.instance.PlaySoundFXClip(toggleModeSFX, 1f);
        isCoinRun = !isCoinRun;
        
        if (isCoinRun)
        {
            StartSceneManager.instance.ToggleCoinRunUIOn();
            coinsCollected = 0;
            coinsCollectedText.text = coinsCollected + "/" + totalCoins;
            EnableCoinsCollectedUI();
        }
        else
        {
            StartSceneManager.instance.ToggleCoinRunUIOff();
            DisableCoinsCollectedUI();
        }

        FindObjectOfType<GameSession>().ResetLivesAndScore();
    }

    public void EnableCoinsCollectedUI()
    {
        coinCountUI.SetActive(true);
    }

    public void DisableCoinsCollectedUI()
    {
        coinCountUI.SetActive(false);
    }

    public bool IsCoinRun()
    {
        return isCoinRun;
    }

    public void SetCoinRun(bool value)
    {
        isCoinRun = value;
    }

    public void CollectCoin()
    {
        if (isCoinRun)
        {
            coinsCollected++;
            coinsCollectedText.text = coinsCollected + "/" + totalCoins;
            
            if (coinsCollected == totalCoins) {
                coinRunSucceeded = true;
            }
        }
    }

    public bool IsSuccess()
    {
        return coinRunSucceeded;
    }
}
