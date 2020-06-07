using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverMenu : AUIPage {

    public void Retry() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.ReplayGame();
    }

    public void Continue() {
        GameManager.instance.PauseGame(false);
        uiController.Open(0);
    }

    public void ToMenu() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.ToMenu();
    }

}
