using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    NavMeshAgent agent;

    public Transform waypointGroup;
    public Waypoint waypoint { get; private set; }

    // Player GameObject
    public GameObject target;
    public bool seenTarget { get; private set; } = false;

    private SphereCollider sphereCollider;

    public float sightFov = 30.0f;

    public Vector3 lastSeenPosition { get; private set; }
    public Vector3 predictedPosition { get; private set; }

    public StateMachine stateMachine { get; private set; } = new StateMachine();

    [Header("Shooting Settings")]
    public GameObject shot;
    public float fireRate = 1.0f;
    private float deltaTime = 0.0f;

    [Header("Object References")]
    public Transform shotTransform;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        if (waypointGroup.childCount > 0) waypoint = waypointGroup.GetChild(0).GetComponent<Waypoint>();

        agent.destination = waypoint.transform.position;

        sphereCollider = GetComponent<SphereCollider>();

        transform.forward = waypoint.transform.forward;

        stateMachine.ChangeState(new StatePatrol(this));
    }

    private void OnValidate()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public float speed = 0.1f;


    public bool reversePathWhenComplete = false;
    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        stateMachine.Update();

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (waypoint != null)
            {
                Waypoint nextWaypoint;

                if (waypoint.nextWaypoint != null)
                {
                    nextWaypoint = waypoint.nextWaypoint;
                }
                else
                {
                    if (reversePathWhenComplete)
                    {
                        nextWaypoint = waypoint.previousWaypoint;
                    }
                    else
                    {
                        nextWaypoint = waypoint;
                    }
                }

                waypoint = nextWaypoint;
                agent.destination = waypoint.transform.position;

            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            // angle between us and the player
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            // reset whether we’ve seen the player
            seenTarget = false;
            RaycastHit hit;
            // is it less than our field of view
            if (angle <= sightFov * 0.6f)
            {
                // if the raycast hits the player we know
                // there is nothing in the way
                // adding transform.up raises up from the floor by 1 unit
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, sphereCollider.radius))
                {
                    if (hit.collider.gameObject == target)
                    {
                        // flag that we've seen the player
                        // remember their position
                        seenTarget = true;
                        lastSeenPosition = target.transform.position;

                        predictedPosition = target.transform.position + target.GetComponent<PlayerController>().speed;
                    } else
                    {
                        seenTarget = false;
                    }
                }
            } else
            {
                seenTarget = false;
            }
        }
    }

    public void FireShot()
    {
        if (deltaTime >= fireRate)
        {
            deltaTime = 0.0f;
            Instantiate(shot, shotTransform.position, transform.rotation);
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        
        if (agent != null) Gizmos.DrawSphere(agent.destination, 0.1f);

        Gizmos.color = Color.blue;

        //Debug.Log(sphereCollider == null);
        if (sphereCollider != null)
        {
            //Debug.Log("Okay");
            //
            
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            if (seenTarget)
                Gizmos.DrawLine(transform.position, lastSeenPosition);
            if (lastSeenPosition != Vector3.zero)
            {
                Gizmos.DrawSphere(lastSeenPosition, 0.3f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(predictedPosition, 0.3f);
                Gizmos.color = Color.blue;
            }

            Vector3 forwardLine = (transform.forward * sphereCollider.radius);
            Vector3 rightPeripheral = (Quaternion.AngleAxis(sightFov * 0.5f, Vector3.up) * transform.forward * sphereCollider.radius);
            Vector3 leftPeripheral = (Quaternion.AngleAxis(sightFov * 0.5f, Vector3.down) * transform.forward * sphereCollider.radius);


            // Translate vectors so they are relative to the object position
            forwardLine += transform.position;
            leftPeripheral += transform.position;
            rightPeripheral += transform.position;

            Gizmos.DrawLine(transform.position, forwardLine);
            Gizmos.DrawLine(transform.position, rightPeripheral);
            Gizmos.DrawLine(transform.position, leftPeripheral);
            // draw lines for the left and right edges of the field of view
        }
    }
}
