using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public static StartSceneManager instance;

    [SerializeField] GameObject titleUI;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject clock;
    [SerializeField] SpriteRenderer clockImage;

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
        titleText.text = SpeedRun.instance.IsSuccess() ? achievementText : gameTitleText;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            clock.SetActive(true);
        }
    }

    public void ToggleSpeedRunUIOn()
    {
        titleUI.SetActive(false);
        var tempColor = clockImage.color;
        tempColor.a = 1f;
        clockImage.color = tempColor;
    }

    public void ToggleSpeedRunUIOff()
    {
        titleUI.SetActive(true);
        var tempColor = clockImage.color;
        tempColor.a = 0.5f;
        clockImage.color = tempColor;
    }
}
