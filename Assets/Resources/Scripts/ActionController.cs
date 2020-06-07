using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour {

    private TrashCheck tc;
    private ThrowController thrower;
    public PointTargetter targetter;
    public Transform handTip;
    private GameObject trashItem;
    public Projector circleRadius;
    private float defaultCircleScale;
    public float maxRadius = 10f;
    public bool focusThrow = false;

    public bool allowFetch;
    public GameObject manualFetch;
    public TrashImpact trashImpact;
    public bool noTarget = true;

    public bool isNPC = true;

    private bool showCircle;

    void Start() {
        tc = GetComponentInChildren<TrashCheck>();
        thrower = GetComponent<ThrowController>();
        if (circleRadius != null) defaultCircleScale = circleRadius.orthographicSize;
        if (circleRadius != null) showCircle = false;
        allowFetch = true;
        manualFetch = null;
        trashImpact = TrashImpact.identity;
    }

    void Update() {
        if (allowFetch) {
            GameObject target = tc.GetTrash();
            if (manualFetch != null) {
                if (target == manualFetch) CollectTrash(target);
            } else if ((trashItem == null && target != null)) {
                CollectTrash(target);
            }
        }
        if (circleRadius != null) {
            if (showCircle) {
                if (HasTrash()) {
                    circleRadius.material.color = Color.Lerp(circleRadius.material.color, Color.red, 0.25f);
                } else {
                    circleRadius.material.color = Color.Lerp(circleRadius.material.color, Color.cyan, 0.25f);
                }
            } else {
                circleRadius.material.color = Color.Lerp(circleRadius.material.color, Color.black, 0.25f);
            }
        }
        thrower.showLine = !noTarget;
    }

    void FixedUpdate() {
        if (trashItem != null) {
            // move trash item to hand with given position and correct orientation
            trashItem.transform.position = Vector3.Lerp(trashItem.transform.position, handTip.transform.position, 10f * Time.fixedDeltaTime);
            trashItem.transform.rotation = Quaternion.Lerp(trashItem.transform.rotation, Quaternion.Euler(-90, 0, 0), 10f * Time.fixedDeltaTime);
        }
    }

    public void Target(float hPos, float vPos) {
        float radius = trashImpact.maxRadiusMul * maxRadius;
        Vector2 moveVector = Vector2.ClampMagnitude(new Vector2(hPos, vPos) * radius, radius);
        targetter.Move(moveVector, transform.position);     // invert targetter if has trash TEMP HasTrash()
        if (!HasTrash()) {
            if (noTarget && moveVector != Vector2.zero) {
                NPCManager.instance.AttachNPC();
            } else if (!noTarget && moveVector == Vector2.zero) {
                NPCManager.instance.ReleaseNPC();
            }
        }
        noTarget = moveVector == Vector2.zero;
    }

    void CollectTrash(GameObject target) {
        TrashItem targetTI = target.GetComponent<TrashItem>();
        if (!targetTI.pickedUp) {
            if (isNPC && !targetTI.byNPC && targetTI.thrown) {
                // trash tossed from player to NPC
                GameManager.instance.PassTrash(targetTI);
            }
             if (!isNPC && !targetTI.byNPC && targetTI.thrown) {
                // trash juggled
                GameManager.instance.JuggleTrash(targetTI);
            }
            CameraManager.instance.Remove(target.transform);
            trashItem = target;
            Rigidbody targetRB = trashItem.GetComponent<Rigidbody>();
            targetTI.pickedUp = true;
            targetTI.pickable = false;
            tc.inBound.Remove(target);
            trashImpact = targetTI.impact;
            targetRB.isKinematic = true;
            trashItem.transform.parent = handTip.transform;
            thrower.item = trashItem.GetComponent<Rigidbody>();
            if (circleRadius != null) circleRadius.orthographicSize = defaultCircleScale * trashImpact.maxRadiusMul;
            if (circleRadius != null) showCircle = true;
        }
    }

    public void ThrowTrash() {
        if (trashItem != null) {
            AudioManager.instance.Play("Trash_Throw");
            trashItem.GetComponent<TrashItem>().pickedUp = false;
            trashItem.GetComponent<TrashItem>().byNPC = isNPC;
            trashItem.GetComponent<TrashItem>().thrown = true;
            trashItem.GetComponent<Rigidbody>().isKinematic = false;
            trashItem.GetComponent<TrailRenderer>().enabled = true;
            trashItem.transform.parent = null;
            if (focusThrow) CameraManager.instance.Add(trashItem.transform);
            thrower.Launch();
            trashItem = null;
            trashImpact = TrashImpact.identity;
            if (circleRadius != null) showCircle = false;
        }
    }

    public void DropTrash() {
        if (trashItem != null) {
            trashItem.GetComponent<TrashItem>().pickedUp = false;
            trashItem.GetComponent<TrashItem>().byNPC = isNPC;
            trashItem.GetComponent<TrashItem>().thrown = false;
            trashItem.GetComponent<Rigidbody>().isKinematic = false;
            trashItem.transform.parent = null;
            thrower.Drop();
            trashItem = null;
            trashImpact = TrashImpact.identity;
        }

    }

    public bool HasTrash() {
        return trashItem != null;
    }

    public float GetRadius() {
        return maxRadius * trashImpact.maxRadiusMul;
    }

    public float UpdateMobility(float initial) {
        return initial * trashImpact.speedMul;
    }
}
