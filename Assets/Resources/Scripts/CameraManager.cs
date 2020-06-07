using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance = null;

    public AdvCameraController cam;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        InitCam();
    }

    public void InitCam() {
    }

    public void AttachCam(AdvCameraController cam) {
        instance.cam = cam;
    }

    public void Add(Transform target) {
        instance.cam.targets.Add(target);
    }

    public bool Remove(Transform target) {
        if (target != null) return instance.cam.targets.Remove(target);
        else return false;
    }

    public Camera GetCamera() {
        return cam.gameObject.GetComponent<Camera>();
    }
}
