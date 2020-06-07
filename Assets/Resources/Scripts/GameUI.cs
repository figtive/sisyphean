using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// MUST BE PLACED ON INDEX 0 ON PARENT"S UICONTROLLER
public class GameUI : AUIPage {

    public ScoreField scoreField;
    public TimerField timerField;
    public Text logField;
    public Image rightBackground;
    public Image rightImage;
    public Sprite icnThrow;
    public Sprite icnDirect;

    private Queue<string> logs;

    void Awake() {
        logs = new Queue<string>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.Escape)) Pause();
        if (GameManager.instance.hasTrash) {
            rightImage.sprite = icnThrow;
            rightBackground.color = Color.Lerp(rightBackground.color, Color.red, 0.25f);
        } else {
            rightImage.sprite = icnDirect;
            rightBackground.color = Color.Lerp(rightBackground.color, Color.cyan, 0.25f);
        }
    }

    public void Pause() {
        GameManager.instance.PauseGame(true);
        uiController.Open(1);
    }

    private float lastTime = GameManager.instance.timeInitial;
    public void ShowTime(float time) {
        lastTime = Mathf.Lerp(lastTime, time, 0.3f);
        timerField.SetTimeScale(lastTime / GameManager.instance.timeMaximum);
    }

    private float lastScore = 0;
    public void ShowScore(int score) {
        lastScore = Mathf.Lerp(lastScore, score, 0.25f);
        scoreField.ShowScore((int) lastScore);
    }

    public void AddLog(string text) {
        logs.Enqueue(text);
        if (logs.Count > 10) logs.Dequeue();
        logField.text = string.Join("\n", logs.ToArray());
    }
}
