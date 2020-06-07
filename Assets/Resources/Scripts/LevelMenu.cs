using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelMenu : AUIPage {

    public TextMeshProUGUI hsLv1;
    public TextMeshProUGUI hsLv2;
    public TextMeshProUGUI hsLv3;

    override public void Start() {
        hsLv1.SetText("Highscore: " + PlayerPrefs.GetInt("HiScore0"));
        hsLv2.SetText("Highscore: " + PlayerPrefs.GetInt("HiScore1"));
        hsLv3.SetText("Highscore: " + PlayerPrefs.GetInt("HiScore2"));
    }
	public void PlayGame(int levelNum) {
        AppManager.instance.BeginGame(levelNum);
    }

    public void ToMenu() {
        uiController.Open(0);
    }
}
