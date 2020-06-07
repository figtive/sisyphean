using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : AUIPage {

    public GameObject nosplash;
    public GameObject[] splashes;

    public override void Start() {
        base.Start();
        if (AppManager.instance.fromStart) {
            AppManager.instance.SetFromStart(false);
            GetComponent<Animator>().Play(0);
            nosplash.SetActive(false);
            foreach (GameObject g in splashes) g.SetActive(true);
            StartCoroutine(MusicDelay(13.250f));
        } else {
            foreach (GameObject g in splashes) g.SetActive(false);
            nosplash.SetActive(true);
        }
    }

    public override void OnOpen() {
        base.OnOpen();
        if (AppManager.instance.fromStart) {
            AppManager.instance.SetFromStart(false);
            GetComponent<Animator>().Play(0);
            nosplash.SetActive(false);
            foreach (GameObject g in splashes) g.SetActive(true);
            StartCoroutine(MusicDelay(13.250f));
        } else {
            foreach (GameObject g in splashes) g.SetActive(false);
            //GetComponent<Animator>().enabled = false;
            //GetComponent<Animator>().StopPlayback();
            nosplash.SetActive(true);
        }
    }

    public void PlayGame() {
        uiController.Open(2);
        // AppManager.instance.BeginGame(AppManager.instance.currentLevel);
    }

    public void OpenSettings() {
        uiController.Open(1, 0);
        Debug.Log("SETTINGGSS");
    }
    
    public void ExitGame() {
        AppManager.instance.ExitGame();
    }

    IEnumerator MusicDelay(float time) {
        AudioManager.instance.Pause("Theme", true);
        yield return new WaitForSeconds(time);
        AudioManager.instance.Pause("Theme", false);
    }
    //IEnumerator QueueAnim(params AnimationClip[] anim) {
    //    int index = 0;
    //    animation.clip = anim[index];
    //    while (index < anim.Length) {
    //        animation.Play;
    //        yield return new WaitForSeconds(anim[index].length);
    //        index++;
    //        animation.clip = anim[index];
    //    }
    //}
}
