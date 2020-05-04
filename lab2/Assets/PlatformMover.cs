using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{

    public Transform waypoint1;
    public Transform waypoint2;
    public GameObject platform;

    public float speed = 1.0f;

    private void Start()
    {
    }

    void Update()
    {
        platform.transform.position = Vector3.Lerp(waypoint1.position, waypoint2.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(waypoint1.position, 0.1f);
        Gizmos.DrawSphere(waypoint2.position, 0.1f);
    }
}
