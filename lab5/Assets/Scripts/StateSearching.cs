using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StateSearching : IState
{
    EnemyController owner;
    NavMeshAgent agent;
    public StateSearching(EnemyController owner) { this.owner = owner; }

    private int maxSearchTime = 5;
    private float timeSearching = 0.0f;
    public void Enter()
    {
        Debug.Log("entering search state");
        agent = owner.GetComponent<NavMeshAgent>();
        if (owner.seenTarget)
        {
            agent.destination = (owner.lastSeenPosition + owner.predictedPosition) / 2.0f;
            agent.isStopped = false;
        }
    }

    private float x = 0;
    private float cosx = 0;

    public void Execute()
    {
        //Debug.Log("updating search state");

        agent.destination = (owner.lastSeenPosition * 0.3f) + (owner.predictedPosition * 0.7f);
        agent.isStopped = false;
        
        if (!agent.pathPending && agent.remainingDistance < 2.0f)
        {
            agent.isStopped = true;
        }
        if (owner.seenTarget == true)
        {
            // found the player
            owner.stateMachine.ChangeState(new StateAttack(owner));
        }
        if (timeSearching >= maxSearchTime)
        {
            // Attempt to find the player has failed
            owner.stateMachine.ChangeState(new StatePatrol(owner));
        }

        if (!owner.seenTarget)
        {
            agent.destination = (owner.predictedPosition);
            // Not seen the player
            timeSearching += Time.deltaTime;
            x += Time.deltaTime;
            cosx = Mathf.Cos(x) / 100.0f;

            agent.transform.RotateAround(Vector3.up, cosx);
        }
        else
        {
            timeSearching = 0;
            cosx = 0;
        }
    }
    public void Exit()
    {
        Debug.Log("exiting search state");
        agent.isStopped = true;
    }
}