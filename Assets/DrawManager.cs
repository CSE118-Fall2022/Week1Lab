using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour {
    public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public List<Vector3> mousePositions;

    public Transform trackingSpace;
    public OVRInput.Controller controller;


    void Start() {
    }

    void Update() {
        OVRInput.Update();
        OVRInput.FixedUpdate();

        if (OVRInput.GetDown(OVRInput.Button.One)
        || OVRInput.GetDown(OVRInput.Button.Three)) {
            Vector3 rightControllerPos = trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            Debug.Log(rightControllerPos.ToString());
            CreateLine(rightControllerPos);
        }

        if (OVRInput.Get(OVRInput.Button.One)
        || OVRInput.Get(OVRInput.Button.Three)) {
            Vector3 rightControllerPos = trackingSpace.TransformPoint(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch));
            Debug.Log(rightControllerPos.ToString());
            UpdateLine(GetCollision(rightControllerPos));
        }
    }

    void CreateLine(Vector3 controllerPos) {
        currentLine = Instantiate(linePrefab, linePrefab.transform.position, linePrefab.transform.rotation);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        mousePositions.Clear();
        Vector3 mousePos = GetCollision(controllerPos);
        mousePositions.Add(mousePos);
        mousePositions.Add(mousePos);
        lineRenderer.SetPosition(0, mousePositions[0]);
        lineRenderer.SetPosition(1, mousePositions[1]);
    }

    void UpdateLine(Vector3 newMousePos) {
        mousePositions.Add(newMousePos);

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newMousePos);
    }

    Vector3 GetCollision(Vector3 pos) {
        RaycastHit hit;
        if (Physics.Raycast(pos, trackingSpace.TransformDirection(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)), out hit)) {
            // Add vector relative to Canvas transform
            Vector3 newMousePos = (hit.point);
            return newMousePos;
        }

        return Vector3.forward;
    }
}
