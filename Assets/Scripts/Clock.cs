using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    bool speedRunToggled = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (!speedRunToggled && other.tag == "Player")
        {
            speedRunToggled = true;
            SpeedRun.instance.ToggleSpeedRun();
            StartCoroutine(SpeedRunToggleBuffer());
        }
    }

    IEnumerator SpeedRunToggleBuffer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        speedRunToggled = false;
    }
}
