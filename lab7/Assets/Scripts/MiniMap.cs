using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

    public Transform[] cars;
    public Transform[] trackers;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < trackers.Length; i++)
        {
            Transform car = cars[i];
            Transform tracker = trackers[i];

            Vector3 pos = car.position;
            Quaternion rot = car.rotation;

            Vector3 newPos = new Vector3(pos.x, 150, pos.z);
            Vector3 newRot = new Vector3(0.0f, car.rotation.eulerAngles.y, 0.0f);

            tracker.eulerAngles = newRot;
            tracker.position = newPos;
        }
    }
}
