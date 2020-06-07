using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashMenu : AUIPage {

    public GameObject[] splashes;

    public override void OnOpen() {
        for (int i = 0; i < splashes.Length; i++) {
            Debug.Log("splash " + i.ToString());
            splashes[i].gameObject.SetActive(true);
            splashes[i].GetComponent<Animator>().Play(0);
            splashes[i].gameObject.SetActive(false);
            StartCoroutine(Wait(5));
        }
        uiController.Open(0, 0);
    }

    IEnumerator Wait(float s) {
        //for (int i = 0; i < splashes.Length; i++) {
        //    if (i == s) splashes[i].gameObject.SetActive(true);
        //    else splashes[i].gameObject.SetActive(false);
        //}
        //splashes[s].GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.Whi = WrapMode.Once;
        //splashes[s].GetComponent<Animator>().Play(0);
        yield return new WaitForSeconds(s);
    }
}
