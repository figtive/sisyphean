using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    //public GameObject[] pagesObj;
    public List<AUIPage> pages;
    public ALoadingPage loadingPage;
    public int currentPage;
    public int returnIndex;
    [HideInInspector] public bool isLoading;
	
    void Awake() {
        AppManager.instance.currentUIController = this;
    }

    void Start() {
        //pages = new List<AUIPage>();
        //foreach (GameObject page in pagesObj) {
        //    AUIPage iPage = (AUIPage) page.GetComponent(typeof(AUIPage));
        //    if (iPage != null) pages.Add(iPage);
        //    else Debug.LogWarning("[UIController] " + page.name + " does not contain object with IUIPage interface!");
        //}

        if (pages.Count == 0) Debug.LogError("[UIController] No pages specified!");
        for (int p = 0; p < pages.Count; p++) pages[p].Show(p == currentPage);
        isLoading = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
    }

	public void Open(int pageNum) {
        if (currentPage != pageNum) {
            try {
                pages[currentPage].Show(false);
                pages[pageNum].Show(true);
                //prevPage = currentPage;
                currentPage = pageNum;
                Debug.Log("opening " + pageNum);
            } catch (System.ArgumentOutOfRangeException) {
                Debug.LogError("No page of index " + pageNum + " in UIController!");
            }
        }
    }

    public void Open(int pageNum, int returnIndex) {
        if (currentPage != pageNum) {
            try {
                pages[currentPage].Show(false);
                pages[pageNum].Show(true);
                this.returnIndex = returnIndex;
                currentPage = pageNum;
            } catch (System.ArgumentOutOfRangeException) {
                Debug.LogError("No page of index " + pageNum + " in UIController!");
            }
        }
    }

    public void Back() {
        Open(returnIndex);
        Debug.Log(gameObject.name + " returning to " + returnIndex);
    }

    public void Loading(bool state, float progress) {
        if (isLoading != state) {
            pages[currentPage].Show(!state);
            loadingPage.Show(state);
            isLoading = state;
        }
        SetProgress(progress);
    }

    public void SetProgress(float progress) {
        loadingPage.SetProgress(progress);
    }
}

public abstract class AUIPage : MonoBehaviour {
    [HideInInspector] public UIController uiController;
    public virtual void Start() {
        uiController = GetComponentInParent<UIController>();
        if (uiController == null) Debug.LogWarning("No UIController found in parent!");
    }
    public void Show(bool state) {
        if (state && !gameObject.activeSelf) OnOpen();
        if (!state && gameObject.activeSelf) OnClose();
        gameObject.SetActive(state);
    }
    public virtual void OnOpen() { }
    public virtual void OnClose() { }
}

public abstract class ALoadingPage : AUIPage {
    public Slider loadingBar;
    void Start() {
        //loadingBar = GetComponentInChildren<Slider>();
    }
    public virtual void SetProgress(float progress) { loadingBar.value = progress; }
}
