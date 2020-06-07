using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : AUIPage {

    public void Resume() {
        GameManager.instance.PauseGame(false);
        uiController.Open(0);
    }

    public void Help() {
        uiController.Open(2, 1);
    }

    public void ToMenu() {
        GameManager.instance.PauseGame(false);
        AppManager.instance.ToMenu();
    }

    public void Exit() {
        AppManager.instance.ExitGame();
    }
}
