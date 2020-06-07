using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinMenu : AUIPage {

	public TextMeshProUGUI scoreText;

	public void Awake() {
		scoreText.SetText(GameManager.instance.score.ToString());
	}

    public void Next() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.NextGame();
    }

	public void Retry() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.ReplayGame();
    }

    public void ToMenu() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.ToMenu();
    }

}
