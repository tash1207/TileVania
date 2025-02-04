using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunTrigger : MonoBehaviour
{
    bool enemyRunToggled = false;

    void OnTriggerEnter2D(Collider2D other) {
        if (!enemyRunToggled && other.tag == "Player")
        {
            enemyRunToggled = true;
            EnemyRunManager.instance.ToggleEnemyRun();
            StartCoroutine(EnemyRunToggleBuffer());
        }
    }

    IEnumerator EnemyRunToggleBuffer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        enemyRunToggled = false;
    }
}
