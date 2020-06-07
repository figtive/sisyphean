using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour {

    public static AppManager instance = null;

    public GameObject gameManager;
    public GameObject cameraManager;
    public GameObject npcManager;
    public int currentLevel = 0;
    public bool fromStart;
    private bool withSplash;

    [HideInInspector] public UIController currentUIController;
    //public GamePrefs preferences;

    private AsyncOperation sceneAsync;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        fromStart = true;
        InitSavedPrefs();
        InitMenu();
        //preferences = new GamePrefs(0.75f, 0.75f, true);
    }
    
    void InitSavedPrefs() {
        if (!PlayerPrefs.HasKey("MusicVol")) PlayerPrefs.SetFloat("MusicVol", 0.75f);
        if (!PlayerPrefs.HasKey("SFXVol")) PlayerPrefs.SetFloat("SFXVol", 0.75f);
        if (!PlayerPrefs.HasKey("ShowHelp")) PlayerPrefs.SetInt("ShowHelp", 1);
        if (!PlayerPrefs.HasKey("HiScrore1")) PlayerPrefs.SetInt("HiScrore1", 0);
        if (!PlayerPrefs.HasKey("HiScrore2")) PlayerPrefs.SetInt("HiScrore3", 0);
        if (!PlayerPrefs.HasKey("HiScrore3")) PlayerPrefs.SetInt("HiScrore3", 0);
        if (!PlayerPrefs.HasKey("MaxLevel")) PlayerPrefs.SetInt("MaxLevel", 3);
    }

    void InitMenu() {
        //AudioManager.instance.Play(new string[] {"Intro", "Theme");
        AudioManager.instance.Play("Theme");
        StartCoroutine(LoadScene(1));
        withSplash = false;
    }

    void InitMenuSplash() {
        //AudioManager.instance.Play(new string[] {"Intro", "Theme");
        AudioManager.instance.Play("Theme");
        StartCoroutine(LoadScene(1));
        withSplash = true;
    }

    public void BeginGame(int sceneIndex) {
        currentLevel = sceneIndex;
        if (GameManager.instance == null) Instantiate(gameManager);
        if (CameraManager.instance == null) Instantiate(cameraManager);
        if (NPCManager.instance == null) Instantiate(npcManager);
        StartCoroutine(LoadScene(sceneIndex));
    }

    public void NextGame() {
        if(currentLevel != 5) BeginGame(currentLevel + 1);
    }

    public void ReplayGame() {
        BeginGame(currentLevel);
    }

    public void ToMenu() {
        GameManager.instance.EndGame();
        InitMenu();
    }

    public void ExitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    IEnumerator LoadScene(int index) {
        if(index != 1) AudioManager.instance.Stop("Theme");
        AsyncOperation scene = SceneManager.LoadSceneAsync(index);
        float progress = 0f;
        while (!scene.isDone) {
            if (currentUIController != null) {
                progress = Mathf.Lerp(progress, Mathf.Clamp01(scene.progress / 0.9f), 0.75f);
                currentUIController.Loading(true, progress);
                Debug.Log("[SceneLoader] Loading " + SceneManager.GetSceneByBuildIndex(index).name + "  ::  Progress: " + progress);
            }

            yield return null;
        }
        Debug.Log("[SceneLoader] " + SceneManager.GetSceneByBuildIndex(index).name + " Loaded!");
        AudioManager.instance.Play("Theme");
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex >= 2) {
            Debug.Log("[SceneLoader] Initializing GameManager");
            GameManager.instance.InitGame();
            //SceneManager.MoveGameObjectToScene(gameManager, scene);
            //SceneManager.MoveGameObjectToScene(cameraManager, scene);
            //SceneManager.MoveGameObjectToScene(npcManager, scene);
        }
        //if (scene.buildIndex == 1) {
        //    FindObjectOfType<UIController>().Open(0);
        //}
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public void SetFromStart(bool start) {
        fromStart = start;
    }
}

public class GamePrefs {
    public float musicVol;
    public float sfxVol;
    public bool showHelp;

    public GamePrefs(float musicVol, float sfxVol, bool showHelp) {
        this.musicVol = musicVol;
        this.sfxVol = sfxVol;
        this.showHelp = showHelp;
    }
}
