using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinController : MonoBehaviour {

    public Transform throwTarget;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider col) {
        TrashItem item = col.gameObject.GetComponent<TrashItem>();
        if (item != null) {
            if (!item.pickedUp) {
                item.disabled = true;
                item.gameObject.GetComponent<ParticleSystem>().Play();
                CameraManager.instance.Remove(col.transform);
                GameManager.instance.DisposeTrash(item);
            }
        }
    }
}
