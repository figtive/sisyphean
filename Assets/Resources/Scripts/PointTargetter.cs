using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTargetter : MonoBehaviour {

    public Transform targetPos;
    public Projector targetProjector;
    public float rayHeight = 10f;

    private Vector3 movePos;
    private bool free = true;
    private Color targetColor;
    
	void Start () {
        movePos = new Vector3(0, rayHeight, 0);
    }
	
	void Update () {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, ~(7 << 8))) {
            targetPos.position = hit.point;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        }

        if (free) {
            targetProjector.material.color = Color.Lerp(targetProjector.material.color, Color.black, 0.25f);
        } else {
            if (GameManager.instance.hasTrash) {
                targetProjector.material.color = Color.Lerp(targetProjector.material.color, Color.red, 0.25f);
            } else {
                targetProjector.material.color = Color.Lerp(targetProjector.material.color, Color.cyan, 0.25f);
            }
        }
	}

    public void Move(Vector2 moveVector, Vector3 parentPos, bool inverted = false) {
        free = moveVector == Vector2.zero;
        if (inverted) moveVector *= -1;
        movePos = new Vector3(moveVector.x + parentPos.x, rayHeight, moveVector.y + parentPos.z);
    }

    void FixedUpdate() {
        transform.position = Vector3.Slerp(transform.position, movePos, (free ? 7f : 16f) * Time.fixedDeltaTime);
    }
}
