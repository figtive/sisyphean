using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIController))]
public class HelpMenu : AUIPage {

    public UIController helpPages;
    public int currentPage;

    public override void Start() {
        base.Start();
        currentPage = 0;
    }

    public override void OnOpen() {
        currentPage = 0;
        helpPages.Open(currentPage);
    }
    
    public void Back() {
        UIController control = (UIController) transform.parent.GetComponentInParent(typeof(UIController));
        Debug.Log(":: " + control.gameObject.name);
        control.Back();
        if (control.returnIndex == 0) GameManager.instance.PauseGame(false);
    }

    public void Prev() {
        if (currentPage != 0) {
            helpPages.Open(--currentPage);
        }
    }

    public void Next() {
        if (currentPage != helpPages.pages.Count - 1) {
            helpPages.Open(++currentPage);
        } else {
            Back();
        }

    }
}
