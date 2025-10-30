using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BaseAIBehaviour : MonoBehaviour
{
    [HideInInspector]public AIPath aiPathfinder;
    [HideInInspector] public AIDestinationSetter destinationSetter;
    [HideInInspector] public EnemyData enemyData;
    [HideInInspector] public Vector3 spawnPosition;
    GameObject currentEndPoint;
    public float attackRange;
    public AIStateMachine aiStateMachine;
    public LayerMask potentialTargetsLayerMask;
    public float detectionRadius;
    public float pursueDistance;

    [HideInInspector] public float health;
    public float maxHealth;
    public EnemyHealthDisplay healthDisplay;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool initiallyFacingLeft;
    public GameObject damagePopupPrefab;
    public delegate void OnPursue();
    public OnPursue onPursueFunctions;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    Color defaultColor;
    Coroutine damageIndicator_co;
    public virtual void Start()
    {
        aiPathfinder = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiStateMachine = new(this);
        spawnPosition = transform.position;
        aiStateMachine.SetState(AIStateMachine.AIStates.Idle);
         health =  maxHealth;
        initiallyFacingLeft = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }
    public virtual  void Attack() { }

    public void DetectTargetsInRange()
    {
        List<RaycastHit2D> hits = Physics2D.CircleCastAll(transform.position, detectionRadius, Vector2.zero, 0, potentialTargetsLayerMask).ToList();
        if (hits.Count > 0)
        {
            hits.FirstOrDefault((x) => destinationSetter.target = x.collider.gameObject.transform.parent);
            target = hits.FirstOrDefault().transform;
            aiStateMachine.SetState(AIStateMachine.AIStates.Pursuing);
        }
        else
        {
            target = null;
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
    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        healthDisplay.UpdateHealthBar(health, maxHealth);
       damageIndicator_co= StartCoroutine(DamageIndicator());
        StartCoroutine(DamagePopup(damageTaken));
        if(health < 1)
        {
            (FindObjectOfType<RoomGenerator>().currentRoom_Data as RoomGenerator.EnemyRoomData).enemiesInRoom.Remove(gameObject);
            FindObjectOfType<RoomGenerator>().playerController.souls += enemyData.soulDrop;
            FindObjectOfType<RoomGenerator>().playerController.playerScreenRef.UpdateCurrencies(FindObjectOfType<RoomGenerator>().playerController.souls, 0, false);
            Destroy(gameObject);
        }
    }

    IEnumerator DamageIndicator()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = defaultColor;
    }

    IEnumerator DamagePopup(float damageTaken)
    {
        GameObject damagePopup1 = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
        
        damagePopup1.SetActive(true);
        Transform damagePopupText=  damagePopup1.transform.GetChild(0);
        damagePopup1.transform.SetParent(transform);
        damagePopupText.GetComponent<TextMeshPro>().text = damageTaken.ToString();
        damagePopup1.transform.position = transform.position;
        damagePopup1.transform.SetAsFirstSibling();
        yield return new WaitForSeconds(0.45f);
        Destroy(damagePopup1);
    }

    public void Stunned(bool status)
    {
        if(status)
        {
            aiStateMachine.SetState(AIStateMachine.AIStates.Stunned);
        }
        else
        {
            aiStateMachine.SetState(AIStateMachine.AIStates.Idle);
        }
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
    //public void OnDrawGizmos()
    //{
    //    if(aiStateMachine.currentAIState == AIStateMachine.AIStates.Idle /*|| aiStateMachine.currentAIState == AIStateMachine.AIStates.Retreating*/)
    //    {
    //        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //    }
    //}
}
