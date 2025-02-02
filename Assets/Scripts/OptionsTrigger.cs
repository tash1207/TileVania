using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsTrigger : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            optionsMenu.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            optionsMenu.SetActive(false);
        }
    }
}
