using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseAIBehaviour : MonoBehaviour
{
    [HideInInspector]public AIPath aiPathfinder;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public Vector3 spawnPosition;
    GameObject currentEndPoint;
    public AIStateMachine aiStateMachine;
    public LayerMask potentialTargetsLayerMask;
    public float detectionRadius;
    public float pursueDistance;
    void Start()
    {
        aiPathfinder = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiStateMachine = new(this);
        spawnPosition = transform.position;
        aiStateMachine.SetState(AIStateMachine.AIStates.Idle);
    }
    public void DetectTargetsInRange()
    {
        List<RaycastHit2D> hits = Physics2D.CircleCastAll(transform.position, detectionRadius, Vector2.zero, 0, potentialTargetsLayerMask).ToList();
        if (hits.Count > 0)
        {
            hits.FirstOrDefault((x) => destinationSetter.target = x.collider.gameObject.transform.parent);
            aiStateMachine.SetState(AIStateMachine.AIStates.Pursuing);
        }
    }
    public void SetEndPoint(Vector3 position, AIStateMachine.AIStates nextState)
    {
        aiPathfinder.destination = position;
        GameObject targetPointsGroup = (GameObject.Find("PathEndPointsList")) ? GameObject.Find("PathEndPointsList") : new() { name = "PathEndPointsList" };
        if(currentEndPoint != null)
        {
            Destroy(currentEndPoint);
        }
        currentEndPoint = new() { name = "EndPoint_" + gameObject.name};
        currentEndPoint.transform.position = position;
        currentEndPoint.transform.parent = targetPointsGroup.transform.parent = targetPointsGroup.transform;
        BoxCollider2D endPointCollder = currentEndPoint.AddComponent<BoxCollider2D>();
        endPointCollder.size = Vector2.one * 0.2f;
        endPointCollder.isTrigger = true;
        EndPointScript endPointScript = currentEndPoint.AddComponent<EndPointScript>();
        endPointScript.movingAgent = this.gameObject;
        endPointScript.stateAfterReachingPoint = nextState;
    }
    // Update is called once per frame
    void Update()
    {
        aiStateMachine.UpdateState(true,false);
    }
    void FixedUpdate()
    {
        aiStateMachine.UpdateState(true, true);
    }
    public void OnDrawGizmos()
    {
        if(aiStateMachine.currentAIState == AIStateMachine.AIStates.Idle /*|| aiStateMachine.currentAIState == AIStateMachine.AIStates.Retreating*/)
        {
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
