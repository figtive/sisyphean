using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    
    public PlayerControllerThird player;
    public BinController trashBin;
    public NPCController[] npcs;
    public UIController uiController;
    public PopUpController popUpController;
    public GameUI gameUI;
    public Canvas canvas;
    public MediaShare media;

    public bool inGame = false;
    public bool isPaused = false;
    public EndState? endState;

    public List<TrashType> trashCollected;
    public float charisma;      // [0, 1]
    public int score;
    public float time, timeInitial, timeMaximum;
    public bool hasTrash;
    private Toggle hostileToggle;
    private bool gameOverNoticed = false;    //TESTING

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Update() {
        if (inGame) {
            if (NPCManager.instance.AllDehostiled()) { if (!gameOverNoticed) { OverGame(EndState.Win); gameOverNoticed = true; } }
            if (time <= 0) { if (!gameOverNoticed) { OverGame(EndState.Lose); gameOverNoticed = true; } }     // TRIGGER SOMETHING
            else { time -= Time.deltaTime; gameOverNoticed = false; }
            if (!isPaused) {
                gameUI.ShowScore(score);
                gameUI.ShowTime(time);
                hasTrash = player.HasTrash();
            }
        }
    }


    public void InitGame() {
        SceneSocket sceneData = FindObjectOfType<SceneSocket>();

        // INITIALIZE MANAGERS
        NPCManager.instance.InitNPCs();
        CameraManager.instance.InitCam();

        // INITIALIZE GAME ENTIITES
        player = sceneData.player;
        media = sceneData.media;
        trashBin = sceneData.trashCan;
        canvas = sceneData.canvas;
        uiController = sceneData.uiController;
        popUpController = sceneData.popUpController;
        gameUI = (GameUI) uiController.pages[0].GetComponent(typeof(GameUI));
        CameraManager.instance.AttachCam(sceneData.cam);
        NPCManager.instance.AttachNPCs(sceneData.npcs);
        NPCManager.instance.SetAllHostile(true);
        CameraManager.instance.Add(player.transform);

        // INITIALIZE STATS
        trashCollected = new List<TrashType>();
        score = GamePoint.Score.initial;
        time = sceneData.timeStart; timeInitial = sceneData.timeStart; timeMaximum = sceneData.timeMaximum;
        charisma = GamePoint.Charisma.initial;

        // TESTING
        hostileToggle = sceneData.hostileToggle;

        Debug.Log("GameManager Loaded!");
        inGame = true;
        isPaused = false;
        endState = null;
        if (PlayerPrefs.GetInt("ShowHelp") != 0) {
            instance.PauseGame(true);
            uiController.Open(2, 0);
        } else instance.PauseGame(false);
    }

    public void EndGame() {
        inGame = false;
    }

    public void PauseGame(bool pause) {
        if (inGame) {
            this.isPaused = pause;
            AudioManager.instance.Pause("Theme", pause);
            if (pause) {
                Time.timeScale = 0f;
            }
            else Time.timeScale = 1f;
        }
    }

    public void OverGame(EndState state) {
        Debug.Log("GAME OVER!");
        if (PlayerPrefs.GetInt("HiScore" + (AppManager.instance.currentLevel - 3)) < score) PlayerPrefs.SetInt("HiScore" + (AppManager.instance.currentLevel - 3), score);
        endState = state;
        gameUI.AddLog(string.Format("<color=yellow><b>Game over</b>\t({0})</color>\t\t\t(S: {1}, C: {2:f3})", state, score, charisma));
        PauseGame(true);
        if (state == EndState.Lose) { uiController.Open(3); AudioManager.instance.Play("Game_Lost"); }
        if (state == EndState.Win) { uiController.Open(4); AudioManager.instance.Play("Game_Win");
    }
}


    public void DisposeTrash(TrashItem trash) {
        AudioManager.instance.Play("Trash_Success");
        int additionalScore = GamePoint.Score.GetScore(trash.type) + (trash.byNPC ? GamePoint.Score.npcBonus : 0);
        float additionalTime = GamePoint.Time.GetTime(trash.type);
        AddScore(additionalScore);
        AddTime(additionalTime);
        AddCharisma(GamePoint.Charisma.GetCharisma(trash.type));
        if (trashCollected.Count == 0) media.TakeHiResShot();
        trashCollected.Add(trash.type);
        gameUI.AddLog(string.Format("<color=lime><b>Trash disposed</b>\t({0})</color>\t\t S+{1}{2} \tT+{3} \tC+{4} ({5:f3})", trash.impact, GamePoint.Score.GetScore(trash.type), (trash.byNPC ? " (+" + GamePoint.Score.npcBonus + ")" : ""), GamePoint.Time.GetTime(trash.type), GamePoint.Charisma.GetCharisma(trash.type), charisma));
        ShowPopUp(2, trash.gameObject.transform, additionalScore, (int)((time + additionalTime > timeMaximum) ? time - additionalTime : additionalTime));
    }

    public void LitterTrash(TrashItem trash) {
        if (!trash.byNPC) AudioManager.instance.Play("Trash_Litter");
        int additionalScore = GamePoint.Score.GetPenalty(trash.type);
        AddScore(additionalScore);
        //AddTime(0);   
        AddCharisma(GamePoint.Charisma.GetPenalty(trash.type));
        ShowPopUp(1, trash.gameObject.transform, additionalScore);
        gameUI.AddLog(string.Format("<color=red><b>Trash littered</b>\t\t({0})</color>\t\t S{1} \tC{2} ({3:f3})", trash.impact, GamePoint.Charisma.GetPenalty(trash.type), GamePoint.Score.GetPenalty(trash.type), charisma));
    }

    public void PassTrash(TrashItem trash) {
        AudioManager.instance.Play("Trash_Juggle");
        int additionalScore = GamePoint.Score.passBonus;
        AddScore(additionalScore);
        //AddTime(0);
        AddCharisma(0);
        gameUI.AddLog(string.Format("<color=orange><b>Trash passed</b>\t\t({0})</color>\t\t S+{1}", trash.impact, GamePoint.Score.passBonus));
        ShowPopUp(1, trash.gameObject.transform, additionalScore);
    }

    public void JuggleTrash(TrashItem trash) {
        AudioManager.instance.Play("Trash_Juggle");
        int additionalScore = GamePoint.Score.juggleBonus;
        AddScore(additionalScore);
        //AddTime(0);
        AddCharisma(0);
        gameUI.AddLog(string.Format("<color=orange><b>Trash juggled</b>\t\t({0})</color>\t\t S+{1}", trash.impact, GamePoint.Score.juggleBonus));
        ShowPopUp(1, trash.gameObject.transform, additionalScore);
    }

    public void NeutralizeNPC(bool success, float required, float randomized, Transform location) {
        if (success) {
            AudioManager.instance.Play("Neut_Success");
            AddScore(GamePoint.Score.npcNeutralize);
            AddTime(GamePoint.Time.npcNeutralize);
            AddCharisma(0);
            gameUI.AddLog(string.Format("<color=aqua><b>Neutralize NPC</b>\t(S)</color>\t\t S+{0} \tT+{1} \tC:({2:f3} [{3:f3}/{4:f3}])", GamePoint.Score.npcNeutralize, GamePoint.Time.npcNeutralize, charisma - required, randomized, GamePoint.Charisma.fixedChance));
            ShowPopUp(3, location, 1);
        } else {
            AudioManager.instance.Play("Neut_Fail");
            gameUI.AddLog(string.Format("<color=aqua><b>Neutralize NPC</b>\t(F)</color>\t\t C:({0:f3} [{1:f3}/{2:f3}])", charisma - required, randomized, GamePoint.Charisma.fixedChance));
            ShowPopUp(3, location, 0);
        }
    }


    private void AddTime(float additional) {
        if (time + additional > timeMaximum) time = timeMaximum;
        else time += additional;
    }

    private void AddScore(float additional) {
        score += (int)additional;
    }

    private void AddCharisma(float additional) {
        charisma += additional;
        //if (charisma > 1) charisma = 1;
        if (charisma < 0) charisma = 0;
    }

    public void ShowPopUp(int mode, Transform location, int score = 0, int time = 0) {
        switch (mode) {
            case 0:     // time+
            popUpController.TimePop(location, time);
            break;
            case 1:     // score+
            popUpController.ScorePop(location, score);
            break;
            case 2:     // time+ score+
            popUpController.TimeScorePop(location, time, score);
            break;
            case 3:     // npcsuccess
            popUpController.NPCPop(location, score != 0);
            break;
            default:
            break;
        } 
    }
}
