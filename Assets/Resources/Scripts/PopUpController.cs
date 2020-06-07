using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour {

    public PopUp[] popups;

    public void Initialize() {
    }

    public PopUp CreatePopUp(int type, Transform location) {
        PopUp instance = Instantiate(popups[type]);
        instance.transform.SetParent(GameManager.instance.canvas.transform, false);
        instance.LockPosition(location);
        return instance;
    }

    public void TimePop(Transform location, int time) {
        PopUp pop = CreatePopUp(0, location);
        pop.SetTime(time);
    }

    public void ScorePop(Transform location, int score) {
        PopUp pop = CreatePopUp(1, location);
        pop.SetScore(score);
    }

    public void TimeScorePop(Transform location, int time, int score) {
        PopUp pop = CreatePopUp(2, location);
        pop.SetTime(time);
        pop.SetScore(score);
    }

    public void NPCPop(Transform location, bool success) {
        PopUp pop = success ? CreatePopUp(3, location) : CreatePopUp(4, location);

    }
}
