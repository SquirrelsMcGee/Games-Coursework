using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StateAttack : IState
{
    EnemyController owner;
    NavMeshAgent agent;
    public StateAttack(EnemyController owner) { this.owner = owner; }
    public void Enter()
    {
        Debug.Log("entering attack state");
        agent = owner.GetComponent<NavMeshAgent>();
        if (owner.seenTarget)
        {
            agent.destination = owner.lastSeenPosition;
            agent.isStopped = false;

            owner.FireShot();
        }
    }

    public void Execute()
    {
        //Debug.Log("updating attack state");
        agent.destination = owner.lastSeenPosition;
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance < 5.0f)
        {
            agent.isStopped = true;
            //agent.transform.LookAt(owner.lastSeenPosition);
            agent.transform.LookAt((owner.lastSeenPosition + owner.predictedPosition) / 2.0f);
        }
        if (owner.seenTarget != true)
        {
            Debug.Log("lost sight");
            // search for the player
            agent.transform.LookAt((owner.lastSeenPosition + owner.predictedPosition) / 2.0f);
            owner.stateMachine.ChangeState(new StateSearching(owner));
        }

        owner.FireShot();
        
        
 }
    public void Exit()
    {
        Debug.Log("exiting attack state");
        agent.isStopped = true;
    }
}