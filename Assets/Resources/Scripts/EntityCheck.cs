using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCheck : MonoBehaviour {

    public bool hitNPC = false;
    public bool hitPlayer = false;

    void OnTriggerEnter(Collider col) {
        if (col.tag == "playerRadius") {
            hitPlayer = true;
        }
        if (col.tag == "npcRadius") {
            hitNPC = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.tag == "npcRadius" || col.tag == "playerRadius") {
            hitNPC = false;
            hitPlayer = false;
        }
    }
}
