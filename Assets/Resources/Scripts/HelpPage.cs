using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpPage : AUIPage {

    public HelpMenu helpMenu;
    public Button prevButton;
    public Button nextButton;

    public override void Start() {
        base.Start();
        if (helpMenu == null) Debug.LogWarning("No HelpMenu found in parent!");
    }

    public override void OnOpen() {
        base.OnOpen();
        if (helpMenu.currentPage == 0) prevButton.gameObject.SetActive(false);
        //if (helpMenu.currentPage == helpMenu.helpPages.pages.Count - 1) nextButton.gameObject.SetActive(false);
    }

    public void Prev() {
        helpMenu.Prev();
    }

    public void Next() {
        helpMenu.Next();
    }
}
