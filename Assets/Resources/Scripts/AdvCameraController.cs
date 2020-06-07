using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AdvCameraController : MonoBehaviour {

    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimit = 50f;
    public bool lookAt = false;

    private Vector3 velocity;
    private Camera cam;

    void Start() {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate() {
        if (targets.Count == 0) return;
        Move();
        Zoom();
    }

    void Move() {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        if (lookAt) transform.LookAt(centerPoint);
    }

    void Zoom() {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetMaxDistance() / zoomLimit);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.fixedDeltaTime);
    }

    float GetMaxDistance() {
        if (targets.Count == 1) return 0f;
        return GetBounds().size.x;
    }
    Vector3 GetCenterPoint() {
        if (targets.Count == 1) return targets[0].position;
        return GetBounds().center;
    }

    Bounds GetBounds() {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++) {
            if (targets[i] != null) bounds.Encapsulate(targets[i].position);
        }
        return bounds;
    }
}
