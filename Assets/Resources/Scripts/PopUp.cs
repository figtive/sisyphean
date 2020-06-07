using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour {

    private Transform lockPos;
    public TextMeshProUGUI timeField;
    public TextMeshProUGUI scoreField;

    void Start () {
        AnimatorClipInfo[] clipInfo = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    void Update() {
        this.transform.position = CameraManager.instance.GetCamera().WorldToScreenPoint(lockPos.position);
    }
	
	public void SetTime(int time) {
        timeField.SetText(((time > 0) ? "+ " : "- ") + Mathf.Abs(time).ToString());
        if (time < 0) timeField.color = Color.red;
    }

    public void SetScore(int score) {
        scoreField.SetText(((score > 0) ? "+ " : "- ") + Mathf.Abs(score).ToString());
        if (score < 0) scoreField.color = Color.red;
    }

    public void LockPosition(Transform position) {
        this.lockPos = position;
    }
}
