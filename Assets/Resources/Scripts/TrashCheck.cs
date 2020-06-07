using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCheck : MonoBehaviour {

    public List<GameObject> inBound = new List<GameObject>();

    public GameObject GetTrash() {
        GameObject target = FindReady();
        return target;
    }

    private GameObject FindReady() {
        GameObject target = null;
        float minDist = 1000f;
        foreach (GameObject trash in inBound) {
            if (trash != null) {
                if (trash.GetComponent<TrashItem>().Free()) {
                    float dist = Vector3.SqrMagnitude(trash.transform.position - transform.position);
                    if (dist <= minDist) {
                        target = trash;
                        minDist = dist;
                    }
                }
            }
        }
        return target;
    }

    void OnTriggerEnter(Collider col) {
        TrashItem item = col.gameObject.GetComponent<TrashItem>();
        if (item != null && !item.bounded) { item.bounded = true; inBound.Add(col.gameObject); }
    }

    void OnTriggerExit(Collider col) {
        inBound.Remove(col.gameObject);
        TrashItem item = col.gameObject.GetComponent<TrashItem>();
        if (item != null) { item.bounded = false; item.pickable = true; }
    }
}
