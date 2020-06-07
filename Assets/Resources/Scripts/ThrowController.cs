using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowController : MonoBehaviour {

    public Rigidbody item;
    public Transform target;
    public LineRenderer line;

    public float h = 25;
    public float gravity = -18;
    public int resolution = 30;

    public bool drawLine = true;
    public bool showLine;
    public bool debugPath;
    private Vector3 startPos;
    private LaunchData launchData;

    void Start() {
        if (target == null) target = GameManager.instance.trashBin.throwTarget;
        if (line != null) line = Instantiate(line, Vector3.zero, Quaternion.identity);
        line.enabled = false;
    }

    void Update() {
        if (item != null) {
            launchData = CalculateLaunchData();
            if (drawLine || debugPath) DrawPath(launchData);
        }
    }

    public void Launch() {
        launchData = CalculateLaunchData();
        Physics.gravity = Vector3.up * gravity;
        item.useGravity = true;
        item.velocity = launchData.initialVelocity;
        item.angularVelocity = new Vector3(5, 5, 5);
        item = null;
        line.enabled = false;
    }

    public void Drop() {
        Physics.gravity = Vector3.up * gravity;
        item.useGravity = true;
        item = null;
        line.enabled = false;
    }

    LaunchData CalculateLaunchData() {
        float displacementY = target.position.y - item.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - item.position.x, 0, target.position.z - item.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    void DrawPath(LaunchData launchData) {
        Vector3 previousDrawPoint = startPos = item.gameObject.GetComponent<Transform>().position;
        List<Vector3> pointArr = new List<Vector3>();
        int i;
        for (i = 0; i <= resolution; i++) {
            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = startPos + displacement;
            RaycastHit hit;
            if (Physics.Raycast(previousDrawPoint, drawPoint - previousDrawPoint, out hit, Vector3.Magnitude(drawPoint - previousDrawPoint), ~(7 << 8))) {
                drawPoint = hit.point;
                pointArr.Add(hit.point - startPos);
                break;
            }
            pointArr.Add(displacement);
            previousDrawPoint = drawPoint;
        }

        if (drawLine && showLine) {
            line.transform.position = startPos;
            line.positionCount = pointArr.Count;
            line.SetPositions(pointArr.ToArray());
            line.enabled = true;
        }
    }

    //void DrawPath(LaunchData launchData) {
    //    Vector3 previousDrawPoint = startPos = item.gameObject.GetComponent<Transform>().position;
    //    Vector3[] pointArr = new Vector3[resolution + 1];
    //    for (int i = 0; i <= resolution; i++) {
    //        float simulationTime = i / (float)resolution * launchData.timeToTarget;
    //        Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
    //        pointArr[i] = displacement;
    //        Vector3 drawPoint = startPos + displacement;
    //        if (debugPath) Debug.DrawLine(previousDrawPoint, drawPoint, Color.red);
    //        previousDrawPoint = drawPoint;
    //    }

    //    if (drawLine && showLine) {
    //        line.transform.position = startPos;
    //        line.positionCount = resolution + 1;
    //        line.SetPositions(pointArr);
    //        line.enabled = true;
    //    }
    //}

    struct LaunchData {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget) {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
