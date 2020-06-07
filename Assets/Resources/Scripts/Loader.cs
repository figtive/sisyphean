using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject appManager;
    public GameObject audioManager;

    void Awake() {
        Application.targetFrameRate = 60;
        if (AudioManager.instance == null) Instantiate(audioManager);
        if (AppManager.instance == null) Instantiate(appManager);
    }
}

/*
 * 
 * Loader
 *  |-> AudioManager
 *  |-> AppManager
 *      |-> GameManager
 *      |-> CameraManager
 *      |-> EntityManager
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
