using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0, -10f);
    public float smoothTime = 0.25f;

    private Vector3 velocity = Vector3.zero;
    private Transform target;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate() {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
