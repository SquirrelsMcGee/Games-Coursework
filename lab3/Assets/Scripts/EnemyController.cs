using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float turnSpeed = 90;

    public float viewDistance = 10;

    private GameObject player;
    private GameObject weapon;
    private GameObject body;

    public LayerMask viewMask;

    [Header("Shooting Settings")]
    public GameObject shot;
    public float fireRate = 0.5f;

    private float nextFire = 0;

    [Header("Object References")]
    public Transform shotTransform;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        //weapon = transform.Find("/Body/Weapon");
        weapon = GameObject.Find("Weapon");
        body = GameObject.Find("Body");
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 playerPos = player.transform.position;
        Vector3 lookTarget = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        Vector3 aimTarget = new Vector3(playerPos.x, shotTransform.transform.position.y, playerPos.z);
        transform.LookAt(lookTarget);
        shotTransform.LookAt(aimTarget);

        if (CanSeePlayer())
        {
            // Do something
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotTransform.position, shotTransform.rotation);
            }
        }
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= viewDistance)
        {
            if (!Physics.Linecast(transform.position, player.transform.position, viewMask))
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dir = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        while(Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
