using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    int currentNode;
    int previousNode;

    int currentNode2;
    int previousNode2;
    public bool PatrolsTwoPoints;
    public enum EnemyState
    {
        patrol,
        chase,
        twoPointer
    };

    EnemyState enemyState = EnemyState.patrol;
    void Start()
    {
        Debug.Log(GameManager.gm.player.transform.position);
        target = GameManager.gm.player.transform;
        agent = GetComponent<NavMeshAgent>();

        currentNode = Random.Range(0, GameManager.gm.nodes.Length);
        previousNode = currentNode;

        enemyState = EnemyState.twoPointer;

        currentNode2 = Random.Range(0, GameManager.gm.twoPointNodes.Length);
        previousNode2 = currentNode2;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.patrol: Patrol(); break;
            case EnemyState.chase: Chase(); break;
            case EnemyState.twoPointer: TwoPointPatrol(); break;
            default: break;
        }
        if (Vector3.Distance(transform.position, GameManager.gm.twoPointNodes[currentNode2].position) < 1)
        {
            currentNode2 = Random.Range(0, GameManager.gm.twoPointNodes.Length);
            while (currentNode2 == previousNode2)
            {
                currentNode2 = Random.Range(0, GameManager.gm.twoPointNodes.Length);
            }
            previousNode2 = currentNode2;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PatrolPoint"))
        {
            if (PatrolsTwoPoints)
            {
                Debug.Log("triggered");

                currentNode2 = Random.Range(0, GameManager.gm.twoPointNodes.Length);
                while (currentNode2 == previousNode2)
                {
                    currentNode2 = Random.Range(0, GameManager.gm.twoPointNodes.Length);
                }
                previousNode2 = currentNode2;
            }
            else
            {
                currentNode = Random.Range(0, GameManager.gm.nodes.Length);
                while (currentNode == previousNode)
                {
                    currentNode = Random.Range(0, GameManager.gm.nodes.Length);
                }
                previousNode = currentNode;
            }
        }
        if (other.CompareTag("Player"))
        {
            Debug.Log("spotted");
            enemyState = EnemyState.chase;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyState = EnemyState.patrol;
        }
        if (other.CompareTag("Player") && PatrolsTwoPoints)
        {
            enemyState = EnemyState.twoPointer;
        }
    }
    void Patrol()
    {
        if (!PatrolsTwoPoints)
        { agent.destination = GameManager.gm.nodes[currentNode].position; }
        
    }
    void TwoPointPatrol()
    {
        GameManager.gm.twoPointPatroller.GetComponent<NavMeshAgent>().destination = GameManager.gm.twoPointNodes[currentNode2].position;
    }
    void Chase()
    { 
        agent.destination = target.position;
    }
}
