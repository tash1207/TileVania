using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRunTrigger : MonoBehaviour
{
    bool coinRunToggled = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (!coinRunToggled && other.tag == "Player")
        {
            coinRunToggled = true;
            CoinRunManager.instance.ToggleCoinRun();
            StartCoroutine(CoinRunToggleBuffer());
        }
    }

    IEnumerator CoinRunToggleBuffer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        coinRunToggled = false;
    }
}
