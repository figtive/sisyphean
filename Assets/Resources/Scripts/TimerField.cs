using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TimerField : MonoBehaviour {

    private TextMeshProUGUI timerText;
    public Image timerBar;
    public float maxLength;

    void Start() {

    }

    public void SetTimeScale(double scale) {
        timerBar.rectTransform.sizeDelta = new Vector2((float)(maxLength * scale), timerBar.rectTransform.rect.height);
    }
}
